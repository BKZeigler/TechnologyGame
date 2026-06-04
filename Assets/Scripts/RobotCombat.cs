using UnityEngine;

public class RobotCombat : UnitThinker
{
    private RobotInstance instance;
    public BattleContext Context { get; private set; }
    public HPBar hpBar;

    public void Initialize(RobotInstance instance, BattleContext context)
    {
        this.instance = instance;
        this.Context = context;

        thinkInterval = (float)(1.0 / instance.atkspd); // attack speed scaling
        thinkTimer = thinkInterval;

        // Spawn HP bar
        GameObject barObj = GameObject.Instantiate(Resources.Load<GameObject>("HPBar"), transform);
        hpBar = barObj.GetComponent<HPBar>();

        // Position above robot
        barObj.transform.localPosition = new Vector3(0, 1.2f, 0);

        UpdateHPBar();
    }

    protected override void Think()
    {
        if (Context.enemies.Count == 0)
            return;

        EnemyCombat enemy = Context.enemies[0]; // later: targeting rules

        enemy.TakeDamage(instance.atkdamage);
        Debug.Log($"Robot dealt {instance.atkdamage} damage");
    }

    public void TakeDamage(double amount)
    {
        instance.health -= amount;
        UpdateHPBar();

        if (instance.health <= 0)
        {
            Debug.Log("Robot died");
            Destroy(gameObject);
        }
    }

    private void UpdateHPBar()
    {
        float normalized = (float)(instance.health / instance.maxHealth);
        hpBar.SetValue(normalized);
    }
}
