using System;

[Serializable]
public class CombatStats
{
    public double health;
    public double maxHealth;

    public double atkdamage;
    public double abilitydamage;
    public double abilityCount;

    public double atkspd;
    public double castspd;
    public double luck;

    public void CopyFrom(CombatStats other)
    {
        health        = other.health;
        maxHealth     = other.maxHealth;
        atkdamage     = other.atkdamage;
        abilitydamage = other.abilitydamage;
        abilityCount  = other.abilityCount;
        atkspd        = other.atkspd;
        castspd       = other.castspd;
        luck          = other.luck;
    }
}
