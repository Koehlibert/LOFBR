using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : Ability
{
    public GameObject shield;
    private float dashDistance = 10;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        loaded = true;
    }
    void Update()
    {
        if (Input.GetButtonDown("Alternative") && (loaded) && player.manasys.checkCost(manaCost))
        {
            Vector3 dir = player.movement.normalized;
            if (dir.magnitude > 0)
            {
                float x = player.transform.position.x+dir.x*dashDistance;
                float z = player.transform.position.z+dir.z*dashDistance;
                /* if (x > MasterScript.Instance.upperAreaLimitX)
                {
                    x = MasterScript.Instance.upperAreaLimitX;
                }
                else if (x< MasterScript.Instance.lowerAreaLimitX)
                {
                    x =  MasterScript.Instance.lowerAreaLimitX;
                }
                if (z > MasterScript.Instance.enemySpawn.getZPos() - 2)
                {
                    z = MasterScript.Instance.enemySpawn.getZPos() - 2;
                }
                else if (z< MasterScript.Instance.friendlySpawn.getZPos() + 2)
                {
                    z = MasterScript.Instance.friendlySpawn.getZPos() + 2;
                }
                Vector3 moveDir = new Vector3(x-player.transform.position.x,0,z-player.transform.position.z); */
                Vector3 moveDir = MasterScript.Instance.CorrectTarget(new Vector3 (x, 0, z));
                StartCoroutine(player.LockMovement(0.2f));
                player.transform.Translate(moveDir,Space.World);
                StartCoroutine("reload");
                reloader.shoot();
                player.manasys.useMana(manaCost);
            }
        }
    }
}
