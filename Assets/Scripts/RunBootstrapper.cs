using UnityEngine;

public class RunBootstrapper : MonoBehaviour
{
    [Header("Starting Robot Type")]
    public RobotData basicRobotData;
    public RobotData newNameRobotData;

    void Start()
    {
        StartNewRun();
    }

    public void StartNewRun()
    {
        // Clear any leftover robots from previous runs
        PlayerManager.Instance.robots.Clear();

        // Create four identical robots
        RobotInstance robot1 = new RobotInstance(basicRobotData);
        RobotInstance robot2 = new RobotInstance(newNameRobotData);
        //RobotInstance robot3 = new RobotInstance(basicRobotData);
        //RobotInstance robot4 = new RobotInstance(basicRobotData);

        // Add them to the player's persistent roster
        PlayerManager.Instance.robots.Add(robot1);
        PlayerManager.Instance.robots.Add(robot2);
        //PlayerManager.Instance.robots.Add(robot3);
        //PlayerManager.Instance.robots.Add(robot4);

        Debug.Log("RunBootstrapper: Created 2 basic robots for the new run.");
    }
}
