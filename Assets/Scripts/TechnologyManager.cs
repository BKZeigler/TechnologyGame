using System.Collections.Generic;
using UnityEngine;

public class TechnologyManager : MonoBehaviour
{
    public static TechnologyManager Instance { get; private set; }
    private Dictionary<int, Technology> techDict = new Dictionary<int, Technology>();
    private int nextId = 0;

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

    public Technology RegisterTech(Technology tech)
    {
        Technology newTech = Instantiate(tech); // clone so each tech is independent
        newTech.id = nextId++;
        techDict[newTech.id] = newTech;
        return newTech;
    }

    public Technology GetTech(int id)
    {
        return techDict.TryGetValue(id, out var tech) ? tech : null;
    }

    public bool TryGetTech(int id, out Technology tech)
    {
        return techDict.TryGetValue(id, out tech);
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