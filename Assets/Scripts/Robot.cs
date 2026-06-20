using UnityEngine;

public class Robot : MonoBehaviour
{
    public RobotInstance instance;

    public void Initialize(RobotInstance instance)
    {
        this.instance = instance;
    }

    public void BasicAttack()
    {
        // Use instance.atkdamage, instance.atkspd, etc.
    }

    public void AbilityCheck()
    {
        // Use instance.abilityDict, instance.abilitydamage, etc.
    }

    public void DisplayStats()
    {
        Debug.Log(instance == null ? "Instance is NULL" : "Instance OK");
        Debug.Log(instance.baseStats == null ? "BaseStats is NULL" : "BaseStats OK");
        //Debug.Log(instance.abilityDict == null ? "AbilityDict is NULL" : "AbilityDict OK");

        Debug.Log($"Health: {instance.baseStats.health}");
        Debug.Log($"Attack Damage: {instance.baseStats.atkdamage}");
        Debug.Log($"Ability Damage: {instance.baseStats.abilitydamage}");
        Debug.Log($"Attack Speed: {instance.baseStats.atkspd}");
        Debug.Log($"Cast Speed: {instance.baseStats.castspd}");
        Debug.Log($"Ability Count: {instance.baseStats.abilityCount}");
        Debug.Log($"Luck: {instance.baseStats.luck}");

        Debug.Log("Abilities:");
        //foreach (var ability in instance.abilityDict)
        //    Debug.Log($"Ability ID: {ability.Key}, Ability Name: {ability.Value.name}");

        Debug.Log("Passives:");
        foreach (var passive in instance.passiveDict)
            Debug.Log($"Passive ID: {passive.Key}, Passive Name: {passive.Value.name}");
    }
}