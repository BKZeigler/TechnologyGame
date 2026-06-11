using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Study")]
public class Study : AbilityData
{
    public override void Execute(RobotInstance caster)
    {
        caster.AddBuff(new AbilityPowerBuff());

        Debug.Log($"{caster.data.name} casts Study!");
    }
}