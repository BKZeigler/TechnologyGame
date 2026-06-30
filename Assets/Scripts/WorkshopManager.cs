using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class WorkshopManager : MonoBehaviour
{
    public WorkshopUI ui;
    public TechnologyCreator techCreator;

    private List<ResourceData> scrapInput = new();
    private List<ResourceData> modifierInput = new();

    private void Start()
    {
        Debug.Log("WorkshopManager Start");
        ui.Initialize(ResourceInventory.Instance.resources.Keys);
        foreach (var kvp in ResourceInventory.Instance.resources)
        {
            Debug.Log("Key: " + (kvp.Key == null ? "NULL" : kvp.Key.resourceName));
        }
        Debug.Log("Workshop inventory count: " + ResourceInventory.Instance.resources.Count);
        ui.OnResourceClicked += HandleResourceClicked;
        ui.OnInputClicked += HandleInputClicked;
        ui.OnCreatePressed += HandleCreatePressed;
    }

    private void HandleResourceClicked(ResourceData resource)
    {
        // Move from inventory to input
        if (resource.rarity == ResourceRarity.Common)
            scrapInput.Add(resource);
        else
            modifierInput.Add(resource);

        ResourceInventory.Instance.resources[resource]--;
        ui.Refresh(ResourceInventory.Instance.resources.Keys, scrapInput, modifierInput);
    }

    private void HandleInputClicked(ResourceData resource)
    {
        // Move back to inventory
        ResourceInventory.Instance.Add(resource, 1);
        scrapInput.Remove(resource);
        modifierInput.Remove(resource);
        ui.Refresh(ResourceInventory.Instance.resources.Keys, scrapInput, modifierInput);
    }

    private void HandleCreatePressed()
    {
        //techCreator.GenerateTechnology(scrapInput, modifierInput);
        techCreator.CreateTechnology(scrapInput.Count); // will eventually use modifiers, right now scrap is value
        SceneManager.LoadScene("TechRewardScene");
    }
}