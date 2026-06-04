using UnityEngine;
using System.Collections.Generic;

public class PlayerAbilityPrompter : MonoBehaviour
{
    public float castSpeed = 0.5f; // seconds per cycle = 1 / castSpeed
    public int robotsPerPrompt = 3;

    private float timer = 0f;
    private int nextRobotIndex = 0;

    private List<RobotInstance> robots;

    void Start()
    {
        robots = PlayerManager.Instance.GetRobots();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= (1f / castSpeed))
        {
            timer = 0f;
            PromptRobots();
        }
    }

    void PromptRobots()
    {
        for (int i = 0; i < robotsPerPrompt; i++)
        {
            int index = (nextRobotIndex + i) % robots.Count;
            robots[index].CastNextAbilities();
        }

        nextRobotIndex = (nextRobotIndex + robotsPerPrompt) % robots.Count;
    }
}
