using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Bruising Hits")]
public class BruisingHitsAbility : AbilityData
{
    public override void Execute(RobotInstance caster)
    {
        int abilityId = id;

        // Get current stack count (X)
        if (!caster.abilityStacks.TryGetValue(abilityId, out int stacks))
            stacks = 0;

        // Calculate number of hits
        int hits = 1 + stacks;

        // Calculate damage per hit
        double damage = (4 + caster.battleStats.abilitydamage) * (1 + caster.tempDamageMultiplier);
        caster.tempDamageMultiplier = 0f;

        var context = caster.combat.Context;

        if (context.enemies.Count == 0)
        {
            Debug.Log($"{caster.data.robotName} tried to cast {abilityName}, but no enemies exist.");
            return;
        }

        // Perform hits
        for (int i = 0; i < hits; i++)
        {
            EnemyCombat target = context.enemies[Random.Range(0, context.enemies.Count)];
            target.TakeDamage(damage);

            Debug.Log($"{caster.data.robotName} hits {target.name} for {damage} damage ({i+1}/{hits})");
        }

        // Increase X by 1
        caster.abilityStacks[abilityId] = stacks + 1;

        Debug.Log($"{abilityName} stack increased to {stacks + 1}");
    }
}