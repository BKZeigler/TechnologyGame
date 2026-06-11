using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Shank")]
public class ShankAbility : AbilityData
{
    public override void Execute(RobotInstance caster)
    {
        var context = caster.combat.Context;

        if (context.enemies.Count == 0)
        {
            Debug.Log($"{caster.data.robotName} tried to cast {abilityName}, but no enemies exist.");
            return;
        }

        EnemyCombat target = context.enemies[Random.Range(0, context.enemies.Count)];

        // Apply bleed
        bool found = false;
        foreach (var debuff in target.activeDebuffs)
        {
            if (debuff is BleedBuff bleed)
            {
                bleed.stacks += 1;
                bleed.OnStack(target, 1);
                found = true;
                break;
            }
        }

        if (!found)
        {
            target.activeDebuffs.Add(new BleedBuff());
            Debug.Log($"{caster.data.robotName} applies Bleed to {target.name}!");
        }
    }
}