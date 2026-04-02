using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Immolate : DamagingAbility
{
    public GameObject partSys;
    private GameObject fire;
    private bool isOnFire;
    private float manaDrain;
    protected int NEnemiesToTrigger = 2;
    protected float DistanceToCheck = 8;
    private InDistanceTracker inDistanceTracker;
    protected override void AdditionalInit()
    {
        isOnFire = false;
        if(!IsInteractive)
            inDistanceTracker = Handler.ClosestFinder.StartTrackingDist(DistanceToCheck, true);
    }
    protected override AbilityInfo GetAbilityInfo()
    {
        return new AbilityInfo(15, 8, new List<AIUtils.AIState> { AIUtils.AIState.Attacking }, true);
    }
    void OnDisable()
    {
        Reset();
    }
    protected override void InteractiveCheck()
    {
        if (isOnFire)
        {
            if (OwnerManaSys.checkCost(manaDrain * Time.deltaTime))
            {
                OwnerManaSys.useMana(manaDrain * Time.deltaTime);
            }
            else
            {
                TurnOff();
            }
        }
        base.InteractiveCheck();
    }
    public new void Reset()
    {
        loaded = true;
        isOnFire = false;
        if (fire)
        {
            fire.SetActive(false);
        }
    }
    private void TurnOn()
    {
        reloader.Shoot();
        OwnerManaSys.useMana(manaCost);
        fire = BulletFactory.Instance.CreateFire(Handler.Owner);
        fire.GetComponent<Damage>().SetProperties(GetDamageValues());
        isOnFire = true;
    }
    private void TurnOff()
    {
        StartCoroutine("Reload");
        Destroy(fire);
        isOnFire = false;
    }
    protected override void AbilityAction()
    {
        if (isOnFire)
        {
            TurnOff();
        }
        else
        {
            TurnOn();
        }
    }
    protected override bool InputPressed()
    {
        return PlayerInputRouter.Instance.SkillToggledThisFrame;
    }
    protected override DamageInfo GetDamageValues()
    {
        return new DamageInfo(3.5f * OwnerLevelSys.GetLevel(), 0, Handler.Owner.Team, true, true);
    }
    protected override void AICheck()
    {
        if (loaded && inDistanceTracker.GetOverCount(NEnemiesToTrigger))
        {
            SetFinalAction();
        }
    }
}
