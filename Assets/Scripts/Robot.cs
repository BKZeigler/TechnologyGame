using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{

    public float health = 0f;
    public float atkdamage = 0f;
    public float abilitydamage = 0f;
    public float abilityCount = 0f;
    public float atkspd = 0f;
    public float castspd = 0f;
    public float luck = 0f;
    public Dictionary<int, Ability> abilityDict = new Dictionary<int, Ability>();
    public Dictionary<int, Passive> passiveDict = new Dictionary<int, Passive>();
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void mergeTech(Technology tech)
    {
        health += tech.stats[0];
        atkdamage += 0.05f * tech.stats[1];
        abilitydamage += 0.05f * tech.stats[2];
        atkspd += 0.01f * tech.stats[3];
        castspd += 0.01f * tech.stats[4]; // no use for ability prompt system planned (better as a global prompt cooldown reduction)
        abilityCount += 0.02f *tech.stats[5];
        luck += 0.1f * tech.stats[6];

        foreach (var ability in tech.abilities)
        {
            abilityDict.Add(ability.Key, ability.Value);
        }

        foreach (var passive in tech.passives)
        {
            passiveDict.Add(passive.Key, passive.Value);
        }
    }

    public void basicAttack() // Will perform on a cooldown based on atkspd, scaling with atkdamage, always active unless cced
    {
        
    }

    public void abilityCheck() // If robot is prompted for an ability, will access next abilityCount abilites in the
    {                          // abilityDict, scaling with ability damage.
        
    }
}
