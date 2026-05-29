using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Databases/PassiveDatabase")]
public class PassiveDatabase : ScriptableObject
{
    public static PassiveDatabase Instance { get; private set; }

    [SerializeField] private List<PassiveData> passives;
    private Dictionary<int, PassiveData> passiveById;

    private void OnEnable()
    {
        Instance = this;
        passiveById = new Dictionary<int, PassiveData>();
        foreach (var passive in passives)
            passiveById[passive.id] = passive;
    }

    public static PassiveData GetPassive(int id)
    {
        if (Instance == null || Instance.passiveById == null)
            return null;

        return Instance.passiveById.TryGetValue(id, out var passive) ? passive : null;
    }
}
