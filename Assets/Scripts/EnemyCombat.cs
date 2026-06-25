using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : UnitThinker, IBuffTarget
{
    public CombatStats stats = new CombatStats();
    public HPBar hpBar;

    public List<Buff> activeDebuffs = new List<Buff>();

    private BattleContext context;

    public void Initialize(BattleContext context)
    {
        this.context = context;

        // Enemy base stats should be set by enemy prefab or script
        stats.health = stats.maxHealth;

        thinkInterval = (float)(1.0 / stats.atkspd);
        thinkTimer = thinkInterval;

        GameObject barObj = GameObject.Instantiate(Resources.Load<GameObject>("HPBar"), transform);
        hpBar = barObj.GetComponent<HPBar>();
        barObj.transform.localPosition = new Vector3(0, 1.2f, 0);

        UpdateHPBar();
    }

    protected override void Update()
    {
        foreach (var debuff in activeDebuffs)
            debuff.Update(this, Time.deltaTime);

        activeDebuffs.RemoveAll(d => d.ShouldRemove());

        base.Update();
    }

    protected override void Think()
    {
        if (context.robots.Count == 0)
            return;

        RobotCombat target = context.robots[Random.Range(0, context.robots.Count)];
        target.TakeDamage(stats.atkdamage);

        //Debug.Log($"Enemy dealt {stats.atkdamage} damage");
        //Debug.Log("Current active debuffs: " + activeDebuffs.Count);
    }

    // -------------------------
    // IBuffTarget Implementation
    // -------------------------
    public CombatStats GetStats() => stats;

    public void TakeDamage(double amount)
    {
        stats.health -= amount;
        //UpdateHPBar();

        if (stats.health <= 0)
        {
            Die();
            return;
        }
        UpdateHPBar();
    }

    public double GetCurrentHealth() => stats.health;

    public void SetCurrentHealth(double value)
    {
        stats.health = value;
        UpdateHPBar();
    }

    public void Die()
    {
        Debug.Log("Enemy died");
        context.NotifyEnemyDied(this);
        Destroy(gameObject);
    }

    private void UpdateHPBar()
    {
        float normalized = (float)(stats.health / stats.maxHealth);
        hpBar.SetValue(normalized);
    }

    public void AddBuff(Buff buff)
    {
        // Normal stacking logic
        //foreach (var b in activeDebuffs)
        //{
            //if (b.GetType() == buff.GetType())
            //{
                //b.stacks += buff.stacks;
                //b.OnStack(this, buff.stacks);
                //return;
            //}
        //}

        activeDebuffs.Add(buff);
        buff.OnFirstApply(this);
    }
}