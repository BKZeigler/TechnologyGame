using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Prepare")]
public class PrepareAbility : AbilityData
{
    public override void Execute(RobotInstance caster)
    {
        caster.AddBuff(new PrepareBuff());
        Debug.Log($"{caster.data.name} uses Prepare! Their next ability deals +25% damage.");
    }
}
