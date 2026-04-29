using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseArmorAura : Ability
{
    private ArmorAura armorAura;
    protected override void AdditionalInit()
    {
        armorAura = BulletFactory.Instance.CreateArmorAura(Handler.Owner).GetComponent<ArmorAura>();
        (Handler.Owner as MainPlayerBehaviour).HasLeveledUp += armorAura.UpdateVals;
    }
    protected override AbilityInfo GetAbilityInfo()
    {
        return new AbilityInfo(20, 2, new List<AIUtils.AIState> { AIUtils.AIState.Attacking, AIUtils.AIState.CheckShoot, AIUtils.AIState.CheckDistSkills });
    }
    protected override void AbilityAction()
    {
    }
    protected override bool InputPressed()
    {
        return false;
    }
    protected override void AICheck()
    {
    }
}
