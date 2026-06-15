using UnityEngine;

[CreateAssetMenu(menuName = "Passives/Pain Sponge")]
public class PainSpongePassive : PassiveData
{
    private void OnEnable()
    {
        passiveName = "Pain Sponge";
    }

    public override void OnHeal(RobotInstance robot, double amount)
    {
        if (amount <= 0)
            return;

        double damage = amount * 0.5;

        var context = robot.combat.Context;

        if (context.enemies.Count > 0)
        {
            EnemyCombat enemy = context.enemies[Random.Range(0, context.enemies.Count)];
            enemy.TakeDamage(damage);

            Debug.Log($"{robot.data.name}'s Pain Sponge triggers! Healed {amount}, dealing {damage} to {enemy.name}");
        }
    }
}