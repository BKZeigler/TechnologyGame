using UnityEngine;
using System.Collections.Generic;
using System.Linq;
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
    public int basicAttackCounter = 0;

    // -------------------------
    // Abilities & Passives (DUPLICATES ALLOWED)
    // -------------------------
    public List<AbilityData> abilities = new List<AbilityData>();   // <--- NEW
    public Dictionary<int, PassiveData> passiveDict = new Dictionary<int, PassiveData>();
    public List<PassiveData> activePassives = new List<PassiveData>();

    // Techs
    public List<int> technologyIDs = new List<int>();

    // Buffs
    public List<Buff> activeBuffs = new List<Buff>();

    // Runtime per-ability state (battle only)
    public Dictionary<int, int> abilityStacks = new Dictionary<int, int>();
    public HashSet<int> disabledAbilities = new HashSet<int>();

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

        InitializeBattleStats();

        // Load base abilities (DUPLICATES ALLOWED)
        foreach (int id in data.baseAbilityIDs)
            abilities.Add(AbilityDatabase.GetAbility(id));

        // Load base passives
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
        disabledAbilities.Clear();
        basicAttackCounter = 0;

        // Activate passives
        activePassives.Clear();
        foreach (var p in passiveDict.Values)
            activePassives.Add(p);
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

        // Add abilities (DUPLICATES ALLOWED)
        foreach (int id in tech.abilityIDs)
            abilities.Add(AbilityDatabase.GetAbility(id));

        // Add passives
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

            // Save duplicates
            abilityIDs    = abilities.Select(a => a.id).ToList(),
            passiveIDs    = passiveDict.Keys.ToList(),
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

        // Load abilities (DUPLICATES ALLOWED)
        instance.abilities.Clear();
        foreach (int id in save.abilityIDs)
            instance.abilities.Add(AbilityDatabase.GetAbility(id));

        // Load passives
        instance.passiveDict.Clear();
        foreach (int id in save.passiveIDs)
            instance.passiveDict[id] = PassiveDatabase.GetPassive(id);

        instance.technologyIDs = new List<int>(save.technologyIDs);

        return instance;
    }

    // -------------------------
    // Ability Casting (DUPLICATE SAFE)
    // -------------------------
    public void CastNextAbilities()
    {
        int count = Mathf.FloorToInt((float)battleStats.abilityCount);
        if (count <= 0 || abilities.Count == 0)
            return;

        for (int i = 0; i < count; i++)
        {
            int index = (nextAbilityIndex + i) % abilities.Count;
            AbilityData ability = abilities[index];

            // Skip disabled abilities
            if (disabledAbilities.Contains(ability.id))
                continue;

            // Check passives
            bool allowed = true;
            foreach (var passive in activePassives)
            {
                if (!passive.AllowAbilityCast(this, ability))
                {
                    allowed = false;
                    break;
                }
            }

            if (!allowed)
                continue;

            TriggerBeforeAbility(ability);
            ability.Execute(this);
            TriggerAfterAbility(ability);

            foreach (var passive in activePassives)
                passive.OnAbilityCast(this, ability);
        }

        nextAbilityIndex = (nextAbilityIndex + count) % abilities.Count;
    }

    public void CastNextSingleAbility()
    {
        if (abilities.Count == 0)
            return;

        AbilityData ability = abilities[nextAbilityIndex];

        if (disabledAbilities.Contains(ability.id))
        {
            nextAbilityIndex = (nextAbilityIndex + 1) % abilities.Count;
            return;
        }

        foreach (var passive in activePassives)
        {
            if (!passive.AllowAbilityCast(this, ability))
            {
                nextAbilityIndex = (nextAbilityIndex + 1) % abilities.Count;
                return;
            }
        }

        TriggerBeforeAbility(ability);
        ability.Execute(this);
        TriggerAfterAbility(ability);

        foreach (var passive in activePassives)
            passive.OnAbilityCast(this, ability);

        nextAbilityIndex = (nextAbilityIndex + 1) % abilities.Count;
    }

    // -------------------------
    // Buff System
    // -------------------------
    public void AddBuff(Buff buff)
    {
        foreach (var passive in activePassives)
        {
            if (!passive.AllowBuff(this, buff))
                return;
        }

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

    public void UpdateCombatState(float deltaTime)
    {
        foreach (var buff in activeBuffs)
            buff.Update(this, deltaTime);

        activeBuffs.RemoveAll(b => b.ShouldRemove());

        foreach (var passive in activePassives)
            passive.OnUpdate(this, deltaTime);
    }

    public void TriggerOnBasicAttack(EnemyCombat target)
    {
        foreach (var buff in activeBuffs)
            buff.OnBasicAttack(this, target);

        foreach (var passive in activePassives)
            passive.OnBasicAttack(this, target);
    }

    // -------------------------
    // IBuffTarget Implementation
    // -------------------------
    public CombatStats GetStats() => battleStats;

    public void TakeDamage(double amount)
    {
        battleStats.health -= amount;
        combat?.UpdateHPBar();

        foreach (var passive in activePassives)
            passive.OnDamageTaken(this, amount);

        if (battleStats.health <= 0)
            combat?.Die();
    }

    public double GetCurrentHealth() => battleStats.health;

    public void SetCurrentHealth(double value)
    {
        double oldHP = battleStats.health;
        battleStats.health = value;
        combat?.UpdateHPBar();

        if (value > oldHP)
        {
            double healed = value - oldHP;
            foreach (var passive in activePassives)
                passive.OnHeal(this, healed);
        }
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

    public void TriggerOnBattleStart()
    {
        foreach (var passive in activePassives)
            passive.OnBattleStart(this);
    }
}