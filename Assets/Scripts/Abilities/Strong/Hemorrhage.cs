using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Hemorrhage")]
public class HemorrhageAbility : AbilityData
{
    public override void Execute(RobotInstance caster)
    {
        var context = caster.combat.Context;

        Debug.Log($"{caster.data.name} casts Hemorrhage!");

        foreach (var enemy in context.enemies)
        {
            bool found = false;

            foreach (var debuff in enemy.activeDebuffs)
            {
                if (debuff is BleedBuff bleed)
                {
                    bleed.stacks *= 2;

                    Debug.Log($"Hemorrhage doubles Bleed on {enemy.name}");

                    found = true;
                    break;
                }
            }

            if (!found)
            {
                Debug.Log($"{enemy.name} has no Bleed — Hemorrhage has no effect.");
            }
        }

        // Disable this ability for the rest of the battle
        caster.disabledAbilities.Add(this.id);
        Debug.Log($"{abilityName} is now disabled for the rest of the battle.");
    }
}