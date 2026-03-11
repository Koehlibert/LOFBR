using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltAttackHeal : Ability
{
    public GameObject ultBullet;
    protected override AbilityInfo GetAbilityInfo()
    {
        return new AbilityInfo(120, 15, new List<AIUtils.AIState> { AIUtils.AIState.Attacking, AIUtils.AIState.CheckShoot, AIUtils.AIState.CheckDistSkills });
    }
    private IEnumerator reload()
    {
        loaded = false;
        Instantiate(ultBullet, player.transform);
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
        GameObject ultInstance = Instantiate(ultBullet, player.transform.position + player.transform.forward * 2 + new Vector3(0f, 2f, 0f), player.transform.rotation);
        StartCoroutine("reload");
        reloader.shoot();
        player.manasys.useMana(manaCost);
    }
    protected override bool InputPressed()
    {
        return PlayerInputRouter.Instance.SkillPressed;
    }
}
