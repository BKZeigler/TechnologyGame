using UnityEngine;

public class AbilityPowerBuff : Buff
{
    public AbilityPowerBuff()
    {
        buffName = "Ability Power";
        stacks = 1;
    }

    public override void OnFirstApply(IBuffTarget target)
    {
        // First stack: +5%
        if (target is RobotInstance robot)
        {
            robot.battleStats.abilitydamage += 0.05f;
            Debug.Log($"{robot.data.name} gains 1 stack of Ability Power! Total ability damage: {robot.battleStats.abilitydamage}");
        }
    }

    public override void OnStack(IBuffTarget target, int addedStacks)
    {
        // Each additional stack: +5% per stack
        if (target is RobotInstance robot)
        {
            robot.battleStats.abilitydamage += 0.05f * addedStacks;
            Debug.Log($"{robot.data.name} gains {addedStacks} more stack(s) of Ability Power! Total ability damage: {robot.battleStats.abilitydamage}");
        }
    }

}