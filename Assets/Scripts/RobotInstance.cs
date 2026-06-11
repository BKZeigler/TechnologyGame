using UnityEngine;
using System.Collections.Generic;
using System;

[System.Serializable]
public class RobotInstance : IBuffTarget
{
    public RobotData data;
    public RobotCombat combat;

    // -------------------------
    // Persistent (saved) stats
    // -------------------------
    public CombatStats baseStats = new CombatStats();

    // -------------------------
    // Battle-only stats
    // -------------------------
    public CombatStats battleStats = new CombatStats();

    public float tempDamageMultiplier = 0f;
    private int nextAbilityIndex = 0;

    // Runtime abilities/passives/techs (IDs only)
    public Dictionary<int, AbilityData> abilityDict = new Dictionary<int, AbilityData>();
    public Dictionary<int, PassiveData> passiveDict = new Dictionary<int, PassiveData>();
    public List<int> technologyIDs = new List<int>();
    public List<Buff> activeBuffs = new List<Buff>();

    // Runtime per-ability state (battle only)
    public Dictionary<int, int> abilityStacks = new Dictionary<int, int>();

    // -------------------------
    // Constructor
    // -------------------------
    public RobotInstance(RobotData data)
    {
        this.data = data;

        // Load persistent stats
        baseStats.maxHealth     = data.baseHealth;
        baseStats.health        = data.baseHealth;
        baseStats.atkdamage     = data.baseAtkDamage;
        baseStats.abilitydamage = data.baseAbilityDamage;
        baseStats.abilityCount  = data.baseAbilityCount;
        baseStats.atkspd        = data.baseAtkSpeed;
        baseStats.castspd       = data.baseCastSpeed;
        baseStats.luck          = data.baseLuck;

        // Initialize battle stats
        InitializeBattleStats();

        // Load base abilities/passives
        foreach (int id in data.baseAbilityIDs)
            abilityDict[id] = AbilityDatabase.GetAbility(id);

        foreach (int id in data.basePassiveIDs)
            passiveDict[id] = PassiveDatabase.GetPassive(id);
    }

    // -------------------------
    // Battle Initialization
    // -------------------------
    public void InitializeBattleStats()
    {
        battleStats.CopyFrom(baseStats);

        tempDamageMultiplier = 0f;
        activeBuffs.Clear();
        abilityStacks.Clear();
    }

    // -------------------------
    // Tech merging (persistent)
    // -------------------------
    public void MergeTech(Technology tech)
    {
        technologyIDs.Add(tech.id);

        baseStats.maxHealth     += tech.stats[0];
        baseStats.health         = baseStats.maxHealth;
        baseStats.atkdamage     += 0.05 * tech.stats[1];
        baseStats.abilitydamage += 0.05 * tech.stats[2];
        baseStats.atkspd        += 0.01 * tech.stats[3];
        baseStats.castspd       += 0.01 * tech.stats[4];
        baseStats.abilityCount  += 0.02 * tech.stats[5];
        baseStats.luck          += 0.1  * tech.stats[6];

        InitializeBattleStats();

        foreach (int id in tech.abilityIDs)
            abilityDict[id] = AbilityDatabase.GetAbility(id);

        foreach (int passiveId in tech.passiveIDs)
            passiveDict[passiveId] = PassiveDatabase.GetPassive(passiveId);
    }

    // -------------------------
    // Saving / Loading
    // -------------------------
    public RobotSaveData ToSaveData()
    {
        return new RobotSaveData
        {
            robotDataName = data.name,

            health        = baseStats.maxHealth,
            atkdamage     = baseStats.atkdamage,
            abilitydamage = baseStats.abilitydamage,
            abilityCount  = baseStats.abilityCount,
            atkspd        = baseStats.atkspd,
            castspd       = baseStats.castspd,
            luck          = baseStats.luck,

            abilityIDs    = new List<int>(abilityDict.Keys),
            passiveIDs    = new List<int>(passiveDict.Keys),
            technologyIDs = new List<int>(technologyIDs)
        };
    }

