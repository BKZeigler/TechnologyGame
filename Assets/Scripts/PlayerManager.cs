using UnityEngine;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour //creates and owns persistent RobotInstances (data) that survive across scenes.
{
    public static PlayerManager Instance { get; private set; }

    public List<RobotInstance> robots = new List<RobotInstance>();

    private TechnologyManager techManager;

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
        techManager = FindFirstObjectByType<TechnologyManager>();
    }

    public List<RobotInstance> GetRobots()
    {
        return robots;
    }

    public void SaveGame()
    {
        GameSaveData save = new GameSaveData();

        // Save robots
        foreach (var robot in robots)
            save.robots.Add(robot.ToSaveData());

        // Save technologies
        foreach (var tech in techManager.GetAllTechnologies())
            save.technologies.Add(tech.ToSaveData());

        SaveSystem.Save(save);
    }

    public void LoadGame()
    {
        GameSaveData save = SaveSystem.Load();
        if (save == null)
        {
            Debug.LogWarning("No save file found");
            return;
        }

        robots.Clear();
        techManager.ClearAll();

        // Load technologies first
        foreach (var techSave in save.technologies)
        {
            Technology tech = Technology.FromSaveData(techSave);
            techManager.RegisterTech(tech);
        }

        // Load robots
        foreach (var robotSave in save.robots)
        {
            RobotInstance robot = RobotInstance.FromSaveData(robotSave);
            robots.Add(robot);
        }

        Debug.Log("Game loaded successfully");
    }
}
