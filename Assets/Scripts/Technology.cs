using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Technology", menuName = "Scriptable Objects/Technology")]
public class Technology : ScriptableObject
{
    public Dictionary<int, double> stats = new Dictionary<int, double>(); //(0, health), (1,dmg), (2, abilityDamage), (3, atkspd), (4, castspd), (5, abilityCount), (6, luck)
    public Dictionary<int, Ability> abilities = new Dictionary<int, Ability>();
    public Dictionary<int, Passive> passives = new Dictionary<int, Passive>();

    public void Initialize(Dictionary<int, double> statDictionary,
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
                2 => "Ability Damage",
                3 => "Attack Speed",
                4 => "Cast Speed",
                5 => "Ability Count",
                6 => "Luck",
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
