using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Databases/PassiveDatabase")]
public class PassiveDatabase : ScriptableObject
{
    public static PassiveDatabase Instance { get; private set; }

    [SerializeField] private List<PassiveData> passives;
    private Dictionary<int, PassiveData> passiveById;

    public List<PassiveData> weakPassives = new List<PassiveData>();
    public List<PassiveData> moderatePassives = new List<PassiveData>();
    public List<PassiveData> strongPassives = new List<PassiveData>();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        // Force Unity to load the asset from Resources
        Resources.Load<PassiveDatabase>("Passive Database");
    }
    private void OnEnable()
    {
        Instance = this;
        passiveById = new Dictionary<int, PassiveData>();

        foreach (var passive in passives)
            passiveById[passive.id] = passive;

        weakPassives.Clear();
        moderatePassives.Clear();
        strongPassives.Clear();

        foreach (var passive in passives)
        {
            switch (passive.tier)
            {
                case TechTier.Weak:
                    weakPassives.Add(passive);
                    break;
                case TechTier.Moderate:
                    moderatePassives.Add(passive);
                    break;
                case TechTier.Strong:
                    strongPassives.Add(passive);
                    break;
            }
        }
    }

    public static PassiveData GetPassive(int id)
    {
        if (Instance == null || Instance.passiveById == null)
            return null;

        return Instance.passiveById.TryGetValue(id, out var passive) ? passive : null;
    }
}
