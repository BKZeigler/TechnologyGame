using UnityEngine;
using System.Collections.Generic;
using System;

[System.Serializable]
public class RobotInstance
{
    public RobotData data;
    public RobotCombat combat;

    // -------------------------
    // Persistent (saved) stats
    // -------------------------
    public double baseHealth;
    public double baseAtkDamage;
    public double baseAbilityDamage;
    public double baseAbilityCount;
    public double baseAtkSpd;
    public double baseCastSpd;
    public double baseLuck;

    // -------------------------
    // Battle-only stats
    // -------------------------
    public double health;
    public double maxHealth;
    public double atkdamage;
    public double abilitydamage;
    public double abilityCount;
    public double atkspd;
    public double castspd;
    public double luck;

    public float tempDamageMultiplier = 0f;
    private int nextAbilityIndex = 0;

    // Runtime abilities/passives/techs (IDs only)
    public Dictionary<int, AbilityData> abilityDict = new Dictionary<int, AbilityData>();
    public Dictionary<int, PassiveData> passiveDict = new Dictionary<int, PassiveData>();
    public List<int> technologyIDs = new List<int>();
    public List<Buff> activeBuffs = new List<Buff>();

    // -------------------------
    // Constructor
    // -------------------------
    public RobotInstance(RobotData data)
    {
        this.data = data;

        // Load persistent stats
        baseHealth        = data.baseHealth;
        baseAtkDamage     = data.baseAtkDamage;
        baseAbilityDamage = data.baseAbilityDamage;
        baseAbilityCount  = data.baseAbilityCount;
        baseAtkSpd        = data.baseAtkSpeed;
        baseCastSpd       = data.baseCastSpeed;
        baseLuck          = data.baseLuck;

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
        maxHealth     = baseHealth;
        health        = baseHealth;
        atkdamage     = baseAtkDamage;
        abilitydamage = baseAbilityDamage;
        abilityCount  = baseAbilityCount;
        atkspd        = baseAtkSpd;
        castspd       = baseCastSpd;
        luck          = baseLuck;

        tempDamageMultiplier = 0f;
        activeBuffs.Clear();
    }

    // -------------------------
    // Tech merging (persistent)
    // -------------------------
    public void MergeTech(Technology tech)
    {
        technologyIDs.Add(tech.id);

        baseHealth        += tech.stats[0];
        baseAtkDamage     += 0.05 * tech.stats[1];
        baseAbilityDamage += 0.05 * tech.stats[2];
        baseAtkSpd        += 0.01 * tech.stats[3];
        baseCastSpd       += 0.01 * tech.stats[4];
        baseAbilityCount  += 0.02 * tech.stats[5];
        baseLuck          += 0.1  * tech.stats[6];

        // Update battle stats immediately if in battle
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

            // Save persistent stats
            health = baseHealth,
            atkdamage = baseAtkDamage,
            abilitydamage = baseAbilityDamage,
            abilityCount = baseAbilityCount,
            atkspd = baseAtkSpd,
            castspd = baseCastSpd,
            luck = baseLuck,

            abilityIDs = new List<int>(abilityDict.Keys),
            passiveIDs = new List<int>(passiveDict.Keys),
            technologyIDs = new List<int>(technologyIDs)
        };
    }

    public static RobotInstance FromSaveData(RobotSaveData save)
    {
        RobotData robotData = Resources.Load<RobotData>("Robots/" + save.robotDataName);
        RobotInstance instance = new RobotInstance(robotData);

        // Load persistent stats
        instance.baseHealth        = save.health;
        instance.baseAtkDamage     = save.atkdamage;
        instance.baseAbilityDamage = save.abilitydamage;
        instance.baseAbilityCount  = save.abilityCount;
        instance.baseAtkSpd        = save.atkspd;
        instance.baseCastSpd       = save.castspd;
        instance.baseLuck          = save.luck;

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
        int count = (int)Math.Floor(abilityCount);
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
    // Battle Cleanup
    // -------------------------
    public void ResetBuffs()
    {
        activeBuffs.Clear();
        tempDamageMultiplier = 0f;
        InitializeBattleStats();
    }
}