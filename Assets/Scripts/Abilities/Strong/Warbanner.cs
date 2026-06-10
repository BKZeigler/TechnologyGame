using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Warbanner")]
public class WarbannerAbility : AbilityData
{
    public override void Execute(RobotInstance caster)
    {
        // Increase global cast speed by +5%
        PlayerManager.Instance.AddGlobalCastSpeed(0.05f);

        Debug.Log($"{caster.data.robotName} casts Warbanner! All robots gain +5% cast speed for this battle.");
    }
}