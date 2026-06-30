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

    public void Initialize(IEnumerable<ResourceData> resources)
    {
        Debug.Log("WorkshopUI Intialize");
        Refresh(resources, new List<ResourceData>(), new List<ResourceData>());
        createButton.onClick.AddListener(() => OnCreatePressed?.Invoke());
    }

    public void Refresh(IEnumerable<ResourceData> inventory, List<ResourceData> scrapInput, List<ResourceData> modifierInput)
    {
        ClearContainer(inventoryContainer);
        ClearContainer(scrapInputContainer);
        ClearContainer(modifierInputContainer);

        foreach (var res in inventory)
        {
            Debug.Log("Key: " + (res == null ? "NULL" : res.resourceName));
            CreateEntry(res, inventoryContainer, () => OnResourceClicked?.Invoke(res));
        }

        foreach (var res in scrapInput)
            CreateEntry(res, scrapInputContainer, () => OnInputClicked?.Invoke(res));

        foreach (var res in modifierInput)
            CreateEntry(res, modifierInputContainer, () => OnInputClicked?.Invoke(res));
    }

    private void CreateEntry(ResourceData res, Transform parent, Action onClick)
    {
        GameObject entry = Instantiate(resourceEntryPrefab, parent);

        var tmp = entry.GetComponentInChildren<TextMeshProUGUI>();
        if (tmp == null)
        {
            Debug.LogError("Entry prefab is missing TextMeshProUGUI!");
            return;
        }

        tmp.text = res.resourceName;

        var button = entry.GetComponent<Button>();
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