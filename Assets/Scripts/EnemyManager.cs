using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemyPrefabs;

    public List<GameObject> GetEnemies()
    {
        return enemyPrefabs;
    }
}
