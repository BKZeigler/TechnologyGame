using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Databases/AbilityDatabase")]
public class AbilityDatabase : ScriptableObject
{
    public static AbilityDatabase Instance { get; private set; }

    [SerializeField] private List<AbilityData> abilities;
    private Dictionary<int, AbilityData> abilityById;

    private void OnEnable()
    {
        Instance = this;
        abilityById = new Dictionary<int, AbilityData>();
        foreach (var ability in abilities)
            abilityById[ability.id] = ability;
    }

    public static AbilityData GetAbility(int id)
    {
        if (Instance == null || Instance.abilityById == null)
            return null;

        return Instance.abilityById.TryGetValue(id, out var ability) ? ability : null;
    }
}