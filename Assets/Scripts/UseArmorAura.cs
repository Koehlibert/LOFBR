using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseArmorAura : Ability
{
    private GameObject aura;
    private bool armorActive;
    new void Start()
    {
        base.Start();
        loaded = true;
        reloadtime = 2f;
        manaCost = 20;
        player = GetComponent<PlayerController>();
        aura = FindAnyObjectByType<ArmorAura>().gameObject;
        aura.SetActive(false);
        armorActive = false;
    }
    void OnDisable()
    {
        reset();
    }
    void Update()
    {
        if (Input.GetButtonDown("Alternative"))
        {
            if(armorActive)
            {
                StartCoroutine("reload");
                aura.SetActive(false);
                armorActive = false;
            }
            else if ((loaded)&&player.manasys.checkCost(manaCost))
            {
                //reloader.shoot();
                player.manasys.useMana(manaCost);
                aura.SetActive(true);
                armorActive = true;
            }  
        }
          
    }
    public new void reset()
    {
        loaded = true;
        armorActive = false;
        if(aura)
        {
            aura.SetActive(false);
        }
    }
}
