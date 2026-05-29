using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Robots/RobotData")]
public class RobotData : ScriptableObject
{
    [Header("Identity")]
    public string robotName;
    public GameObject prefab;

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