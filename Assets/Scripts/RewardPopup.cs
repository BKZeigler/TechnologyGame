using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class RewardPopup : MonoBehaviour
{
    public Transform rewardContainer;
    public GameObject rewardEntryPrefab;

    private List<(ResourceData, int)> pending;

    public void Show(List<(ResourceData, int)> rewards)
    {
        pending = rewards;

        foreach (Transform child in rewardContainer)
            Destroy(child.gameObject);

        foreach (var reward in rewards)
        {
            GameObject entry = Instantiate(rewardEntryPrefab, rewardContainer);
            entry.GetComponentInChildren<TextMeshProUGUI>().text =
                $"{reward.Item1.resourceName} x{reward.Item2}";
        }
    }

    public void Accept()
    {
        foreach (var reward in pending)
            ResourceInventory.Instance.Add(reward.Item1, reward.Item2);

        Close();
    }

    public void Skip()
    {
        Close();
    }

    private void Close()
    {
        Destroy(gameObject);
        SceneManager.LoadScene("MapScene");
    }
}