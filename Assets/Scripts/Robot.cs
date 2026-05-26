using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{

    public float health = 0f;
    public float damage = 0f;
    public float range = 0f;
    public float atkspd = 0f;
    public float castspd = 0f;
    public Dictionary<int, Ability> abilityDict = new Dictionary<int, Ability>();
    public Dictionary<int, Passive> passiveDict = new Dictionary<int, Passive>();
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
