using System.Collections.Generic;
using UnityEngine;

public class TechnologyManager : MonoBehaviour
{
    private Dictionary<int, Technology> techDict = new Dictionary<int, Technology>();
    private int nextId = 0;

    public Technology RegisterTech(Technology tech)
    {
        tech.id = nextId++;
        techDict[tech.id] = tech;
        return tech;
    }

    public Technology GetTech(int id)
    {
        return techDict.TryGetValue(id, out var tech) ? tech : null;
    }

    public void ClearAll()
    {
        techDict.Clear();
        nextId = 0;
    }

    public IEnumerable<Technology> GetAllTechnologies()
    {
        return techDict.Values;
    }

    public void LoadTechnologies(List<TechnologySaveData> saves)
    {
        ClearAll();

        foreach (var save in saves)
        {
            Technology tech = Technology.FromSaveData(save);
            techDict[tech.id] = tech;

            // Ensure nextId is always ahead
            if (tech.id >= nextId)
             nextId = tech.id + 1;
        }
    }
}