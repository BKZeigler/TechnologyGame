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
        Refresh(resources, new List<ResourceData>(), new List<ResourceData>());
        createButton.onClick.AddListener(() => OnCreatePressed?.Invoke());
    }

    public void Refresh(IEnumerable<ResourceData> inventory, List<ResourceData> scrapInput, List<ResourceData> modifierInput)
    {
        ClearContainer(inventoryContainer);
        ClearContainer(scrapInputContainer);
        ClearContainer(modifierInputContainer);

        foreach (var res in inventory)
            CreateEntry(res, inventoryContainer, () => OnResourceClicked?.Invoke(res));

        foreach (var res in scrapInput)
            CreateEntry(res, scrapInputContainer, () => OnInputClicked?.Invoke(res));

        foreach (var res in modifierInput)
            CreateEntry(res, modifierInputContainer, () => OnInputClicked?.Invoke(res));
    }

    private void CreateEntry(ResourceData res, Transform parent, Action onClick)
    {
        GameObject entry = Instantiate(resourceEntryPrefab, parent);
        entry.GetComponentInChildren<TextMeshProUGUI>().text = res.resourceName;
        entry.GetComponent<Button>().onClick.AddListener(() => onClick());
    }

    private void ClearContainer(Transform container)
    {
        foreach (Transform child in container)
            Destroy(child.gameObject);
    }
}