using UnityEngine;
using System.Collections.Generic;

public class BattleSceneManager : MonoBehaviour
{
    [Header("Spawn Settings")]
    public float topY = 3.5f;
    public float bottomY = -3.5f;
    public float horizontalPadding = 1f;

    private BattleContext context;

    void Start()
    {
        context = FindAnyObjectByType<BattleContext>(); // safe replacement for deprecated API

        var robotInstances = PlayerManager.Instance.GetRobots();
        var enemyPrefabs = EnemyManager.Instance.GetEnemies(); // use your existing manager

        SpawnRobots(robotInstances, bottomY);
        SpawnEnemies(enemyPrefabs, topY);
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

            // Spawn robot using your existing factory
            GameObject robotObj = RobotFactory.SpawnRobot(robots[i], spawnPos);

            // Inject combat logic
            RobotCombat combat = robotObj.GetComponent<RobotCombat>();
            combat.Initialize(robots[i], context);

            robots[i].combat = combat; 

            context.robots.Add(combat);
        }
    }

    void SpawnEnemies(List<GameObject> units, float yPosition)
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

            GameObject enemyObj = Instantiate(units[i], spawnPos, Quaternion.identity);

            EnemyCombat combat = enemyObj.GetComponent<EnemyCombat>();
            combat.Initialize(context);

            context.enemies.Add(combat);
        }
    }
}