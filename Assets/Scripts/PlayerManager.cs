using UnityEngine;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour //creates and owns persistent RobotInstances (data) that survive across scenes.
{
    public static PlayerManager Instance { get; private set; }

    public List<RobotInstance> robots = new List<RobotInstance>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Start()
    {
        //robots.Add(new RobotInstance(starterRobotData));
    }

    public List<RobotInstance> GetRobots()
    {
        return robots;
    }
}
