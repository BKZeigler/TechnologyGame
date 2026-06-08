using UnityEngine;

public abstract class Buff
{
    public string buffName;
    public int stacks = 1;

    // Called when applied
    public virtual void OnApply(RobotInstance robot) {}

    // Called before an ability is executed
    public virtual void OnBeforeAbility(RobotInstance robot, AbilityData ability) {}

    // Called after an ability is executed
    public virtual void OnAfterAbility(RobotInstance robot, AbilityData ability) {}

    // Called each turn or each cast cycle (optional)
    public virtual void OnTick(RobotInstance robot) {}

    // Whether this buff should be removed
    public virtual bool ShouldRemove() => false;
}