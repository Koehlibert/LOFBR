using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ManaDrain : DamagingAbility
{
    private MainPlayerBehaviour enemy;
    private float DurationTime = 2.5f;
    private Vector3 offset = new Vector3(0, 2, 0);
    private GameObject ManaDrainer;
    protected override void AdditionalInit()
    {
        enemy = MasterScript.Instance.GetOpponentPlayer(Handler.Owner.Team);
    }
    protected override AbilityInfo GetAbilityInfo()
    {
        return new AbilityInfo(25, 10, new List<AIUtils.AIState> { AIUtils.AIState.Attacking, AIUtils.AIState.CheckShoot });
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        reloader = HUD.Instance.GetReload(HUD.Instance.AltReloader);
    }
    protected override void InteractiveCheck()
    {
        if (enemy.isActiveAndEnabled)
        {
            base.InteractiveCheck();
        }
    }
    private IEnumerator duration()
    {
        yield return new WaitForSeconds(DurationTime + OwnerLevelSys.GetLevel() * 0.8f);
        Destroy(ManaDrainer);
    }
    protected override void AbilityAction()
    {
        float distance = Vector3.Distance(Handler.Owner.transform.position, enemy.transform.position);
        if (distance <= 20)
        {
            base.AbilityAction();
            ManaDrainer = BulletFactory.Instance.CreateManaDrainer(Handler.Owner);
            StartCoroutine("Reload");
            StartCoroutine("duration");
        }
    }
    protected override bool InputPressed()
    {
        return PlayerInputRouter.Instance.AlternativePressed;
    }
    protected override DamageInfo GetDamageValues()
    {
        return new DamageInfo(0, 0, CombatUtils.Team.Player, true, true);
    }
}
