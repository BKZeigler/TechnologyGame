using UnityEngine;

public class LeechLockBuff : Buff
{

    public LeechLockBuff(float durationSeconds)
    {
        buffName = "Leech Lock";
        duration = durationSeconds;
    }

    public override void OnFirstApply(IBuffTarget target)
    {
        if (target is RobotInstance robot)
        {
            Debug.Log($"{robot.data.name} is empowered by Leech Lock for {duration} seconds!");
        }
    }

    public override void OnBasicAttack(IBuffTarget target, EnemyCombat enemy)
    {
        if (target is RobotInstance robot)
        {
            double healAmount = robot.battleStats.abilitydamage;

        // any additions to multipliers to healing would go here

            robot.battleStats.health += healAmount;
            Debug.Log($"{robot.data.name} heals for {healAmount} due to Leech Lock!");
        }
    }

    public override bool ShouldRemove()
    {
        return duration <= 0f;
    }
}
