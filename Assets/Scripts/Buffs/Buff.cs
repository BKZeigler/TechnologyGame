public abstract class Buff
{
    public string buffName;
    public BuffType buffType = BuffType.Buff;
    public int stacks = 1;

    // -1 = infinite duration
    public float duration = -1f;

    public virtual void Update(IBuffTarget target, float deltaTime)
    {
        if (duration > 0)
        {
            duration -= deltaTime;

            // Clamp to zero so ShouldRemove() works cleanly
            if (duration < 0)
                duration = 0;
        }
    }

    public virtual bool ShouldRemove()
    {
        // Infinite duration
        if (duration < 0)
            return false;

        // Timed buff expired
        return duration == 0;
    }

    public virtual void OnFirstApply(IBuffTarget target) {}
    public virtual void OnBasicAttack(IBuffTarget target, EnemyCombat enemy) {}
    public virtual void OnStack(IBuffTarget target, int addedStacks) {}
    public virtual void OnBeforeAbility(IBuffTarget target, AbilityData ability) {}
    public virtual void OnAfterAbility(IBuffTarget target, AbilityData ability) {}
    public virtual void OnTick(IBuffTarget target) {}
}