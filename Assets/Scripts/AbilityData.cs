using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Ability")]
public class AbilityData : ScriptableObject
{
    public int id;
    public string abilityName;
    [TextArea] public string description;
    // Add fields like cooldown, damage scaling, etc.
}
