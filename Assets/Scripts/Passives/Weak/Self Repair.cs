using UnityEngine;

[CreateAssetMenu(menuName = "Passives/Self-Repair")]
public class SelfRepairPassive : PassiveData
{
    private float timer = 0f;

    private void OnEnable()
    {
        passiveName = "Self-Repair";
    }

    public override void OnBattleStart(RobotInstance robot)
    {
        timer = 0f; // reset timer each battle
    }

    public override void OnUpdate(RobotInstance robot, float deltaTime)
    {
        timer += deltaTime;

        if (timer >= 5f)
        {
            timer -= 5f;

            double healAmount = robot.battleStats.maxHealth * 0.05;
            double newHP = Mathf.Min(
                (float)(robot.battleStats.health + healAmount),
                (float)robot.battleStats.maxHealth
            );

            robot.SetCurrentHealth(newHP);

            Debug.Log($"{robot.data.name}'s Self-Repair restores {healAmount} HP!");
        }
    }
}