using UnityEngine;

public class RobotCombat : UnitThinker
{
    private RobotInstance instance;
    public BattleContext Context { get; private set; }
    public HPBar hpBar;

    protected override void Update()
    {
        // Tick buffs first
        instance.UpdateCombatState(Time.deltaTime);

        // Recalculate attack interval dynamically
        thinkInterval = (float)(1.0 / instance.battleStats.atkspd);

        // Then run the normal Think() timer
        base.Update();
    }
    public void Initialize(RobotInstance instance, BattleContext context)
    {
        this.instance = instance;
        this.Context = context;

        thinkInterval = (float)(1.0 / instance.battleStats.atkspd); // attack speed scaling
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
        Context.enemies.RemoveAll(e => e == null); // removes destroyed enemies

        if (Context.enemies.Count == 0)
            return;

        EnemyCombat enemy = Context.enemies[0]; // later: targeting rules

        // Check passives before attacking
        foreach (var passive in instance.activePassives)
        {
            if (!passive.AllowBasicAttack(instance))
                return; // basic attack blocked
        }

        enemy.TakeDamage(instance.battleStats.atkdamage);
        instance.TriggerOnBasicAttack(enemy);
        Debug.Log($"{instance.data.name}Robot dealt {instance.battleStats.atkdamage} damage");
    }

    public void TakeDamage(double amount)
    {
        instance.TakeDamage(amount);
    }

    public void UpdateHPBar()
    {
        float normalized = (float)(instance.battleStats.health / instance.battleStats.maxHealth);
        hpBar.SetValue(normalized);
    }

    public void Die()
    {
        instance.Die();
    }
}
