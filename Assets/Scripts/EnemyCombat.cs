using UnityEngine;

public class EnemyCombat : UnitThinker
{
    public double health = 50;
    public double attackDamage = 5;
    public double attackSpeed = 1;

    private BattleContext context;

    public void Initialize(BattleContext context)
    {
        this.context = context;

        thinkInterval = (float)(1.0 / attackSpeed);
        thinkTimer = thinkInterval;
    }

    protected override void Think()
    {
        if (context.robots.Count == 0)
            return;

        RobotCombat target = context.robots[Random.Range(0, context.robots.Count)];
        target.TakeDamage(attackDamage);

        Debug.Log($"Enemy dealt {attackDamage} damage");
    }

    public void TakeDamage(double amount)
    {
        health -= amount;

        if (health <= 0)
        {
            Debug.Log("Enemy died");
            Destroy(gameObject);
        }
    }
}
