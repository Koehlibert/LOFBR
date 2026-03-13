using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stomp : DamagingAbility
{
    private HumanBodyBones Bone = HumanBodyBones.LeftLowerLeg;
    public GameObject bullet;
    protected override void OnEnable()
    {
        base.OnEnable();
        reloader = HUD.Instance.GetReload(HUD.Instance.AltReloader);
    }
    private IEnumerator Reload()
    {
        loaded = false;
        yield return new WaitForSeconds(reloadtime);
        loaded = true;
    }
    protected override void AbilityAction()
    {
        StartCoroutine("Shootanim");
        StartCoroutine("Reload");
        if (IsInteractive)
        {
            reloader.shoot();
            reloader.shoot();
            OwnerManaSys.useMana(manaCost);
        }
    }
    protected override bool InputPressed()
    {
        return PlayerInputRouter.Instance.AlternativePressed;
    }
    protected override DamageInfo GetDamageValues()
    {
        return new DamageInfo(70 + (OwnerLevelSys.GetLevel() - 2) * 6, 0, Handler.Owner.Team, true, false);
    }
    private IEnumerator Shootanim()
    {
        StartCoroutine(Handler.movementAI.LockMovement(0.95f));
        Handler.Owner.animator.Play("Stomp", 0, 0f);
        yield return new WaitForSeconds(0.7f);
        GameObject wave = BulletFactory.Instance.CreateShockwave(Handler.Owner, false, Bone);
        wave.GetComponent<Damage>().SetProperties(GetDamageValues());
        //soundsource.Play();
    }
    protected override AbilityInfo GetAbilityInfo()
    {
        return new AbilityInfo(80, 5, new List<AIUtils.AIState> { AIUtils.AIState.Attacking, AIUtils.AIState.CheckShoot, AIUtils.AIState.CheckDistSkills });
    }
}
