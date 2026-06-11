using UnityEngine;

public class AbilityCountBuff : Buff
{
    public AbilityCountBuff()
    {
        buffName = "Ability Count";
        stacks = 1;
    }

    public override void OnFirstApply(IBuffTarget target)
    {
        // First stack: +1
        if (target is RobotInstance robot)
        {
            robot.battleStats.abilityCount += 1;
            Debug.Log($"{robot.data.name} gains 1 stack of Ability Count! Total ability count: {robot.battleStats.abilityCount}");
        }
    }

    public override void OnStack(IBuffTarget target, int addedStacks)
    {
        // Each additional stack: +1 per stack
        if (target is RobotInstance robot)
        {
            robot.battleStats.abilityCount += 1 + addedStacks;
            Debug.Log($"{robot.data.name} gains {addedStacks} more stack(s) of Ability Count! Total ability count: {robot.battleStats.abilityCount}");
        }
    }

}