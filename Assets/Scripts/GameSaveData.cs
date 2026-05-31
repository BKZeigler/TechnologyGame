using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GameSaveData
{
    public List<RobotSaveData> robots = new List<RobotSaveData>();
    public List<TechnologySaveData> technologies = new List<TechnologySaveData>();
}