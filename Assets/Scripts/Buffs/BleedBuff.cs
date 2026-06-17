using UnityEngine;

public class BleedBuff : Buff
{
    private float tickTimer = 0f;

    public BleedBuff()
    {
        buffName = "Bleed";
        buffType = BuffType.Debuff;
        stacks = 1;
        duration = -1f; // infinite until battle ends
    }

    public override void Update(IBuffTarget target, float deltaTime)
    {
        Debug.Log($"Enemy received debuff: {buffName}, stacks={stacks}");
        base.Update(target, deltaTime);

        tickTimer += deltaTime;

        if (tickTimer >= 1f) // 1 second tick
        {
            tickTimer = 0f;

            double damage = 2 * stacks;
            target.TakeDamage(damage);

            Debug.Log($"{buffName} deals {damage} bleed damage!");
        }
    }

    public override void OnStack(IBuffTarget target, int addedStacks)
    {
        Debug.Log($"{buffName} stacked to {stacks}!");
    }
}