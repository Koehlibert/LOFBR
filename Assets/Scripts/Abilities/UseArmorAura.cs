using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseArmorAura : Ability
{
    private GameObject aura;
    protected override void AdditionalInit()
    {
        aura = BulletFactory.Instance.CreateArmorAura(Handler.Owner);
    }
    protected override AbilityInfo GetAbilityInfo()
    {
        return new AbilityInfo(20, 2, new List<AIUtils.AIState> { AIUtils.AIState.Attacking, AIUtils.AIState.CheckShoot, AIUtils.AIState.CheckDistSkills });
    }
    void OnDisable()
    {
        Reset();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        reloader = HUD.Instance.GetReload(HUD.Instance.AltReloader);
    }
    protected override void AbilityAction()
    {
    }
    protected override bool InputPressed()
    {
        return false;
    }
}
