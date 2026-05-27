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
    //private int minimumStatPercentage = 0;
    ///private int maximumStatPercentage = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Technology CreateTechnology(double iValue) //Example track ivalue = 100
    {
        statValue = 0;

        initialValue = iValue; // initialValue = 100
        currentValue = iValue; // currentValue = 100

        // Roll that stat share. Random 0-100% subtract percentage of the current value and update stat value.
        double statPercent = RandomBasicPercent(); // example random roll for stat share
        statValue = currentValue * statPercent; // stat value share
        currentValue = currentValue - statValue; // remove the stat value from the current value

        //Find abilites and passives

        if (currentValue > 10) // Weak abilites
        {
            int maxCount = (int)Math.Floor(currentValue / 10.0);
            int tempCount = UnityEngine.Random.Range(0, maxCount); // example random roll for weak abilities
            weakACount = tempCount;
            currentValue -= tempCount * 10;
        }
        if (currentValue > 10) // Weak passives
        {
            int maxCount = (int)Math.Floor(currentValue / 10.0);
            int tempCount = UnityEngine.Random.Range(0, maxCount); // example random roll for weak passives
            weakPCount = tempCount;
            currentValue -= tempCount * 10;
        }
        if (currentValue > 20) // Moderate abilites
        {
            int maxCount = (int)Math.Floor(currentValue / 20.0);
            int tempCount = UnityEngine.Random.Range(0, maxCount); // example random roll for moderate abilities
            modACount = tempCount;
            currentValue -= tempCount * 20;
        }
        if (currentValue > 20) // Moderate passives
        {
            int maxCount = (int)Math.Floor(currentValue / 20.0);
            int tempCount = UnityEngine.Random.Range(0, maxCount); // example random roll for moderate passives
            modPCount = tempCount;
            currentValue -= tempCount * 20;
        }
        if (currentValue > 30) // Strong abilites
        {
            int maxCount = (int)Math.Floor(currentValue / 30.0);
            int tempCount = UnityEngine.Random.Range(0, maxCount); // example random roll for strong abilities
            strongACount = tempCount;
            currentValue -= tempCount * 30;
        }
        if (currentValue > 30) // Strong passives
        {
            int maxCount = (int)Math.Floor(currentValue / 30.0);
            int tempCount = UnityEngine.Random.Range(0, maxCount); // example random roll for strong passives
            strongPCount = tempCount;
            currentValue -= tempCount * 30;
        }
        statValue += currentValue; // add any remaining value to the stat value

        var tech = ScriptableObject.CreateInstance<Technology>();

        tech.Initialize(CalcStats(), createAbilityDictionary(weakACount, modACount, strongACount), createPassiveDictionary(weakPCount, modPCount, strongPCount));
        //tech.DisplayTechInfo(); only used for testing debugging

        return tech;
    }

    Dictionary<int, double> CalcStats() //(0, health), (1,dmg), (2, abilityDamage), (3, atkspd), (4, castspd), (5, abilityCount), (6, luck)
    {
        Dictionary<int, double> stats = new Dictionary<int, double>();
        double tempPercent = RandomBasicPercent();

        stats.Add(0, Math.Round(statValue * tempPercent, 1)); // health
        statValue -= statValue * tempPercent; // remove the assigned stat value from the total stat value

        tempPercent = RandomBasicPercent();

        stats.Add(1, Math.Round(statValue * tempPercent, 1)); // damage
        statValue -= statValue * tempPercent;

        tempPercent = RandomBasicPercent();

        stats.Add(2, Math.Round(statValue * tempPercent, 1)); // ability damage
        statValue -= statValue * tempPercent;

        tempPercent = RandomBasicPercent();

        stats.Add(3, Math.Round(statValue * tempPercent, 1)); // atk speed
        statValue -= statValue * tempPercent;

        tempPercent = RandomBasicPercent();

        stats.Add(4, Math.Round(statValue * tempPercent, 1)); // cast speed
        statValue -= statValue * tempPercent;

        tempPercent = RandomBasicPercent();

        stats.Add(5, Math.Round(statValue * tempPercent, 1)); // ability count
        statValue -= statValue * tempPercent;

        tempPercent = RandomBasicPercent();

        stats.Add(6, Math.Round(statValue * tempPercent, 1)); // luck
        statValue -= statValue * tempPercent;

        //print stats
        //foreach (var ele in stats){
        //    Debug.Log($"Key: {ele.Key}, Value: {ele.Value}");
        //}
        return stats;
    }

    Dictionary<int, Ability> createAbilityDictionary(int weakCount, int modCount, int strongCount)
    {
        Dictionary<int, Ability> abilities = new Dictionary<int, Ability>();

    //pull from pool of abilites based on the counts for weak/mod/strong

        return abilities;
    }

    Dictionary<int, Passive> createPassiveDictionary(int weakCount, int modCount, int strongCount)
    {
        Dictionary<int, Passive> passives = new Dictionary<int, Passive>();

    //pull from pool of passives based on the counts for weak/mod/strong

        return passives;
    }

    double RandomBasicPercent()
    {
    return UnityEngine.Random.Range(0f, 100f) / 100f;
    }
}