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

    public Technology CreateTechnology(double iValue)
    {
        statValue = 0;

        initialValue = iValue;
        currentValue = iValue;

        double statPercent = RandomBasicPercent();
        statValue = currentValue * statPercent;
        currentValue -= statValue;

        if (currentValue > 10)
        {
            int maxCount = (int)Math.Floor(currentValue / 10.0);
            weakACount = UnityEngine.Random.Range(0, maxCount);
            currentValue -= weakACount * 10;
        }
        if (currentValue > 10)
        {
            int maxCount = (int)Math.Floor(currentValue / 10.0);
            weakPCount = UnityEngine.Random.Range(0, maxCount);
            currentValue -= weakPCount * 10;
        }
        if (currentValue > 20)
        {
            int maxCount = (int)Math.Floor(currentValue / 20.0);
            modACount = UnityEngine.Random.Range(0, maxCount);
            currentValue -= modACount * 20;
        }
        if (currentValue > 20)
        {
            int maxCount = (int)Math.Floor(currentValue / 20.0);
            modPCount = UnityEngine.Random.Range(0, maxCount);
            currentValue -= modPCount * 20;
        }
        if (currentValue > 30)
        {
            int maxCount = (int)Math.Floor(currentValue / 30.0);
            strongACount = UnityEngine.Random.Range(0, maxCount);
            currentValue -= strongACount * 30;
        }
        if (currentValue > 30)
        {
            int maxCount = (int)Math.Floor(currentValue / 30.0);
            strongPCount = UnityEngine.Random.Range(0, maxCount);
            currentValue -= strongPCount * 30;
        }

        //statValue += currentValue; maybe make the system less lenient by not giving the leftover value as stat boosts? (possible upgrade)

        // Create the Technology object
        var tech = ScriptableObject.CreateInstance<Technology>();

        // Assign a unique name from the run pool
        tech.techName = RunManager.Instance.GetRandomUnusedName();

        // Assign stats and IDs
        tech.stats = CalcStatsArray();
        tech.abilityIDs = CreateAbilityIdList(weakACount, modACount, strongACount);
        Debug.Log($"Created Technology {tech.techName} with ability IDs: {string.Join(",", tech.abilityIDs)}");
        tech.passiveIDs = CreatePassiveIdList(weakPCount, modPCount, strongPCount);

        return tech;
    }

    double[] CalcStatsArray()
    {
        double[] statsArr = new double[7];
        double tempPercent;

        tempPercent = RandomBasicPercent();
        statsArr[0] = Math.Round(statValue * tempPercent, 1);
        statValue -= statValue * tempPercent;

        tempPercent = RandomBasicPercent();
        statsArr[1] = Math.Round(statValue * tempPercent, 1);
        statValue -= statValue * tempPercent;

        tempPercent = RandomBasicPercent();
        statsArr[2] = Math.Round(statValue * tempPercent, 1);
        statValue -= statValue * tempPercent;

        tempPercent = RandomBasicPercent();
        statsArr[3] = Math.Round(statValue * tempPercent, 1);
        statValue -= statValue * tempPercent;

        tempPercent = RandomBasicPercent();
        statsArr[4] = Math.Round(statValue * tempPercent, 1);
        statValue -= statValue * tempPercent;

        tempPercent = RandomBasicPercent();
        statsArr[5] = Math.Round(statValue * tempPercent, 1);
        statValue -= statValue * tempPercent;

        tempPercent = RandomBasicPercent();
        statsArr[6] = Math.Round(statValue * tempPercent, 1);
        statValue -= statValue * tempPercent;

        return statsArr;
    }

    List<int> CreateAbilityIdList(int weakCount, int modCount, int strongCount)
    {
        var ids = new List<int>();

        // For now, just give the BasicDamage ability if weakCount > 0
        for (int i = 0; i < weakCount; i++)
            ids.Add(1); // ID of BasicDamage

        // TODO: pull from your tiered ability pools
        // Example:
        // ids.Add(AbilityPoolWeak.GetRandom().id);

        return ids;
    }

    List<int> CreatePassiveIdList(int weakCount, int modCount, int strongCount)
    {
        var ids = new List<int>();

        // TODO: pull from your tiered passive pools and then push their IDs

        return ids;
    }

    double RandomBasicPercent()
    {
        return UnityEngine.Random.Range(0f, 100f) / 100f;
    }
}