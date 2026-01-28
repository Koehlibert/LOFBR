using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltAttack : Ability
{
    // Start is called before the first frame update
    public GameObject bullet;
    new void Start()
    {
        base.Start();
        loaded = true;
    }
    void Update()
    {
        if (Input.GetButtonDown("Alternative")&&(loaded)&&player.manasys.checkCost(manaCost))
        {
            GameObject wave = Instantiate(bullet, player.transform.position + new Vector3(0f,-0.4f,0f), player.transform.rotation);
            wave.GetComponent<Damage>().SetProperties(70+(player.levelsys.getLevel()-2)*6,0,player.Team,false, true);
            StartCoroutine("reload");
            reloader.shoot();
            player.manasys.useMana(manaCost);
        }
    }
    private IEnumerator reload()
    {
        loaded = false;
        yield return new WaitForSeconds(reloadtime);
        loaded = true;
    }

}
