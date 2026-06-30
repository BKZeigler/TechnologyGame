using UnityEngine;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    public List<RobotInstance> robots = new List<RobotInstance>();
    public bool HasLoadedGame { get; private set; } = false;

    private TechnologyManager techManager;
    private MapSaveManager mapSaveManager;

    // -------------------------
    // Global battle buffs
    // -------------------------
    public float globalCastSpeedMultiplier = 1f;

    public void AddGlobalCastSpeed(float amount)
    {
        globalCastSpeedMultiplier += amount;
    }

    public void ResetGlobalBuffs()
    {
        globalCastSpeedMultiplier = 1f;
    }

    // -------------------------
    // Unity Lifecycle
    // -------------------------
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

    // -------------------------
    // Robot Access
    // -------------------------
    public List<RobotInstance> GetRobots()
    {
        return robots;
    }

    // -------------------------
    // Saving
    // -------------------------
    public void SaveGame()
    {
        GameSaveData save = new GameSaveData();

        foreach (var robot in robots)
            save.robots.Add(robot.ToSaveData());

        foreach (var tech in techManager.GetAllTechnologies())
            save.technologies.Add(tech.ToSaveData());

        // Resources
        save.resourceNames = new List<string>();
        save.resourceAmounts = new List<int>();

        foreach (var kvp in ResourceInventory.Instance.resources)
        {
            save.resourceNames.Add(kvp.Key.resourceName);
            save.resourceAmounts.Add(kvp.Value);
        }

        SaveSystem.Save(save);
    }

    public void SaveGameFromMap(GridManager grid, PlayerMarker marker)
    {
        GameSaveData save = new GameSaveData();

        foreach (var robot in robots)
            save.robots.Add(robot.ToSaveData());

        foreach (var tech in techManager.GetAllTechnologies())
            save.technologies.Add(tech.ToSaveData());

        MapSaveManager.Instance.SaveMap(save, grid, marker);

        // Resources
        save.resourceNames = new List<string>();
        save.resourceAmounts = new List<int>();

        foreach (var kvp in ResourceInventory.Instance.resources)
        {
            if (kvp.Key == null)
            {
                Debug.LogError("ResourceInventory contains NULL key — this should never happen.");
                continue;
            }

            save.resourceNames.Add(kvp.Key.resourceName);
            save.resourceAmounts.Add(kvp.Value);
        }
        Debug.Log("Saving resources: " + ResourceInventory.Instance.resources.Count);

        SaveSystem.Save(save);
    }

    public void SaveGameAfterReward()
    {
        GameSaveData save = SaveSystem.Load();
        if (save == null)
            save = new GameSaveData();

        save.robots.Clear();
        foreach (var robot in robots)
            save.robots.Add(robot.ToSaveData());

        save.technologies.Clear();
        foreach (var tech in techManager.GetAllTechnologies())
            save.technologies.Add(tech.ToSaveData());

        // Resources
        save.resourceNames = new List<string>();
        save.resourceAmounts = new List<int>();

        foreach (var kvp in ResourceInventory.Instance.resources)
        {
            save.resourceNames.Add(kvp.Key.resourceName);
            save.resourceAmounts.Add(kvp.Value);
        }

        SaveSystem.Save(save);
    }

    // -------------------------
    // Loading
    // -------------------------
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

        foreach (var techSave in save.technologies)
        {
            Technology tech = Technology.FromSaveData(techSave);
            techManager.RegisterTech(tech);
        }

        foreach (var robotSave in save.robots)
        {
            RobotInstance robot = RobotInstance.FromSaveData(robotSave);
            robots.Add(robot);
        }

        if (mapSaveManager != null)
            mapSaveManager.LoadMap(save);

        // Resources
        //ResourceInventory.Instance.resources.Clear();

        //for (int i = 0; i < save.resourceNames.Count; i++)
        //{
        //    ResourceData data = ResourceDatabase.Instance.GetByName(save.resourceNames[i]);
        //    if (data != null)
        //        ResourceInventory.Instance.resources[data] = save.resourceAmounts[i];
        //}

        Debug.Log("Game loaded successfully");
    }
}