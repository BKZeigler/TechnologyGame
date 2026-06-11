using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Magic Missiles")]
public class MagicMissilesAbility : AbilityData
{
    public override void Execute(RobotInstance caster)
    {
        var context = caster.combat.Context;

        if (context.enemies.Count == 0)
        {
            Debug.Log($"{caster.data.name} tried to cast {abilityName}, but no enemies exist.");
            return;
        }

        // Calculate number of missiles
        int missileCount = Mathf.Max(1, Mathf.FloorToInt((float)(caster.battleStats.abilitydamage * 0.5)));

        // Damage per missile
        double damagePerMissile = (2 + (caster.battleStats.abilitydamage * 0.5)) * (1 + caster.tempDamageMultiplier);
        caster.tempDamageMultiplier = 0f;

        // Fire missiles
        for (int i = 0; i < missileCount; i++)
        {
            // Pick a random enemy each missile
            EnemyCombat target = context.enemies[Random.Range(0, context.enemies.Count)];

            target.TakeDamage(damagePerMissile);

            Debug.Log($"{caster.data.name} fires Magic Missile #{i + 1} at {target.name} for {damagePerMissile} damage!");
        }
    }
}