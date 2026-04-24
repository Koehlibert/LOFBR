using UnityEngine;

public abstract class StatusEffectTimer : StatusEffect
{
    private float timer;
    private void Init()
    {
        Destroy(this, timer);
    }
}