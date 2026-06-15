using UnityEngine;

[CreateAssetMenu(menuName = "Passives/Finale")]
public class FinalePassive : PassiveData
{
    private int abilityCastCounter = 0;
    private bool isTriggeringFinale = false;

    private void OnEnable()
    {
        passiveName = "Finale";
    }

    public override void OnBattleStart(RobotInstance robot)
    {
        abilityCastCounter = 0;
        isTriggeringFinale = false;
    }

    public override void OnAbilityCast(RobotInstance robot, AbilityData ability)
    {
        if (isTriggeringFinale)
            return; // prevent recursion

        abilityCastCounter++;

        if (abilityCastCounter >= 10)
        {
            abilityCastCounter = 0;
            TriggerFinale(robot);
        }
    }

    private void TriggerFinale(RobotInstance robot)
    {
        var context = robot.combat.Context;

        Debug.Log($"{robot.data.name}'s Finale triggers! Casting ALL abilities!");

        isTriggeringFinale = true;

        foreach (var ability in robot.abilityDict.Values)
        {
            ability.Execute(robot);
        }

        isTriggeringFinale = false;
    }
}