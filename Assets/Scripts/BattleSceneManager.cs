using UnityEngine;
using System.Collections.Generic;

public class BattleSceneManager : MonoBehaviour // spawns robots into the scene, instantiates them using RobotFactory
{

    [Header("Spawn Settings")]
    public float topY = 3.5f;      // Y position for enemies
    public float bottomY = -3.5f;  // Y position for player robots
    public float horizontalPadding = 1f; // space from screen edges

    void Start()
    {
        var robotInstances = PlayerManager.Instance.GetRobots();
        var enemyPrefabs = FindFirstObjectByType<EnemyManager>().GetEnemies();

        SpawnRobots(robotInstances, bottomY);
        SpawnUnits(enemyPrefabs, topY);
    }

    void SpawnRobots(List<RobotInstance> robots, float yPosition)
    {
        float screenWidth = Camera.main.orthographicSize * Camera.main.aspect * 2f;
        float usableWidth = screenWidth - (horizontalPadding * 2f);
        float spacing = usableWidth / (robots.Count + 1);

        for (int i = 0; i < robots.Count; i++)
        {
            float xPos = -screenWidth / 2f + horizontalPadding + spacing * (i + 1);
            Vector3 spawnPos = new Vector3(xPos, yPosition, 0);

            // Instantiate prefab from RobotData
            RobotFactory.SpawnRobot(robots[i], spawnPos);

            // Inject the RobotInstance into the Robot MonoBehaviour
            //Robot robotMB = robotObj.GetComponent<Robot>();
            //robotMB.Initialize(robots[i]);
        }
    }

    void SpawnUnits(List<GameObject> units, float yPosition)
    {
        if (units == null || units.Count == 0)
            return;

        float screenWidth = Camera.main.orthographicSize * Camera.main.aspect * 2f;
        float usableWidth = screenWidth - (horizontalPadding * 2f);
        float spacing = usableWidth / (units.Count + 1);

        for (int i = 0; i < units.Count; i++)
        {
            float xPos = -screenWidth / 2f + horizontalPadding + spacing * (i + 1);
            Vector3 spawnPos = new Vector3(xPos, yPosition, 0);
            Instantiate(units[i], spawnPos, Quaternion.identity);
        }
    }
}
