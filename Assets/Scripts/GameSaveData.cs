using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GameSaveData
{
    public List<RobotSaveData> robots = new List<RobotSaveData>();
    public List<TechnologySaveData> technologies = new List<TechnologySaveData>();

    // Map-related
    public int playerX;
    public int playerY;
    public List<TileEventSaveData> mapEvents = new List<TileEventSaveData>();

    // Resource-related
    public List<string> resourceNames = new List<string>();
    public List<int> resourceAmounts = new List<int>();
}