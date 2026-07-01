using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

public class WorkshopUI : MonoBehaviour
{
    public Transform inventoryContainer;
    public Transform scrapInputContainer;
    public Transform modifierInputContainer;
    public GameObject resourceEntryPrefab;
    public Button createButton;

    public event Action<ResourceData> OnResourceClicked;
    public event Action<ResourceData> OnInputClicked;
    public event Action OnCreatePressed;

    public void Initialize(
        Dictionary<ResourceData, int> inventory,
        Dictionary<ResourceData, int> scrapInput,
        Dictionary<ResourceData, int> modifierInput)
    {
        Debug.Log("WorkshopUI Initialize");
        Refresh(inventory, scrapInput, modifierInput);
        createButton.onClick.AddListener(() => OnCreatePressed?.Invoke());
    }

    public void Refresh(
        Dictionary<ResourceData, int> inventory,
        Dictionary<ResourceData, int> scrapInput,
        Dictionary<ResourceData, int> modifierInput)
    {
        ClearContainer(inventoryContainer);
        ClearContainer(scrapInputContainer);
        ClearContainer(modifierInputContainer);

        // Inventory entries
        foreach (var kvp in inventory)
        {
            var res = kvp.Key;
            var amount = kvp.Value;
            if (res == null) continue;

            CreateEntry(res, amount, inventoryContainer, () => OnResourceClicked?.Invoke(res));
        }

        // Scrap input entries
        foreach (var kvp in scrapInput)
        {
            var res = kvp.Key;
            var amount = kvp.Value;
            if (res == null || amount <= 0) continue;

            CreateEntry(res, amount, scrapInputContainer, () => OnInputClicked?.Invoke(res));
        }

        // Modifier input entries
        foreach (var kvp in modifierInput)
        {
            var res = kvp.Key;
            var amount = kvp.Value;
            if (res == null || amount <= 0) continue;

            CreateEntry(res, amount, modifierInputContainer, () => OnInputClicked?.Invoke(res));
        }
    }

    private void CreateEntry(ResourceData res, int amount, Transform parent, Action onClick)
    {
        GameObject entry = Instantiate(resourceEntryPrefab, parent);

        var tmp = entry.GetComponentInChildren<TextMeshProUGUI>();
        if (tmp == null)
        {
            Debug.LogError("Entry prefab is missing TextMeshProUGUI!");
            return;
        }

        tmp.text = $"{res.resourceName} x{amount}";

        var button = entry.GetComponentInChildren<Button>();
        if (button == null)
        {
            Debug.LogError("Entry prefab is missing Button component!");
            return;
        }

        button.onClick.AddListener(() => onClick());
    }

    private void ClearContainer(Transform container)
    {
        foreach (Transform child in container)
            Destroy(child.gameObject);
    }
}