using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Databases/TechnologyNameDatabase")]
public class TechnologyNameDatabase : ScriptableObject
{
    public List<string> allNames = new List<string>();
}
