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
        ui.Initialize(ResourceInventory.Instance.resources.Keys);
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
        SceneManager.LoadScene("TechRewardScene");
    }
}