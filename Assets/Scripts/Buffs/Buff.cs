using UnityEngine;

public abstract class Buff
{
    public string buffName;
    public int stacks = 1;

    public float duration = -1f; // -1 means infinite duration

    // Called when applied
    public virtual void Update(RobotInstance robot, float deltaTime)
    {
        if (duration > 0)
        {
        duration -= deltaTime;
        }
    }   

    public virtual void OnFirstApply(RobotInstance robot) {}

    public virtual void OnBasicAttack(RobotInstance robot, EnemyCombat target) {}

    public virtual void OnStack(RobotInstance robot, int addedStacks) {}

    // Called before an ability is executed
    public virtual void OnBeforeAbility(RobotInstance robot, AbilityData ability) {}

    // Called after an ability is executed
    public virtual void OnAfterAbility(RobotInstance robot, AbilityData ability) {}

    // Called each turn or each cast cycle (optional)
    public virtual void OnTick(RobotInstance robot) {}

    // Whether this buff should be removed
    public virtual bool ShouldRemove()
    {
        return duration == 0 || duration < 0;
    }
}