using UnityEngine;

public abstract class PassiveData : ScriptableObject
{
    public int id;
    public string passiveName;
    public string description;
    // Return false to block the buff
    public virtual bool AllowBuff(RobotInstance robot, Buff buff)
    {
        return true;
    }
}
