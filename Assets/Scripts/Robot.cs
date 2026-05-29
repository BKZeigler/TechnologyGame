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
        Debug.Log($"Health: {instance.health}");
        Debug.Log($"Attack Damage: {instance.atkdamage}");
        Debug.Log($"Ability Damage: {instance.abilitydamage}");
        Debug.Log($"Attack Speed: {instance.atkspd}");
        Debug.Log($"Cast Speed: {instance.castspd}");
        Debug.Log($"Ability Count: {instance.abilityCount}");
        Debug.Log($"Luck: {instance.luck}");

        Debug.Log("Abilities:");
        foreach (var ability in instance.abilityDict)
            Debug.Log($"Ability ID: {ability.Key}, Ability Name: {ability.Value.name}");

        Debug.Log("Passives:");
        foreach (var passive in instance.passiveDict)
            Debug.Log($"Passive ID: {passive.Key}, Passive Name: {passive.Value.name}");
    }
}