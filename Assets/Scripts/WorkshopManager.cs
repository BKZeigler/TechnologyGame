using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class WorkshopManager : MonoBehaviour
{
    public WorkshopUI ui;
    public TechnologyCreator techCreator;

    // Amounts in inputs (per resource)
    private Dictionary<ResourceData, int> scrapInput = new();
    private Dictionary<ResourceData, int> modifierInput = new();

    private void Start()
    {
        Debug.Log("WorkshopManager Start");

        // Build a snapshot of inventory amounts from ResourceInventory
        var inventoryAmounts = new Dictionary<ResourceData, int>();
        foreach (var kvp in ResourceInventory.Instance.resources)
        {
            if (kvp.Key == null) continue;
            inventoryAmounts[kvp.Key] = kvp.Value;
        }

        Debug.Log("Workshop inventory count: " + inventoryAmounts.Count);

        ui.Initialize(inventoryAmounts, scrapInput, modifierInput);

        ui.OnResourceClicked += HandleResourceClicked;
        ui.OnInputClicked += HandleInputClicked;
        ui.OnCreatePressed += HandleCreatePressed;
    }

    private void HandleResourceClicked(ResourceData resource)
    {
        // Current inventory amount
        int invAmount = ResourceInventory.Instance.GetAmount(resource);
        int scrapAmount = scrapInput.TryGetValue(resource, out var s) ? s : 0;
        int modAmount = modifierInput.TryGetValue(resource, out var m) ? m : 0;

        // Total already allocated to inputs
        int alreadyAllocated = scrapAmount + modAmount;

        // Prevent going below zero
        if (invAmount - alreadyAllocated <= 0)
        {
            Debug.Log($"Cannot move more {resource.resourceName}, not enough in inventory.");
            return;
        }

        // Allocate one unit to the appropriate input
        if (resource.rarity == ResourceRarity.Scrap)
        {
            scrapInput[resource] = scrapAmount + 1;
        }
        else
        {
            modifierInput[resource] = modAmount + 1;
        }

        Debug.Log($"Allocated 1 {resource.resourceName} to input. Total allocated: {alreadyAllocated + 1}");

        RefreshUI();
    }

    private void HandleInputClicked(ResourceData resource)
    {
        // Move one unit back from input to inventory
        if (scrapInput.TryGetValue(resource, out var scrapAmount) && scrapAmount > 0)
        {
            scrapAmount--;
            if (scrapAmount <= 0)
                scrapInput.Remove(resource);
            else
                scrapInput[resource] = scrapAmount;
        }
        else if (modifierInput.TryGetValue(resource, out var modAmount) && modAmount > 0)
        {
            modAmount--;
            if (modAmount <= 0)
                modifierInput.Remove(resource);
            else
                modifierInput[resource] = modAmount;
        }
        else
        {
            Debug.Log($"No {resource.resourceName} in inputs to remove.");
            return;
        }

        Debug.Log($"Returned 1 {resource.resourceName} from input.");

        RefreshUI();
    }

    private void HandleCreatePressed()
    {
        int totalScrap = 0;
        foreach (var kvp in scrapInput)
            totalScrap += kvp.Value;

        // Apply the scrap cost to the real inventory
        foreach (var kvp in scrapInput)
        {
            ResourceInventory.Instance.resources[kvp.Key] -= kvp.Value;
        }
        foreach (var kvp in modifierInput)
        {
            ResourceInventory.Instance.resources[kvp.Key] -= kvp.Value;
        }

        Technology tech = techCreator.CreateTechnology(totalScrap, modifierInput); // create the tech based on inputs

        PlayerManager.Instance.pendingCraftedTech = tech; // store it for nexr scene

        SceneManager.LoadScene("TechRewardScene");
    }

    private void RefreshUI()
    {
        var inventoryAmounts = new Dictionary<ResourceData, int>();

        foreach (var kvp in ResourceInventory.Instance.resources)
        {
            var res = kvp.Key;
            int baseAmount = kvp.Value;

            int scrapUsed = scrapInput.TryGetValue(res, out var s) ? s : 0;
            int modUsed = modifierInput.TryGetValue(res, out var m) ? m : 0;

            int remaining = baseAmount - scrapUsed - modUsed;

            inventoryAmounts[res] = Mathf.Max(remaining, 0);
        }

        ui.Refresh(inventoryAmounts, scrapInput, modifierInput);
    }
}