using System.Collections.Generic;
using UnityEngine;

public class TechnologyCreator : MonoBehaviour
{
    private float initialValue = 0f;
    private float currentValue = 0f;
    private float statValue = 150f;
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
        CreateTechnology(150f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateTechnology(float iValue) //Example track ivalue = 100
    {
        initialValue = iValue; // initialValue = 100
        currentValue = iValue; // currentValue = 100

        // Roll that stat share. Random 0-100% subtract percentage of the current value and update stat value.
        float statPercent = RandomBasicPercent(); // example random roll for stat share
        statValue = currentValue * statPercent; // stat value share
        currentValue = currentValue - statValue; // remove the stat value from the current value

        //Find abilites and passives
        int maximum;

        if (currentValue < 10) // Weak abilites
        {
            maximum = (int)(currentValue % 10);
            int tempCount = Random.Range(0, maximum); // example random roll for weak abilities
            weakACount = tempCount;
            currentValue -= tempCount * 10;
        }
        if (currentValue < 10) // Weak passives
        {
            maximum = (int)(currentValue % 10);
            int tempCount = Random.Range(0, maximum); // example random roll for weak passives
            weakPCount = tempCount;
            currentValue -= tempCount * 10;
        }
        if (currentValue < 20) // Moderate abilites
        {
            maximum = (int)(currentValue % 20);
            int tempCount = Random.Range(0, maximum); // example random roll for moderate abilities
            modACount = tempCount;
            currentValue -= tempCount * 20;
        }
        if (currentValue < 20) // Moderate passives
        {
            maximum = (int)(currentValue % 20);
            int tempCount = Random.Range(0, maximum); // example random roll for moderate passives
            modPCount = tempCount;
            currentValue -= tempCount * 20;
        }
        if (currentValue < 30) // Strong abilites
        {
            maximum = (int)(currentValue % 30);
            int tempCount = Random.Range(0, maximum); // example random roll for strong abilities
            strongACount = tempCount;
            currentValue -= tempCount * 30;
        }
        if (currentValue < 30) // Strong passives
        {
            maximum = (int)(currentValue % 30);
            int tempCount = Random.Range(0, maximum); // example random roll for strong passives
            strongPCount = tempCount;
            currentValue -= tempCount * 30;
        }
        statValue += currentValue; // add any remaining value to the stat value

        var tech = ScriptableObject.CreateInstance<Technology>();

        tech.Initialize(CalcStats(), createAbilityDictionary(weakACount, modACount, strongACount), createPassiveDictionary(weakPCount, modPCount, strongPCount));
        tech.DisplayTechInfo();
    }

    Dictionary<int, float> CalcStats() //(0, health), (1,dmg), (2, abilityDamage), (3, atkspd), (4, castspd), (5, abilityCount), (6, luck)
    {
        Dictionary<int, float> stats = new Dictionary<int, float>();
        float tempPercent = RandomBasicPercent();

        stats.Add(0, statValue * tempPercent); // health
        statValue -= statValue * tempPercent; // remove the assigned stat value from the total stat value

        tempPercent = RandomBasicPercent();

        stats.Add(1, statValue * tempPercent); // damage
        statValue -= statValue * tempPercent;

        tempPercent = RandomBasicPercent();

        stats.Add(2, statValue * tempPercent); // ability damage
        statValue -= statValue * tempPercent;

        tempPercent = RandomBasicPercent();

        stats.Add(3, statValue * tempPercent); // atk speed
        statValue -= statValue * tempPercent;

        tempPercent = RandomBasicPercent();

        stats.Add(4, statValue * tempPercent); // cast speed
        statValue -= statValue * tempPercent;

        tempPercent = RandomBasicPercent();

        stats.Add(5, statValue * tempPercent); // ability count
        statValue -= statValue * tempPercent;

        tempPercent = RandomBasicPercent();

        stats.Add(6, statValue * tempPercent); // luck
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

    float RandomBasicPercent()
    {
    return Random.Range(0f, 100f) / 100f;
    }
}