using UnityEngine;

public static class RobotFactory // Takes a RobotInstance and turns it into a live GameObject
{
    public static Robot SpawnRobot(RobotData data, Vector3 position)
    {
        // Create the runtime instance (persistent data)
        RobotInstance instance = new RobotInstance(data);

        // Instantiate the prefab
        GameObject obj = GameObject.Instantiate(data.prefab, position, Quaternion.identity);

        // Get the Robot MonoBehaviour
        Robot robot = obj.GetComponent<Robot>();

        // Inject the instance
        robot.Initialize(instance);

        return robot;
    }

    public static Robot SpawnRobot(RobotInstance instance, Vector3 position) //RobotFactory.SpawnRobot(robotInstances[i], spawnPos);
    {
        // Instantiate from existing instance (e.g., PlayerManager robots)
        GameObject obj = GameObject.Instantiate(instance.data.prefab, position, Quaternion.identity);

        Robot robot = obj.GetComponent<Robot>();
        robot.Initialize(instance);

        return robot;
    }
}
