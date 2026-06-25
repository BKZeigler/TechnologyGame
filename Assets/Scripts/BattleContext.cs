using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleContext : MonoBehaviour
{
    public List<RobotCombat> robots = new List<RobotCombat>();
    public List<EnemyCombat> enemies = new List<EnemyCombat>();

    public event Action OnEnemyListChanged;

    public void NotifyEnemyDied(EnemyCombat enemy)
    {
        enemies.Remove(enemy);
        OnEnemyListChanged?.Invoke();
    }
}
