using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class TechnologySaveData
{
    public int id;
    public string techName;
    public double[] stats;
    public List<int> abilityIDs;
    public List<int> passiveIDs;
}
