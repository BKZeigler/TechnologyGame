using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Flurry Kick")]
public class FlurryKick : AbilityData
{
    public override void Execute(RobotInstance caster)
    {
        var context = caster.combat.Context;

        if (context.enemies.Count == 0)
        {
            Debug.Log($"{caster.data.name} tried to cast {abilityName}, but no enemies exist.");
            return;
        }

        // Calculate number of kicks
        int kickCount = Mathf.Max(1, 3 + Mathf.FloorToInt((float)(caster.luck * 0.5)));

        // Damage per kick
        double damagePerKick = (1 + (caster.abilitydamage * 0.2)) * (1 + caster.tempDamageMultiplier);
        caster.tempDamageMultiplier = 0f;

        EnemyCombat target = context.enemies[Random.Range(0, context.enemies.Count)];

        // Fire kicks
        for (int i = 0; i < kickCount; i++)
        {
            target.TakeDamage(damagePerKick);

            caster.AddBuff(new AtkSpdBuff());

            Debug.Log($"{caster.data.name} fires Flurry Kick #{i + 1} at {target.name} for {damagePerKick} damage!");
        }
    }
}
