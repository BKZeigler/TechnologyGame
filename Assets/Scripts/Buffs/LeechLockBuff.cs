using UnityEngine;

public class LeechLockBuff : Buff
{

    public LeechLockBuff(float durationSeconds)
    {
        buffName = "Leech Lock";
        duration = durationSeconds;
    }

    public override void OnFirstApply(RobotInstance robot)
    {
        Debug.Log($"{robot.data.name} is empowered by Leech Lock for {duration} seconds!");
    }

    public override void OnBasicAttack(RobotInstance robot, EnemyCombat target)
    {
        double healAmount = robot.abilitydamage;

        // any additions to multipliers to healing would go here

        robot.health += healAmount;
        Debug.Log($"{robot.data.name} heals for {healAmount} due to Leech Lock!");
    }

    public override bool ShouldRemove()
    {
        return duration <= 0f;
    }
}
