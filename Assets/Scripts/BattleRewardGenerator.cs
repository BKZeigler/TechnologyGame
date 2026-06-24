using UnityEngine;
using System.Collections.Generic;

public class BattleRewardGenerator : MonoBehaviour
{
    public List<ResourceData> possibleRewards;

    public List<(ResourceData, int)> GenerateRewards()
    {
        List<(ResourceData, int)> rewards = new List<(ResourceData, int)>();

        int rolls = Random.Range(2, 4); // 2–3 rewards

        for (int i = 0; i < rolls; i++)
        {
            ResourceData res = possibleRewards[Random.Range(0, possibleRewards.Count)];

            int amount = res.rarity switch
            {
                ResourceRarity.Common => Random.Range(3, 7),
                ResourceRarity.Rare => Random.Range(1, 3),
                ResourceRarity.Legendary => 1,
                _ => 1
            };
    
            rewards.Add((res, amount));
        }

        return rewards;
    }
}
