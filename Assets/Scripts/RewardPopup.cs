using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class RewardPopup : MonoBehaviour
{
    [Header("UI References")]
    public Transform rewardContainer;
    public GameObject rewardEntryPrefab;

    public Button acceptButton;
    public Button skipButton;

    private List<(ResourceData, int)> pendingRewards;

    private void Start()
    {
        // Hook up button listeners
        acceptButton.onClick.AddListener(OnAcceptPressed);
        skipButton.onClick.AddListener(OnSkipPressed);
    }

    // Called by BattleSceneManager or MapScene
    public void Show(List<(ResourceData, int)> rewards)
    {
        pendingRewards = rewards;

        // Clear old entries
        foreach (Transform child in rewardContainer)
            Destroy(child.gameObject);

        // Populate UI
        foreach (var reward in rewards)
        {
            GameObject entry = Instantiate(rewardEntryPrefab, rewardContainer);
            entry.GetComponentInChildren<TextMeshProUGUI>().text =
                $"{reward.Item1.resourceName} x{reward.Item2}";
        }
    }

    private void OnAcceptPressed()
    {
        foreach (var reward in pendingRewards)
            ResourceInventory.Instance.Add(reward.Item1, reward.Item2);

        ClosePopup();
    }

    private void OnSkipPressed()
    {
        ClosePopup();
    }

    private void ClosePopup()
    {
        Destroy(gameObject);

        // Return to map (or whatever scene you want)
        SceneManager.LoadScene("MapScene");
    }
}