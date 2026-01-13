using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : Ability
{
    public GameObject shield;
    private MasterScript master;
    private float dashDistance = 10;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        loaded = true;
        master = FindObjectOfType<MasterScript>();
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
                if (x > 18)
                {
                    x = 18;
                }
                else if (x< -18)
                {
                    x = -18;
                }
                if (z > master.enemySpawn.getZPos() - 2)
                {
                    z = master.enemySpawn.getZPos() - 2;
                }
                else if (z< master.friendlySpawn.getZPos() + 2)
                {
                    z = master.friendlySpawn.getZPos() + 2;
                }
                Vector3 moveDir = new Vector3(x-player.transform.position.x,0,z-player.transform.position.z);
                StartCoroutine(player.LockMovement(0.2f));
                player.transform.Translate(moveDir,Space.World);
                StartCoroutine("reload");
                reloader.shoot();
                player.manasys.useMana(manaCost);
            }
        }
    }
}
