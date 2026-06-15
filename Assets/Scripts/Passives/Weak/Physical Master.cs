using UnityEngine;

[CreateAssetMenu(menuName = "Passives/Physical Master")]
public class PhysicalMasterPassive : PassiveData
{
    private void OnEnable()
    {
        passiveName = "Physical Master";
    }

    public override void OnBattleStart(RobotInstance robot)
    {
        // +25% attack damage
        robot.battleStats.atkdamage *= 1.25;

        // +25% attack speed
        robot.battleStats.atkspd *= 1.25;

        Debug.Log($"{robot.data.name} gains +25% Attack Damage and +25% Attack Speed from Physical Master!");
    }

    public override bool AllowAbilityCast(RobotInstance robot, AbilityData ability)
    {
        // Completely disable ability usage
        return false;
    }
}