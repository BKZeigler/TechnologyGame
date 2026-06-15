using UnityEngine;

[CreateAssetMenu(menuName = "Passives/Reversal")]
public class ReversalPassive : PassiveData
{
    private double accumulatedLoss = 0;
    private double threshold = 0;

    private void OnEnable()
    {
        passiveName = "Reversal";
    }

    public override void OnBattleStart(RobotInstance robot)
    {
        accumulatedLoss = 0;
        threshold = robot.battleStats.maxHealth * 0.25;
    }

    public override void OnDamageTaken(RobotInstance robot, double amount)
    {
        accumulatedLoss += amount;

        TryTrigger(robot);
    }

    public override void OnHeal(RobotInstance robot, double amount)
    {
        accumulatedLoss -= amount;

        if (accumulatedLoss < 0)
            accumulatedLoss = 0;
    }

    private void TryTrigger(RobotInstance robot)
    {
        while (accumulatedLoss >= threshold)
        {
            accumulatedLoss -= threshold;

            double damage = robot.battleStats.maxHealth * 0.20;

            var context = robot.combat.Context;

            if (context.enemies.Count > 0)
            {
                EnemyCombat enemy = context.enemies[Random.Range(0, context.enemies.Count)];
                enemy.TakeDamage(damage);

                Debug.Log($"{robot.data.name}'s Reversal triggers! Deals {damage} damage to {enemy.name}");
            }
        }
    }
}