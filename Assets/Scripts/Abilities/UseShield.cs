using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseShield : Ability
{
    public GameObject shield;
    private GameObject shieldInstance;
    protected override AbilityInfo GetAbilityInfo()
    {
        return new AbilityInfo(50, 12, new List<AIUtils.AIState> { AIUtils.AIState.Attacking, AIUtils.AIState.CheckShoot });
    }
    private IEnumerator DestroyShield()
    {
        player.GetHealth().AddArmor(100);
        player.DisableDamageFlash();
        yield return new WaitForSeconds(1.5f);
        player.GetHealth().AddArmor(-100);
        player.EnableDamageFlash();
        GameObject.Destroy(shieldInstance);
    }

    protected override void AbilityAction()
    {
        shieldInstance = BulletFactory.Instance.CreateShield(Handler.Owner);
        StartCoroutine("reload");
        StartCoroutine("DestroyShield");
        reloader.shoot();
        player.manasys.useMana(manaCost);
    }
    protected override bool InputPressed()
    {
        return PlayerInputRouter.Instance.SkillPressed;
    }
}
