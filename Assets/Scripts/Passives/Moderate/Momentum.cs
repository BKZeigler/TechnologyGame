using UnityEngine;

[CreateAssetMenu(menuName = "Passives/Momentum")]
public class MomentumPassive : PassiveData
{
    private int basicAttackCounter = 0;
    private bool isTriggering = false;

    private void OnEnable()
    {
        passiveName = "Momentum";
    }

    public override void OnBattleStart(RobotInstance robot)
    {
        basicAttackCounter = 0;
        isTriggering = false;
    }

    public override void OnBasicAttack(RobotInstance robot, EnemyCombat target)
    {
        if (isTriggering)
            return;

        basicAttackCounter++;

        if (basicAttackCounter >= 10)
        {
            basicAttackCounter = 0;
            TriggerMomentum(robot);
        }
    }

    private void TriggerMomentum(RobotInstance robot)
    {
        isTriggering = true;

        Debug.Log($"{robot.data.name}'s Momentum triggers! Casting next ability.");

        robot.CastNextSingleAbility();

        isTriggering = false;
    }
}