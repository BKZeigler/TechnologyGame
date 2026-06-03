using UnityEngine;

public class EnemyCombat : UnitThinker
{
    public double health = 50;
    public double maxHealth = 50;
    public double attackDamage = 5;
    public double attackSpeed = 1;
    public HPBar hpBar;

    private BattleContext context;

    public void Initialize(BattleContext context)
    {
        this.context = context;

        thinkInterval = (float)(1.0 / attackSpeed);
        thinkTimer = thinkInterval;

        GameObject barObj = GameObject.Instantiate(Resources.Load<GameObject>("HPBar"), transform);
        hpBar = barObj.GetComponent<HPBar>();
        barObj.transform.localPosition = new Vector3(0, 1.2f, 0);

        health = maxHealth;

        UpdateHPBar();
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
        UpdateHPBar();

        if (health <= 0)
        {
            Debug.Log("Enemy died");
            Destroy(gameObject);
        }
    }

    private void UpdateHPBar()
    {
        float normalized = (float)(health / maxHealth);
        hpBar.SetValue(normalized);
    }
}
