using UnityEngine;

public class AtkSpdBuff : Buff
{
    public AtkSpdBuff()
    {
        buffName = "Attack Speed";
        stacks = 1;
    }

    public override void OnFirstApply(RobotInstance robot)
    {
        // First stack: +10%
        robot.atkspd += 0.1f;
        Debug.Log($"{robot.data.name} gains 1 stack of Attack Speed! Total atk spd: {robot.atkspd}");
    }

    public override void OnStack(RobotInstance robot, int addedStacks)
    {
        // Each additional stack: +10% per stack
        robot.atkspd += 0.1f * addedStacks;
        Debug.Log($"{robot.data.name} gains {addedStacks} more stack(s) of Attack Speed! Total atk spd: {robot.atkspd}");
    }

}
