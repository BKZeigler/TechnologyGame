using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Robots/RobotData")]
public class RobotData : ScriptableObject // every robot type in your game must have a RobotData ScriptableObject.
{                                         // Ex: RobotData_Sniper.asset, RobotData_Tank.asset
    [Header("Identity")]                  // Each contains base stats, base abilites/passives, prefab reference
    public string robotName;              // Player manager creates RobotInstances like robots.Add(new RobotInstance(robotData_Sniper))
    public GameObject prefab;             // And then BattleSceneManager spawns them.

    [Header("Base Stats")]
    public double baseHealth;
    public double baseAtkDamage;
    public double baseAbilityDamage;
    public double baseAbilityCount;
    public double baseAtkSpeed;
    public double baseCastSpeed;
    public double baseLuck;

    [Header("Base Abilities & Passives (IDs Only)")]
    public List<int> baseAbilityIDs;
    public List<int> basePassiveIDs;
}