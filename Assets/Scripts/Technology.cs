using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Technology", menuName = "Scriptable Objects/Technology")]
public class Technology : ScriptableObject
{
    public int id;
    public string techName;

    // (0, health), (1,dmg), (2, abilityDamage), (3, atkspd),
    // (4, castspd), (5, abilityCount), (6, luck)
    public double[] stats = new double[7];

    // IDs instead of direct references
    public List<int> abilityIDs = new List<int>();
    public List<int> passiveIDs = new List<int>();

    public void DisplayTechInfo()
    {
        Debug.Log($"Technology: {techName} (ID: {id})");

        string[] statNames =
        {
            "Health", "Damage", "Ability Damage",
            "Attack Speed", "Cast Speed", "Ability Count", "Luck"
        };

        Debug.Log("Technology Stats:");
        for (int i = 0; i < stats.Length; i++)
            Debug.Log($"{statNames[i]}: {stats[i]}");

        Debug.Log("Technology Abilities:");
        foreach (var abilityId in abilityIDs)
            Debug.Log($"Ability ID: {abilityId}");

        Debug.Log("Technology Passives:");
        foreach (var passiveId in passiveIDs)
            Debug.Log($"Passive ID: {passiveId}");
    }
}