using UnityEngine;

public enum ResourceRarity { Common, Rare, Legendary }

[CreateAssetMenu(menuName = "Resources/Resource")]
public class ResourceData : ScriptableObject
{
    public string resourceName;
    public ResourceRarity rarity;
}
