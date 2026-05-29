using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Passive")]
public class PassiveData : ScriptableObject
{
    public int id;
    public string passiveName;
    [TextArea] public string description;
    // Add fields like modifiers, triggers, etc.
}
