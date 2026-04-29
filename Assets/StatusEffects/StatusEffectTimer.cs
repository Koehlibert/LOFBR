using UnityEngine;

public abstract class StatusEffectTimer : StatusEffect
{
    private float Timer;
    public void Init(float timer)
    {
        Timer = timer;
    }
    public void StartTimer()
    {
        Destroy(this, Timer);
    }
}