using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Immolate : Ability
{
    public GameObject partSys;
    //public ReloadUlt reloader
    private GameObject fire;
    private bool isOnFire;
    private float manaDrain;
    new void Start()
    {
        base.Start();
        loaded = true;
        reloadtime = 8f;
        fire = FindObjectOfType<FireBehaviour>().gameObject;
        fire.SetActive(false);
        isOnFire = false;
    }
    void OnDisable()
    {
        reset();
    }
    void Update()
    {
        if (isOnFire)
        {
            if(player.manasys.checkCost(manaDrain * Time.deltaTime))
            {
                player.manasys.useMana(manaDrain * Time.deltaTime);
            }
            else
            {
                TurnOff();
            }
        }
        if (Input.GetButtonDown("Skill"))
        {
            if(isOnFire)
            {
                TurnOff();
            }
            else if ((loaded)&&player.manasys.checkCost(manaCost))
            {
                TurnOn();
            }  
        }  
    }
    public new void  reset()
    {
        loaded = true;
        isOnFire = false;
        if(fire)
        {
            fire.SetActive(false);
        }
    }
    private void TurnOn()
    {
        reloader.shoot();
        player.manasys.useMana(manaCost);
        fire.SetActive(true);
        fire.GetComponent<Damage>().SetDamage(3.5f * player.levelsys.getLevel(), 0);
        isOnFire = true;
    }
    private void TurnOff()
    {
        StartCoroutine("reload");
        fire.SetActive(false);
        isOnFire = false;
    }
}
