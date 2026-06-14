using UnityEngine;

public abstract class PassiveData : ScriptableObject
{
    public int id;
    public string passiveName;
    public string description;
    // Return false to block the buff
    // -------------------------
    // Buff Interception
    // -------------------------
    // Return false to block the buff (e.g., Stainless Steel)
    public virtual bool AllowBuff(RobotInstance robot, Buff buff)
    {
        return true;
    }

    // -------------------------
    // Combat Event Hooks
    // -------------------------

    // Called when the robot performs a basic attack
    public virtual void OnBasicAttack(RobotInstance robot, EnemyCombat target) {}

    // Called when the robot takes damage
    public virtual void OnDamageTaken(RobotInstance robot, double amount) {}

    // Called when the robot casts an ability
    public virtual void OnAbilityCast(RobotInstance robot, AbilityData ability) {}

    // Called when a buff is successfully applied to the robot
    public virtual void OnBuffApplied(RobotInstance robot, Buff buff) {}

    // Called when a debuff is successfully applied to the robot
    public virtual void OnDebuffApplied(RobotInstance robot, Buff buff) {}

    // Called when the robot kills an enemy
    public virtual void OnEnemyKilled(RobotInstance robot, EnemyCombat enemy) {}

    // Called at the start of battle
    public virtual void OnBattleStart(RobotInstance robot) {}

    // Called at the end of battle
    public virtual void OnBattleEnd(RobotInstance robot) {}
}
