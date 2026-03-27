using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Immolate : DamagingAbility
{
    public GameObject partSys;
    private GameObject fire;
    private bool isOnFire;
    private float manaDrain;
    protected override void AdditionalInit()
    {
        isOnFire = false;
    }
    protected override AbilityInfo GetAbilityInfo()
    {
        return new AbilityInfo(15, 8, new List<AIUtils.AIState> { AIUtils.AIState.Attacking }, true);
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        reloader = HUD.Instance.GetReload(HUD.Instance.SkillReloader);
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
    }
}
