using System;
using System.Collections.Generic;
using UnityEngine;

public class TechnologyCreator : MonoBehaviour
{
    private double initialValue = 0;
    private double currentValue = 0;
    private double statValue = 0;

    private int weakACount = 0;
    private int weakPCount = 0;
    private int modACount = 0;
    private int modPCount = 0;
    private int strongACount = 0;
    private int strongPCount = 0;

    // Redistribution flags
    private bool redistributeHealth = false;
    private bool redistributeLuck = false;
    private bool redistributeAtkDmg = false;
    private bool redistributeAtkSpd = false;
    private bool redistributeAbilityDmg = false;
    private bool redistributeAbilityCount = false;

    private bool forceLeftoverIntoStats = false;
    private double minGreasePercent = 0;

    public Technology CreateTechnology(double scrapValue, Dictionary<ResourceData, int> modifiers)
    {
        ResetInternalState();

        // 1 Scrap = 10 value
        initialValue = scrapValue * 10;
        currentValue = initialValue;

        ApplyModifiers(modifiers);

        double statPercent = RandomBasicPercent(minGreasePercent);
        statValue = currentValue * statPercent;
        currentValue -= statValue;

        AllocateAbilityAndPassiveCounts();

        if (forceLeftoverIntoStats)
            statValue += currentValue;

        var tech = ScriptableObject.CreateInstance<Technology>();
        tech.techName = RunManager.Instance.GetRandomUnusedName();

        tech.stats = CalcStatsArray();
        ApplyStatRedistribution(tech.stats);
        ClampStats(tech.stats, initialValue);

        tech.abilityIDs = CreateAbilityIdList(weakACount, modACount, strongACount);
        tech.passiveIDs = CreatePassiveIdList(weakPCount, modPCount, strongPCount);

        return tech;
    }

    private void ApplyModifiers(Dictionary<ResourceData, int> modifiers)
    {
        foreach (var kvp in modifiers)
        {
            ResourceData res = kvp.Key;
            int amount = kvp.Value;

            switch (res.resourceName)
            {
                case "Iron": weakACount += amount; break;
                case "Copper": weakPCount += amount; break;

                case "Steel": modACount += amount; break;
                case "Bronze": modPCount += amount; break;

                case "Damascus Steel": strongACount += amount; break;
                case "Corinthian Bronze": strongPCount += amount; break;

                case "Tungsten": forceLeftoverIntoStats = true; break;

                case "Grease":
                    minGreasePercent += amount * 0.05;
                    minGreasePercent = Math.Min(minGreasePercent, 0.95);
                    break;

                case "Glass":   redistributeHealth = true; break;
                case "Pyrite":  redistributeLuck = true; break;
                case "Gold":    redistributeAtkDmg = true; break;
                case "Lead":    redistributeAtkSpd = true; break;
                case "Coal":    redistributeAbilityDmg = true; break;
                case "Quartz":  redistributeAbilityCount = true; break;
            }
        }
    }

    private void AllocateAbilityAndPassiveCounts()
    {
        TryAllocate(ref weakACount, 10);
        TryAllocate(ref weakPCount, 10);
        TryAllocate(ref modACount, 20);
        TryAllocate(ref modPCount, 20);
        TryAllocate(ref strongACount, 30);
        TryAllocate(ref strongPCount, 30);
    }

    private void TryAllocate(ref int count, int cost)
    {
        if (currentValue > cost)
        {
            int maxCount = (int)Math.Floor(currentValue / cost);
            int add = UnityEngine.Random.Range(0, maxCount);
            count += add;
            currentValue -= add * cost;
        }
    }

    double[] CalcStatsArray()
    {
        double[] statsArr = new double[7];
        double remaining = statValue;

        for (int i = 0; i < 7; i++)
        {
            double percent = RandomBasicPercent();
            double val = remaining * percent;
            statsArr[i] = Math.Round(val, 1);
            remaining -= val;
        }

        return statsArr;
    }

