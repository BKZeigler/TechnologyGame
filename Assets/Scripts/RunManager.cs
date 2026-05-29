using UnityEngine;
using System.Collections.Generic;

public class RunManager : MonoBehaviour
{
    public static RunManager Instance { get; private set; }

    [SerializeField] private TechnologyNameDatabase nameDatabase;

    private List<string> unusedNames = new List<string>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        ResetNamePool();
    }

    public void ResetNamePool()
    {
        unusedNames = new List<string>(nameDatabase.allNames);
    }

    public string GetRandomUnusedName()
    {
        if (unusedNames.Count == 0)
            ResetNamePool(); // fallback safety

        int index = Random.Range(0, unusedNames.Count);
        string name = unusedNames[index];
        unusedNames.RemoveAt(index);
        return name;
    }
}