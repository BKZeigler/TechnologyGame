using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Resources/ResourceDatabase")]
public class ResourceDatabase : ScriptableObject
{
    private static ResourceDatabase _instance;
    public static ResourceDatabase Instance
    {
        get
        {
            if (_instance == null)
                _instance = Resources.Load<ResourceDatabase>("ResourceDatabase");
            return _instance;
        }
    }

    [Header("All ResourceData assets")]
    public List<ResourceData> allResources = new List<ResourceData>();

    // Lookup table: name → ResourceData
    private Dictionary<string, ResourceData> lookup;

    private void OnEnable()
    {
        BuildLookup();
    }

    private void BuildLookup()
    {
        lookup = new Dictionary<string, ResourceData>();

        foreach (var res in allResources)
        {
            if (!lookup.ContainsKey(res.resourceName))
                lookup.Add(res.resourceName, res);
        }
    }

    public ResourceData GetByName(string name)
    {
        if (lookup == null || lookup.Count == 0)
            BuildLookup();

        lookup.TryGetValue(name, out ResourceData result);
        return result;
    }
}