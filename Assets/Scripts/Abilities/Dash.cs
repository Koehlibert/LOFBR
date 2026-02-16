using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : Ability
{
    public GameObject shield;

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
    new void Start()
    {
        base.Start();
        loaded = true;
    }
        protected override bool InputPressed()
    {
        return PlayerInputRouter.Instance.AlternativePressed;
    }
}
