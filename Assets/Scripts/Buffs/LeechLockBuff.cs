using UnityEngine;

public class LeechLockBuff : Buff
{

    public LeechLockBuff(float durationSeconds)
    {
        buffName = "Leech Lock";
        buffType = BuffType.Buff;
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
            //double healAmount = robot.battleStats.abilitydamage;

        // any additions to multipliers to healing would go here

            // robot.battleStats.health += healAmount;
            double healAmount = robot.battleStats.abilitydamage;
            double newHP = Mathf.Min(
                (float)(robot.battleStats.health + healAmount), // add heal amount to current health as an option
                (float)robot.battleStats.maxHealth // or if the max hp is the smaller value, set it to that
            );

            robot.SetCurrentHealth(newHP);

            Debug.Log($"{robot.data.name} heals for {healAmount} due to Leech Lock!");
        }
    }

    public override bool ShouldRemove()
    {
        return duration <= 0f;
    }
}
