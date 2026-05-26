using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Technology", menuName = "Scriptable Objects/Technology")]
public class Technology : ScriptableObject
{
    public Dictionary<int, float> stats = new Dictionary<int, float>(); //(0, health), (1,dmg), (2, atkspd), (3, range), (4, castspd), (5, luck)
    public Dictionary<int, Ability> abilities = new Dictionary<int, Ability>();
    public Dictionary<int, Passive> passives = new Dictionary<int, Passive>();

    public void Initialize(Dictionary<int, float> statDictionary,
                       Dictionary<int, Ability> abilityDictionary,
                       Dictionary<int, Passive> passiveDictionary)
    {
        stats = statDictionary;
        abilities = abilityDictionary;
        passives = passiveDictionary;
    }

    public void DisplayTechInfo()
    {
        Debug.Log("Technology Stats:");
        foreach (var stat in stats)
        {
            string statName = stat.Key switch
            {
                0 => "Health",
                1 => "Damage",
                2 => "Attack Speed",
                3 => "Range",
                4 => "Cast Speed",
                5 => "Luck",
                _ => "Unknown Stat"
            };
            Debug.Log($"{statName}: {stat.Value}");
        }

        Debug.Log("Technology Abilities:");
        foreach (var ability in abilities)
        {
            Debug.Log($"Ability ID: {ability.Key}, Ability Name: {ability.Value.name}");
        }

        Debug.Log("Technology Passives:");
        foreach (var passive in passives)
        {
            Debug.Log($"Passive ID: {passive.Key}, Passive Name: {passive.Value.name}");
        }
    }
}
