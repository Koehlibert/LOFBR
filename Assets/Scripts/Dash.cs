using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : Ability
{
    public GameObject shield;

    private float dashDistance = 10;

    public override string InputString => "Alternative";

    protected override void AbilityAction()
    {
        Vector3 dir = player.movement.normalized;
        if (dir.magnitude > 0)
        {
            float x = player.transform.position.x + dir.x * dashDistance;
            float z = player.transform.position.z + dir.z * dashDistance;
            Vector3 moveDir = MasterScript.Instance.CorrectTarget(new Vector3(x, 0, z));
            StartCoroutine(player.LockMovement(0.2f));
            player.transform.position = moveDir;
            StartCoroutine("reload");
            reloader.shoot();
            player.manasys.useMana(manaCost);
        }
    }

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        loaded = true;
    }
}
