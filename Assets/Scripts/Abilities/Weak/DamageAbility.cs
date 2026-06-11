using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Damage Ability")]
public class DamageAbility : AbilityData
{
    public override void Execute(RobotInstance caster)
    {
        // Calculate damage
        double damage = (baseDamage + caster.battleStats.abilitydamage) * (1 + caster.tempDamageMultiplier);
        caster.tempDamageMultiplier = 0f;

        // Get battle context
        var context = caster.combat.Context;

        // No enemies? Do nothing
        if (context.enemies.Count == 0)
        {
            Debug.Log($"{caster.data.name} tried to cast {abilityName}, but no enemies exist.");
            return;
        }

        // Pick a random enemy
        EnemyCombat target = context.enemies[Random.Range(0, context.enemies.Count)];

        // Apply damage
        target.TakeDamage(damage);

        Debug.Log($"{caster.data.name} casts {abilityName} on {target.name} for {damage} damage!");
    }
}