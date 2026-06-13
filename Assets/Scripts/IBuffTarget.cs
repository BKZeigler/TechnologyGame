public interface IBuffTarget
{
    CombatStats GetStats();
    void TakeDamage(double amount);
    double GetCurrentHealth();
    void SetCurrentHealth(double value);
}