using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltAttackHeal : Ability
{
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
        GameObject ultInstance = BulletFactory.Instance.CreateSuperRegenAura(Handler.Owner);
        StartCoroutine("reload");
        reloader.shoot();
        OwnerManaSys.useMana(manaCost);
    }
    protected override bool InputPressed()
    {
        return PlayerInputRouter.Instance.SkillPressed;
    }
}
