using System.Collections.Generic;
using UnityEngine;

public class BattlePoolManager : MonoBehaviour // USE THIS BY PUTTING THE POOL OF SCENES IN THE INSPECTOR
{
    public static BattlePoolManager Instance { get; private set; }

    [SerializeField] 
    private List<string> allBattleScenes = new List<string>();

    private Queue<string> battleQueue;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        RefillBattleQueue();
    }

    private void RefillBattleQueue()
    {
        // Create a shuffled queue of all battles
        List<string> shuffled = new List<string>(allBattleScenes);
        Shuffle(shuffled);

        battleQueue = new Queue<string>(shuffled);
    }

    public string GetNextBattle()
    {
        if (battleQueue.Count == 0)
            RefillBattleQueue();

        return battleQueue.Dequeue();
    }

    private void Shuffle(List<string> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);
            (list[i], list[rand]) = (list[rand], list[i]);
        }
    }
}
