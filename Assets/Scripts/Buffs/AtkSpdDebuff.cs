using UnityEngine;

public class AtkSpdDebuff : Buff
{
    public AtkSpdDebuff()
    {
        buffName = "Attack Speed Debuff";
        buffType = BuffType.Debuff;
        stacks = 1;
        duration = -1f; // lasts entire battle unless removed
    }

    public override void OnFirstApply(IBuffTarget target)
    {
        var stats = target.GetStats();
        stats.atkspd *= 0.90; // reduce attack speed by 10%
        Debug.Log($"{buffName}: Attack speed reduced by 10%.");
    }

    public override void OnStack(IBuffTarget target, int addedStacks)
    {
        var stats = target.GetStats();
        for (int i = 0; i < addedStacks; i++)
            stats.atkspd *= 0.90; // multiplicative stacking

        Debug.Log($"{buffName}: Additional stack(s) applied. Attack speed further reduced.");
    }
}