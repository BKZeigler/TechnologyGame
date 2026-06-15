using UnityEngine;

[CreateAssetMenu(menuName = "Passives/Ability Master")]
public class AbilityMasterPassive : PassiveData
{
    private void OnEnable()
    {
        passiveName = "Ability Master";
    }

    public override void OnBattleStart(RobotInstance robot)
    {
        // +25% ability damage
        robot.battleStats.abilitydamage *= 1.25;
        Debug.Log($"{robot.data.name} gains +25% Ability Damage from Ability Master!");
    }

    public override bool AllowBasicAttack(RobotInstance robot)
    {
        // Completely disable basic attacks
        return false;
    }
}