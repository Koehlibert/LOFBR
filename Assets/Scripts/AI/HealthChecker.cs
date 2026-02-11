using System.Buffers;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.WSA;

public class HealthChecker : AIModule
{
    private float HealthyThreshold;
    private float DamagedThreshold;
    public void Init(float healthyThreshold, float damagedThreshold)
    {

        this.HealthyThreshold = healthyThreshold;
        this.DamagedThreshold = damagedThreshold;
    }
    public override void Checker()
    {
        throw new System.NotImplementedException();
    }
    void Update()
    {
        float hpPercent = Handler.Owner.GetHealth().healthDisplay();
        if (hpPercent == 1)
        {
            Handler.HealthState = AIUtils.HealthState.FullHP;
            return;
        }
        if (hpPercent > HealthyThreshold)
        {
            Handler.HealthState = AIUtils.HealthState.Healthy;
            return;
        }
        if (hpPercent > DamagedThreshold)
        {
            Handler.HealthState = AIUtils.HealthState.PartiallyDamaged;
            return;
        }
        else
        {
            Handler.HealthState = AIUtils.HealthState.Hurt;
            return;
        }
    }
}