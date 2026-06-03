using UnityEngine;

public class RobotCombat : UnitThinker
{
    private RobotInstance instance;
    private BattleContext context;

    public void Initialize(RobotInstance instance, BattleContext context)
    {
        this.instance = instance;
        this.context = context;

        thinkInterval = (float)(1.0 / instance.atkspd); // attack speed scaling
        thinkTimer = thinkInterval;
    }

    protected override void Think()
    {
        if (context.enemies.Count == 0)
            return;

        EnemyCombat enemy = context.enemies[0]; // later: targeting rules

        enemy.TakeDamage(instance.atkdamage);
        Debug.Log($"Robot dealt {instance.atkdamage} damage");
    }

    public void TakeDamage(double amount)
    {
        instance.health -= amount;

        if (instance.health <= 0)
        {
            Debug.Log("Robot died");
            Destroy(gameObject);
        }
    }
}
