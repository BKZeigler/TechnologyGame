using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class RobotSaveData
{
    public string robotDataName;   // reference to RobotData asset
    public double health;
    public double atkdamage;
    public double abilitydamage;
    public double abilityCount;
    public double atkspd;
    public double castspd;
    public double luck;

    public List<int> abilityIDs;
    public List<int> passiveIDs;
    public List<int> technologyIDs;
}
