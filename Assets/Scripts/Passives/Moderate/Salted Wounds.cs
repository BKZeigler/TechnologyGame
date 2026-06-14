using UnityEngine;

[CreateAssetMenu(menuName = "Passives/Salted Wounds")]
public class SaltedWoundsPassive : PassiveData
{
    private void OnEnable()
    {
        passiveName = "Salted Wounds";
    }

    public override void OnBasicAttack(RobotInstance robot, EnemyCombat target)
    {
        robot.basicAttackCounter++;

        if (robot.basicAttackCounter >= 10)
        {
            robot.basicAttackCounter = 0;

            double damage = 5 + robot.battleStats.abilitydamage;

            var context = robot.combat.Context;

            if (context.enemies.Count > 0)
            {
                EnemyCombat enemy = context.enemies[Random.Range(0, context.enemies.Count)];
                enemy.TakeDamage(damage);

                Debug.Log($"{robot.data.name}'s Salted Wounds triggers! Deals {damage} bonus damage to {enemy.name}");
            }
        }
    }
}