    public static RobotInstance FromSaveData(RobotSaveData save)
    {
        RobotData robotData = Resources.Load<RobotData>("Robots/" + save.robotDataName);
        RobotInstance instance = new RobotInstance(robotData);

        instance.baseStats.maxHealth     = save.health;
        instance.baseStats.health        = save.health;
        instance.baseStats.atkdamage     = save.atkdamage;
        instance.baseStats.abilitydamage = save.abilitydamage;
        instance.baseStats.abilityCount  = save.abilityCount;
        instance.baseStats.atkspd        = save.atkspd;
        instance.baseStats.castspd       = save.castspd;
        instance.baseStats.luck          = save.luck;

        instance.InitializeBattleStats();

        instance.abilityDict.Clear();
        foreach (int id in save.abilityIDs)
            instance.abilityDict[id] = AbilityDatabase.GetAbility(id);

        instance.passiveDict.Clear();
        foreach (int id in save.passiveIDs)
            instance.passiveDict[id] = PassiveDatabase.GetPassive(id);

        instance.technologyIDs = new List<int>(save.technologyIDs);

        return instance;
    }

    // -------------------------
    // Ability Casting
    // -------------------------
    public void CastNextAbilities()
    {
        int count = (int)Math.Floor(battleStats.abilityCount);
        if (count <= 0 || abilityDict.Count == 0)
            return;

        var keys = new List<int>(abilityDict.Keys);

        for (int i = 0; i < count; i++)
        {
            int index = (nextAbilityIndex + i) % keys.Count;
            int abilityId = keys[index];
            AbilityData ability = abilityDict[abilityId];

            TriggerBeforeAbility(ability);
            ability.Execute(this);
            TriggerAfterAbility(ability);
        }

        nextAbilityIndex = (nextAbilityIndex + count) % keys.Count;
    }

    // -------------------------
    // Buff System
    // -------------------------
    public void AddBuff(Buff buff)
    {
        foreach (var b in activeBuffs)
        {
            if (b.GetType() == buff.GetType())
            {
                b.stacks += buff.stacks;
                b.OnStack(this, buff.stacks);
                return;
            }
        }

        activeBuffs.Add(buff);
        buff.OnFirstApply(this);
    }

    public void TriggerBeforeAbility(AbilityData ability)
    {
        foreach (var buff in activeBuffs)
            buff.OnBeforeAbility(this, ability);
    }

    public void TriggerAfterAbility(AbilityData ability)
    {
        foreach (var buff in activeBuffs)
            buff.OnAfterAbility(this, ability);

        activeBuffs.RemoveAll(b => b.ShouldRemove());
    }

    public void UpdateBuffs(float deltaTime)
    {
        foreach (var buff in activeBuffs)
            buff.Update(this, deltaTime);

        activeBuffs.RemoveAll(b => b.ShouldRemove());
    }

    public void TriggerOnBasicAttack(EnemyCombat target)
    {
        foreach (var buff in activeBuffs)
            buff.OnBasicAttack(this, target);
    }

    // -------------------------
    // IBuffTarget Implementation
    // -------------------------
    public CombatStats GetStats() => battleStats;

    public void TakeDamage(double amount)
    {
        battleStats.health -= amount;
        combat?.UpdateHPBar();

        if (battleStats.health <= 0)
            combat?.Die();
    }

    public double GetCurrentHealth() => battleStats.health;

    public void SetCurrentHealth(double value)
    {
        battleStats.health = value;
        combat?.UpdateHPBar();
    }

    public void Die()
    {
        // handled by RobotCombat
    }

    // -------------------------
    // Battle Cleanup
    // -------------------------
    public void ResetBuffs()
    {
        activeBuffs.Clear();
        tempDamageMultiplier = 0f;
        InitializeBattleStats();
    }
}