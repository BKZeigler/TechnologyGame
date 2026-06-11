using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Enrage")]
public class Enrage : AbilityData
{
    public override void Execute(RobotInstance caster)
    {
        caster.AddBuff(new AbilityCountBuff());

        Debug.Log($"{caster.data.name} casts Enrage!");
    }
}
