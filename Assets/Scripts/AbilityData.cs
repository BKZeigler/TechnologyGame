using UnityEngine;

public abstract class AbilityData : ScriptableObject
{
    public int id;
    public string abilityName;
    public string description;
    public TechTier tier;

    // Base scaling values
    public float baseDamage;
    public float apScaling;   // ability power scaling
    public float luckScaling; // crit or bonus scaling

    // Execute the ability
    public abstract void Execute(RobotInstance caster);
}
