using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : Ability
{
    public GameObject shield;
    protected override AbilityInfo GetAbilityInfo()
    {
        return new AbilityInfo(15, 2.5f, new List<AIUtils.AIState> { AIUtils.AIState.Attacking, AIUtils.AIState.CheckShoot, AIUtils.AIState.CheckDistSkills, AIUtils.AIState.CheckGeneralSkills });
    }
    private float dashDistance = 10;
    protected override void AbilityAction()
    {
        Vector3 dir = player.aIHandler.MovementDirection.normalized;
        if (dir.magnitude > 0)
        {
            float x = player.transform.position.x + dir.x * dashDistance;
            float z = player.transform.position.z + dir.z * dashDistance;
            Vector3 moveDir = MasterScript.Instance.CorrectTarget(new Vector3(x, 0, z));
            StartCoroutine(player.aIHandler.movementAI.LockMovement(0.2f));
            player.transform.position = moveDir;
            StartCoroutine("reload");
            reloader.shoot();
            player.manasys.useMana(manaCost);
        }
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        reloader = HUD.Instance.GetReload(HUD.Instance.AltReloader);
    }
    protected override bool InputPressed()
    {
        return PlayerInputRouter.Instance.AlternativePressed;
    }
}
