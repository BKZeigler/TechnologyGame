using System.Collections.Generic;
using UnityEngine;

public class ResourceInventory : MonoBehaviour
{
    public static ResourceInventory Instance;

    public Dictionary<ResourceData, int> resources = new Dictionary<ResourceData, int>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadFromSave();   // <-- THIS MUST BE HERE
    }

    public void Add(ResourceData resource, int amount)
    {
        if (!resources.ContainsKey(resource))
            resources[resource] = 0;

        resources[resource] += amount;

        Debug.Log($"Added {amount} of {resource.resourceName}. New total: {resources[resource]}");
    }

    public int GetAmount(ResourceData resource)
    {
        return resources.TryGetValue(resource, out int value) ? value : 0;
    }

    private void LoadFromSave()
    {
        GameSaveData save = SaveSystem.Load();
        if (save == null)
            return;

        resources.Clear();

        for (int i = 0; i < save.resourceNames.Count; i++)
        {
            ResourceData data = ResourceDatabase.Instance.GetByName(save.resourceNames[i]);

            if (data == null)
            {
                Debug.LogWarning($"Save file contains unknown resource: {save.resourceNames[i]}");
                continue;
            }

            resources[data] = save.resourceAmounts[i];
        }
    }
}