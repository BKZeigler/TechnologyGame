using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/E.M.P.")]
public class EMPAbility : AbilityData
{
    public override void Execute(RobotInstance caster)
    {
        var context = caster.combat.Context;

        // Calculate damage
        double damage = 3 * caster.battleStats.abilitydamage * (1 + caster.tempDamageMultiplier);
        caster.tempDamageMultiplier = 0f;

        // Deal AOE damage to all enemies
        foreach (var enemy in context.enemies)
        {
            enemy.TakeDamage(damage);
            Debug.Log($"E.M.P hits {enemy.name} for {damage} damage!");
        }

        // Apply self-debuff (attack speed -10%)
        caster.AddBuff(new AtkSpdDebuff());

        Debug.Log($"{caster.data.name} is slowed by E.M.P. (-10% attack speed)");
    }
}