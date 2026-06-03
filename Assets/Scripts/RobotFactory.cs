using UnityEngine;

public static class RobotFactory
{
    public static GameObject SpawnRobot(RobotData data, Vector3 position)
    {
        // Create runtime instance
        RobotInstance instance = new RobotInstance(data);

        // Instantiate prefab
        GameObject obj = GameObject.Instantiate(data.prefab, position, Quaternion.identity);

        // Inject instance
        Robot robot = obj.GetComponent<Robot>();
        robot.Initialize(instance);

        return obj; // return the GameObject
    }

    public static GameObject SpawnRobot(RobotInstance instance, Vector3 position)
    {
        GameObject obj = GameObject.Instantiate(instance.data.prefab, position, Quaternion.identity);

        Robot robot = obj.GetComponent<Robot>();
        robot.Initialize(instance);

        return obj; // return the GameObject
    }
}