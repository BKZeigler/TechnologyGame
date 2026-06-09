using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Stab")]
public class Stab : AbilityData
{
    public override void Execute(RobotInstance caster)
    {
        var context = caster.combat.Context;

        if (context.enemies.Count == 0)
        {
            Debug.Log($"{caster.data.name} tried to cast {abilityName}, but no enemies exist.");
            return;
        }

        // Damage
        double damage = (5 + caster.abilitydamage) * (1 + caster.tempDamageMultiplier);
        caster.tempDamageMultiplier = 0f;

        // Fire stabs
        EnemyCombat target = context.enemies[Random.Range(0, context.enemies.Count)];

        target.TakeDamage(damage);


    }
}