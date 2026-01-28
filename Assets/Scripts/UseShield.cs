using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseShield : Ability
{
    public GameObject shield;
    private GameObject shieldInstance;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        loaded = true;
    }
    void Update()
    {
        if (Input.GetButtonDown("Skill")&&(loaded)&&player.manasys.checkCost(manaCost))
        {
            shieldInstance = Instantiate(shield, player.transform.position + new Vector3(0f,2f,0f), player.transform.rotation);
            shieldInstance.GetComponent<Shield>().SetPlayer(FindAnyObjectByType<PlayerController>());
            StartCoroutine("reload");
            StartCoroutine("destroyShield");
            reloader.shoot();
            player.manasys.useMana(manaCost);
        }
    }
    private IEnumerator destroyShield()
    {
        player.GetHealth().AddArmor(100);
        yield return new WaitForSeconds(1.5f);
        player.GetHealth().AddArmor(-100);
        GameObject.Destroy(shieldInstance);
    }
}