    private void ApplyStatRedistribution(double[] stats)
    {
        // MergeTech multipliers (robot-power per stat point)
        double[] mult = new double[]
        {
            1.0,   // Health
            0.05,  // Damage
            0.05,  // AbilityDamage
            0.01,  // AttackSpeed
            0.01,  // CastSpeed
            0.02,  // AbilityCount
            0.1    // Luck
        };

        bool[] flags = new bool[]
        {
            redistributeHealth,
            redistributeAtkDmg,
            redistributeAbilityDmg,
            redistributeAtkSpd,
            false, // CastSpeed unaffected
            redistributeAbilityCount,
            redistributeLuck
        };

        // 1. Convert raw stats → robot-power
        double[] power = new double[7];
        for (int i = 0; i < 7; i++)
            power[i] = stats[i] * mult[i];

        // 2. Sum flagged robot-power
        double pool = 0;
        for (int i = 0; i < 7; i++)
            if (flags[i])
                pool += power[i];

        // 3. Zero flagged stats
        for (int i = 0; i < 7; i++)
            if (flags[i])
                stats[i] = 0;

        if (pool <= 0)
            return;

        // 4. Sum multipliers of unflagged stats
        double totalMult = 0;
        for (int i = 0; i < 7; i++)
            if (!flags[i])
                totalMult += mult[i];

        // 5. Redistribute robot-power proportionally
        for (int i = 0; i < 7; i++)
        {
            if (!flags[i])
            {
                double addPower = pool * (mult[i] / totalMult);
                stats[i] += Math.Round(addPower / mult[i], 1); // convert back to raw stat
            }
        }

        // 6. Clamp raw stats to avoid absurd values
        double maxStat = initialValue; // scrapValue * 10
        for (int i = 0; i < 7; i++)
        {
            if (stats[i] > maxStat)
                stats[i] = Math.Round(maxStat, 1);
            if (stats[i] < 0)
                stats[i] = 0;
        }
    }
    private void ClampStats(double[] stats, double totalValue)
    {
        // Simple safety clamp: no stat should exceed totalValue
        for (int i = 0; i < stats.Length; i++)
        {
            if (stats[i] > totalValue)
                stats[i] = Math.Round(totalValue, 1);
            if (stats[i] < 0)
                stats[i] = 0;
        }
    }

    List<int> CreateAbilityIdList(int weakCount, int modCount, int strongCount)
    {
        var ids = new List<int>();
        var adb = AbilityDatabase.Instance;

        for (int i = 0; i < weakCount; i++)
            ids.Add(GetRandomAbility(adb.weakAbilities).id);

        for (int i = 0; i < modCount; i++)
            ids.Add(GetRandomAbility(adb.moderateAbilities).id);

        for (int i = 0; i < strongCount; i++)
            ids.Add(GetRandomAbility(adb.strongAbilities).id);

        return ids;
    }

    AbilityData GetRandomAbility(List<AbilityData> list)
    {
        if (list == null || list.Count == 0)
            return null;

        return list[UnityEngine.Random.Range(0, list.Count)];
    }

    List<int> CreatePassiveIdList(int weakCount, int modCount, int strongCount)
    {
        var ids = new List<int>();
        var pdb = PassiveDatabase.Instance;

        for (int i = 0; i < weakCount; i++)
            ids.Add(GetRandomPassive(pdb.weakPassives).id);

        for (int i = 0; i < modCount; i++)
            ids.Add(GetRandomPassive(pdb.moderatePassives).id);

        for (int i = 0; i < strongCount; i++)
            ids.Add(GetRandomPassive(pdb.strongPassives).id);

        return ids;
    }

    PassiveData GetRandomPassive(List<PassiveData> list)
    {
        if (list == null || list.Count == 0)
            return null;

        return list[UnityEngine.Random.Range(0, list.Count)];
    }

    double RandomBasicPercent(double min = 0)
    {
        double p = UnityEngine.Random.Range(0f, 100f) / 100f;
        return Math.Max(p, min);
    }

    private void ResetInternalState()
    {
        initialValue = currentValue = statValue = 0;

        weakACount = weakPCount = modACount = modPCount = strongACount = strongPCount = 0;

        redistributeHealth = redistributeLuck = redistributeAtkDmg = redistributeAtkSpd =
        redistributeAbilityDmg = redistributeAbilityCount = false;

        forceLeftoverIntoStats = false;
        minGreasePercent = 0;
    }
}