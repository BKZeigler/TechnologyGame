using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Databases/AbilityDatabase")]
public class AbilityDatabase : ScriptableObject
{
    public static AbilityDatabase Instance { get; private set; }

    [SerializeField] private List<AbilityData> abilities;
    private Dictionary<int, AbilityData> abilityById;
    public List<AbilityData> weakAbilities = new List<AbilityData>();
    public List<AbilityData> moderateAbilities = new List<AbilityData>();
    public List<AbilityData> strongAbilities = new List<AbilityData>();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        // Force Unity to load the asset from Resources
        Resources.Load<AbilityDatabase>("Ability Database");
    }
    private void OnEnable()
    {
        Instance = this;
        abilityById = new Dictionary<int, AbilityData>();
        foreach (var ability in abilities)
            abilityById[ability.id] = ability;

        weakAbilities.Clear();
        moderateAbilities.Clear();
        strongAbilities.Clear();

        foreach (var ability in abilities)
        {
            switch (ability.tier)
            {
                case TechTier.Weak:
                    weakAbilities.Add(ability);
                    break;
                case TechTier.Moderate:
                    moderateAbilities.Add(ability);
                    break;
                case TechTier.Strong:
                    strongAbilities.Add(ability);
                    break;
            }
        }
    }

    public static AbilityData GetAbility(int id)
    {
        if (Instance == null || Instance.abilityById == null)
            return null;

        return Instance.abilityById.TryGetValue(id, out var ability) ? ability : null;
    }
}