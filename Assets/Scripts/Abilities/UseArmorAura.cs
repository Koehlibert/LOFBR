using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseArmorAura : Ability
{
    private GameObject aura;
    private bool armorActive;
    protected override void AdditionalInit()
    {
        aura = FindAnyObjectByType<ArmorAura>().gameObject;
        aura.SetActive(false);
        armorActive = false;
    }
    protected override AbilityInfo GetAbilityInfo()
    {
        return new AbilityInfo(20, 2, new List<AIUtils.AIState> { AIUtils.AIState.Attacking, AIUtils.AIState.CheckShoot, AIUtils.AIState.CheckDistSkills });
    }
    void OnDisable()
    {
        Reset();
    }
    public new void Reset()
    {
        loaded = true;
        armorActive = false;
        if (aura)
        {
            aura.SetActive(false);
        }
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        reloader = HUD.Instance.GetReload(HUD.Instance.AltReloader);
    }
    protected override void AbilityAction()
    {
        if (armorActive)
        {
            StartCoroutine("reload");
            aura.SetActive(false);
            armorActive = false;
        }
        else if ((loaded) && OwnerManaSys.checkCost(manaCost))
        {
            OwnerManaSys.useMana(manaCost);
            aura.SetActive(true);
            armorActive = true;
        }
    }
    protected override bool InputPressed()
    {
        return PlayerInputRouter.Instance.AlternativePressed;
    }
}
