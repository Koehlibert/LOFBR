using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltAttack : Ability
{
    public GameObject ultBullet;
    new void Start()
    {
        base.Start();
        loaded = true;
        reloadtime = 15f;
        manaCost = 250;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Ult")&&(loaded)&&player.manasys.checkCost(manaCost))
        {
            GameObject ultInstance = Instantiate(ultBullet, player.transform.position + player.transform.forward*2 + new Vector3(0f,2f,0f), player.transform.rotation);
            ultInstance.GetComponent<Damage>().SetProperties(50+(player.levelsys.getLevel()-5)*4.5f, 0, CombatUtils.Team.Player, false, true, true);
            StartCoroutine("reload");
            reloader.shoot();
            player.manasys.useMana(manaCost);
        }
    }
}
