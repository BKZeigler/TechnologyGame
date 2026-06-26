using System.Collections.Generic;
using UnityEngine;

public class ResourceInventory : MonoBehaviour
{
    public static ResourceInventory Instance;

    // Persistent counts
    public Dictionary<ResourceData, int> resources = new Dictionary<ResourceData, int>();
    public List<ResourceData> debugResources = new List<ResourceData>();
    public List<int> debugResourceAmounts = new List<int>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Add(ResourceData resource, int amount)
    {
        if (!resources.ContainsKey(resource))
            resources[resource] = 0;

        resources[resource] += amount;

        // Update debug lists
        if (!debugResources.Contains(resource))
        {
            debugResources.Add(resource);
            debugResourceAmounts.Add(0);
        }

        int index = debugResources.IndexOf(resource);
        debugResourceAmounts[index] += amount;
    }

    public int GetAmount(ResourceData resource)
    {
        return resources.TryGetValue(resource, out int value) ? value : 0;
    }
}
