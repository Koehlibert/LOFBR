using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UltAttackHeal : Ability
{
    private int HurtFriendliesToTrigger = 3;
    private float HealRadiusToCheck = 15;
    private InDistanceTracker inDistanceTracker;
    protected override void AdditionalInit()
    {
        if (!IsInteractive)
            inDistanceTracker = Handler.ClosestFinder.StartTrackingDist(HealRadiusToCheck, true, Handler.Owner.Team);
    }
    protected override AbilityInfo GetAbilityInfo()
    {
        return new AbilityInfo(120, 15, new List<AIUtils.AIState> { AIUtils.AIState.Attacking, AIUtils.AIState.CheckShoot, AIUtils.AIState.CheckDistSkills });
    }
    private IEnumerator reload()
    {
        loaded = false;
        yield return new WaitForSeconds(reloadtime);
        loaded = true;
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        reloader = HUD.Instance.GetReload(HUD.Instance.SkillReloader);
    }
    protected override void AbilityAction()
    {
        BulletFactory.Instance.CreateSuperRegenAura(Handler.Owner);
        StartCoroutine("Reload");
        base.AbilityAction();
    }
    protected override bool InputPressed()
    {
        return PlayerInputRouter.Instance.SkillPressed;
    }
    protected override void AICheck()
    {
        if(loaded && inDistanceTracker.GetOverCount(HurtFriendliesToTrigger))
            SetFinalAction();
    }
}
