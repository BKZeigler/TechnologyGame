using System.Collections.Generic;
using UnityEngine;

public class TechnologyManager : MonoBehaviour
{
    public List<Technology> pendingTech = new List<Technology>();

    public void AddTech(Technology tech)
    {
        pendingTech.Add(tech);
    }

    public Technology GetTech(int index)
    {
        return pendingTech[index];
    }

    public void RemoveFirstTech()
    {
        if (pendingTech.Count > 0)
        {
            pendingTech.RemoveAt(0);
        }
    }   

    public Technology GetFirstTech()
    {
        if (pendingTech.Count > 0)
        {
            return pendingTech[0];
        }
        return null;
    }

    public void ClearTech()
    {
        pendingTech.Clear();
    }
    
}