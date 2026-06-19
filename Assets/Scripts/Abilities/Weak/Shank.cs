using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Shank")]
public class ShankAbility : AbilityData
{
    public override void Execute(RobotInstance caster)
    {
        Debug.Log($"SHANK EXECUTED by {caster.data.robotName} at time {Time.time}");
        var context = caster.combat.Context;

        if (context.enemies.Count == 0)
        {
            Debug.Log($"{caster.data.robotName} tried to cast {abilityName}, but no enemies exist.");
            return;
        }

        EnemyCombat target = context.enemies[Random.Range(0, context.enemies.Count)];

        // Apply bleed
        foreach (var debuff in target.activeDebuffs)
        {
            if (debuff is BleedBuff bleed)
            {
                bleed.OnStack(target, 1);
                return;
            }
        }

        Debug.Log("No existing bleed found, applying new one.");
        var newBleed = new BleedBuff();
        target.AddBuff(newBleed);
        Debug.Log($"{caster.data.robotName} applies Bleed to {target.name}!");
    }
}