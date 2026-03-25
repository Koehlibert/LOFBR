using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : Ability
{
    public GameObject shield;
    private float dashDistance = 10;
    protected override AbilityInfo GetAbilityInfo()
    {
        return new AbilityInfo(15, 2.5f, new List<AIUtils.AIState> { AIUtils.AIState.Attacking, AIUtils.AIState.CheckShoot, AIUtils.AIState.CheckDistSkills, AIUtils.AIState.CheckGeneralSkills });
    }
    protected override void AbilityAction()
    {
        Vector3 dir = Handler.MovementDirection.normalized;
        if (dir.magnitude > 0)
        {
            float x = Handler.Owner.transform.position.x + dir.x * dashDistance;
            float z = Handler.Owner.transform.position.z + dir.z * dashDistance;
            Vector3 moveDir = MasterScript.Instance.CorrectTarget(new Vector3(x, 0, z));
            StartCoroutine(Handler.movementAI.LockMovement(0.2f));
            Handler.Owner.transform.position = moveDir;
            StartCoroutine("Reload");
            base.AbilityAction();
        }
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        if (IsInteractive)
        {
            reloader = HUD.Instance.GetReload(HUD.Instance.AltReloader);
        }
    }
    protected override bool InputPressed()
    {
        return PlayerInputRouter.Instance.AlternativePressed;
    }
}
