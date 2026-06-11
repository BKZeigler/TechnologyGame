using UnityEngine;

public class PrepareBuff : Buff
{
    public PrepareBuff()
    {
        buffName = "Prepare";
        stacks = 1;
    }

    private bool consumed = false;

    public override void OnBeforeAbility(IBuffTarget target, AbilityData ability)
    {
        // Increase damage of next ability by 25%
        if (target is RobotInstance robot)
        {
            robot.tempDamageMultiplier += 0.25f * stacks;
            consumed = true;
        }
    }

    public override bool ShouldRemove()
    {
        return consumed;
    }
}
