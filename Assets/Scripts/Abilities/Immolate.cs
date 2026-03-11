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
        fire = FindAnyObjectByType<FireBehaviour>().gameObject;
        fire.SetActive(false);
        isOnFire = false;
    }
    protected override AbilityInfo GetAbilityInfo()
    {
        return new AbilityInfo(15, 8, new List<AIUtils.AIState> { AIUtils.AIState.Attacking });
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
            if (player.manasys.checkCost(manaDrain * Time.deltaTime))
            {
                player.manasys.useMana(manaDrain * Time.deltaTime);
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
        reloader.shoot();
        player.manasys.useMana(manaCost);
        fire.SetActive(true);
        fire.GetComponent<Damage>().SetProperties(GetDamageValues());
        isOnFire = true;
    }
    private void TurnOff()
    {
        StartCoroutine("reload");
        fire.SetActive(false);
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
        return PlayerInputRouter.Instance.SkillPressedThisFrame;
    }
    protected override DamageInfo GetDamageValues()
    {
        return new DamageInfo(3.5f * player.Levelsys.GetLevel(), 0, player.Team, true, true);
    }

    public override void Checker()
    {
        throw new System.NotImplementedException();
    }
}
