using UnityEngine;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    public List<RobotInstance> robots = new List<RobotInstance>();

    public bool HasLoadedGame { get; private set; } = false;

    private TechnologyManager techManager;
    private MapSaveManager mapSaveManager;

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

    private void Start()
    {
        techManager = FindFirstObjectByType<TechnologyManager>();
        mapSaveManager = FindFirstObjectByType<MapSaveManager>();
    }

    public List<RobotInstance> GetRobots()
    {
        return robots;
    }

    public void SaveGame()
    {
        GameSaveData save = new GameSaveData();

        // Robots
        foreach (var robot in robots)
            save.robots.Add(robot.ToSaveData());

        // Technologies
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
        
        HasLoadedGame = true;

        robots.Clear();
        techManager.ClearAll();

        // Technologies
        foreach (var techSave in save.technologies)
        {
            Technology tech = Technology.FromSaveData(techSave);
            techManager.RegisterTech(tech);
        }

        // Robots
        foreach (var robotSave in save.robots)
        {
            RobotInstance robot = RobotInstance.FromSaveData(robotSave);
            robots.Add(robot);
        }

        // Map
        if (mapSaveManager != null)
            mapSaveManager.LoadMap(save);

        Debug.Log("Game loaded successfully");
    }

    public void SaveGameFromMap(GridManager grid, PlayerMarker marker)
    {
        GameSaveData save = new GameSaveData();

        // Robots
        foreach (var robot in robots)
            save.robots.Add(robot.ToSaveData());

        // Technologies
        foreach (var tech in techManager.GetAllTechnologies())
            save.technologies.Add(tech.ToSaveData());

        // Map
        MapSaveManager.Instance.SaveMap(save, grid, marker);

        SaveSystem.Save(save);
    }

    
}