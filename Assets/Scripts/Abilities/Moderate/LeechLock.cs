using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Leech Lock")]
public class LeechLockAbility : AbilityData
{
    public override void Execute(RobotInstance caster)
    {
        caster.AddBuff(new LeechLockBuff(3f));
        Debug.Log($"{caster.data.name} casts Leech Lock! Their basic attacks heal for {caster.abilitydamage} for 3 seconds.");
    }
}
