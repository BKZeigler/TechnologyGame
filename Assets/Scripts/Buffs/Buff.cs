using UnityEngine;

public abstract class Buff
{
    public string buffName;
    public BuffType buffType = BuffType.Buff;
    public int stacks = 1;

    public float duration = -1f; // -1 means infinite duration

    // Called when applied
    public virtual void Update(IBuffTarget target, float deltaTime)
    {
        if (duration > 0)
        {
        duration -= deltaTime;
        }
    }   

    public virtual void OnFirstApply(IBuffTarget target) {}

    public virtual void OnBasicAttack(IBuffTarget target, EnemyCombat enemy) {}

    public virtual void OnStack(IBuffTarget target, int addedStacks) {}

    // Called before an ability is executed
    public virtual void OnBeforeAbility(IBuffTarget target, AbilityData ability) {}

    // Called after an ability is executed
    public virtual void OnAfterAbility(IBuffTarget target, AbilityData ability) {}

    // Called each turn or each cast cycle (optional)
    public virtual void OnTick(IBuffTarget target) {}

    // Whether this buff should be removed
    public virtual bool ShouldRemove()
    {
        return duration == 0 || duration < 0;
    }
}