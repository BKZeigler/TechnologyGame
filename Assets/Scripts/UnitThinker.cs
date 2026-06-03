using UnityEngine;

public abstract class UnitThinker : MonoBehaviour
{
    protected float thinkTimer;
    protected float thinkInterval;

    protected virtual void Update()
    {
        thinkTimer -= Time.deltaTime;

        if (thinkTimer <= 0f)
        {
            Think();
            thinkTimer = thinkInterval;
        }
    }

    protected abstract void Think();
}