using UnityEngine;
using System.Collections.Generic;
using System;

[System.Serializable]
public class RobotInstance
{
    public RobotData data;
    public RobotCombat combat;

    // Runtime stats
    public double health;
    public double maxHealth;
    public double atkdamage;
    public double abilitydamage;
    public double abilityCount;
    public double atkspd;
    public double castspd;
    public double luck;

    private int nextAbilityIndex = 0;

    // Runtime abilities/passives/techs (IDs only)
    public Dictionary<int, AbilityData> abilityDict = new Dictionary<int, AbilityData>();
    public Dictionary<int, PassiveData> passiveDict = new Dictionary<int, PassiveData>();
    public List<int> technologyIDs = new List<int>();

    public RobotInstance(RobotData data)
    {
        this.data = data;

        // Initialize from base stats
        maxHealth = data.baseHealth;
        health = maxHealth;
        atkdamage = data.baseAtkDamage;
        abilitydamage = data.baseAbilityDamage;
        abilityCount = data.baseAbilityCount;
        atkspd = data.baseAtkSpeed;
        castspd = data.baseCastSpeed;
        luck = data.baseLuck;

        // Initialize abilities/passives from base IDs
        foreach (int id in data.baseAbilityIDs)
            abilityDict[id] = AbilityDatabase.GetAbility(id);

        foreach (int id in data.basePassiveIDs)
            passiveDict[id] = PassiveDatabase.GetPassive(id);
    }

    public void MergeTech(Technology tech)
    {
        technologyIDs.Add(tech.id);

        maxHealth        += tech.stats[0];
        atkdamage     += 0.05 * tech.stats[1];
        abilitydamage += 0.05 * tech.stats[2];
        atkspd        += 0.01 * tech.stats[3];
        castspd       += 0.01 * tech.stats[4];
        abilityCount  += 0.02 * tech.stats[5];
        luck          += 0.1  * tech.stats[6];

        foreach (int id in tech.abilityIDs)
        {
            AbilityData ability = AbilityDatabase.GetAbility(id);
            if (ability != null)
            {
                abilityDict[id] = ability;

                Debug.Log("---- AbilityDict Contents After Merge ----");
                foreach (var kvp in abilityDict)
                {
                    Debug.Log($"Key = {kvp.Key}, Ability = {kvp.Value.abilityName}");
                }
                Debug.Log("------------------------------------------");
            }
        }

        foreach (int passiveId in tech.passiveIDs)
            passiveDict[passiveId] = PassiveDatabase.GetPassive(passiveId);
    }

    public RobotSaveData ToSaveData()
    {
        return new RobotSaveData
        {
            robotDataName = data.name,
            health = health,
            atkdamage = atkdamage,
            abilitydamage = abilitydamage,
            abilityCount = abilityCount,
            atkspd = atkspd,
            castspd = castspd,
            luck = luck,
            abilityIDs = new List<int>(abilityDict.Keys),
            passiveIDs = new List<int>(passiveDict.Keys),
            technologyIDs = new List<int>(technologyIDs)
        };
    }

    public static RobotInstance FromSaveData(RobotSaveData save)
    {
        RobotData robotData = Resources.Load<RobotData>("Robots/" + save.robotDataName);
        RobotInstance instance = new RobotInstance(robotData);

        instance.health = save.health;
        instance.atkdamage = save.atkdamage;
        instance.abilitydamage = save.abilitydamage;
        instance.abilityCount = save.abilityCount;
        instance.atkspd = save.atkspd;
        instance.castspd = save.castspd;
        instance.luck = save.luck;

        instance.abilityDict.Clear();
        foreach (int id in save.abilityIDs)
            instance.abilityDict[id] = AbilityDatabase.GetAbility(id);

        instance.passiveDict.Clear();
        foreach (int id in save.passiveIDs)
            instance.passiveDict[id] = PassiveDatabase.GetPassive(id);

        instance.technologyIDs = new List<int>(save.technologyIDs);

        return instance;
    }

    public void CastNextAbilities()
    {
        Debug.Log($"{data.name} is casting next abilities...");
        int count = (int)Math.Floor(abilityCount);
        if (count <= 0) return;

        if (abilityDict.Count == 0)
            return; // <-- prevents divide-by-zero

        var keys = new List<int>(abilityDict.Keys);

        for (int i = 0; i < count; i++)
        {
            int index = (nextAbilityIndex + i) % keys.Count;
            int abilityId = keys[index];
            AbilityData ability = abilityDict[abilityId];

            ability.Execute(this);
        }

        nextAbilityIndex = (nextAbilityIndex + count) % keys.Count;
    }
}
