using System.Collections.Generic;
using UnityEngine;

public class ResourceInventory : MonoBehaviour
{
    public static ResourceInventory Instance;

    // Persistent counts
    public Dictionary<ResourceData, int> resources = new Dictionary<ResourceData, int>();

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
    }

    public int GetAmount(ResourceData resource)
    {
        return resources.TryGetValue(resource, out int value) ? value : 0;
    }
}
