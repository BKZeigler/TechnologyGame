using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{

    public double health = 0f;
    public double atkdamage = 0f;
    public double abilitydamage = 0f;
    public double abilityCount = 0f;
    public double atkspd = 0f;
    public double castspd = 0f;
    public double luck = 0f;
    public Dictionary<int, Ability> abilityDict = new Dictionary<int, Ability>();
    public Dictionary<int, Passive> passiveDict = new Dictionary<int, Passive>();
    public List<Technology> acquiredTech = new List<Technology>();
    
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MergeTech(Technology tech)
    {
        acquiredTech.Add(tech);

        health += tech.stats[0];
        atkdamage += 0.05 * tech.stats[1];
        abilitydamage += 0.05 * tech.stats[2];
        atkspd += 0.01 * tech.stats[3];
        castspd += 0.01 * tech.stats[4]; // no use for ability prompt system planned (better as a global prompt cooldown reduction)
        abilityCount += 0.02 *tech.stats[5];
        luck += 0.1 * tech.stats[6];

        foreach (var ability in tech.abilities)
        {
            abilityDict.Add(ability.Key, ability.Value);
        }

        foreach (var passive in tech.passives)
        {
            passiveDict.Add(passive.Key, passive.Value);
        }
    }

    public void BasicAttack() // Will perform on a cooldown based on atkspd, scaling with atkdamage, always active unless cced
    {
        
    }

    public void AbilityCheck() // If robot is prompted for an ability, will access next abilityCount abilites in the
    {                          // abilityDict, scaling with ability damage.
        
    }

    public void DisplayStats()
    {
        Debug.Log($"Health: {health}");
        Debug.Log($"Attack Damage: {atkdamage}");
        Debug.Log($"Ability Damage: {abilitydamage}");
        Debug.Log($"Attack Speed: {atkspd}");
        Debug.Log($"Cast Speed: {castspd}");
        Debug.Log($"Ability Count: {abilityCount}");
        Debug.Log($"Luck: {luck}");
        Debug.Log("Abilities:");
        foreach (var ability in abilityDict)        {
            Debug.Log($"Ability ID: {ability.Key}, Ability Name: {ability.Value.name}");
        }
        Debug.Log("Passives:");
        foreach (var passive in passiveDict)        {
            Debug.Log($"Passive ID: {passive.Key}, Passive Name: {passive.Value.name}");
        }
    }
}
