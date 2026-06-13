using UnityEngine;

public class AtkSpdBuff : Buff
{
    public AtkSpdBuff()
    {
        buffName = "Attack Speed";
        buffType = BuffType.Buff;
        stacks = 1;
    }

    public override void OnFirstApply(IBuffTarget target)
    {
        // First stack: +10%
        if (target is RobotInstance robot)
        {
            robot.battleStats.atkspd += 0.1f;
            Debug.Log($"{robot.data.name} gains 1 stack of Attack Speed! Total atk spd: {robot.battleStats.atkspd}");
        }
    }

    public override void OnStack(IBuffTarget target, int addedStacks)
    {
        // Each additional stack: +10% per stack
        if (target is RobotInstance robot)
        {
            robot.battleStats.atkspd += 0.1f * addedStacks;
            Debug.Log($"{robot.data.name} gains {addedStacks} more stack(s) of Attack Speed! Total atk spd: {robot.battleStats.atkspd}");
        }
    }

}
