using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Reload : MonoBehaviour
{
    public Image Reloadcircle;
    public Image ReloadParent;
    public RawImage manaCostMet;
    public RawImage Icon;
    private Ability ability;
    private float reloadtime;
    public bool reloading;
    public float timer;
    private PlayerController player;
    private Mana playerMana;
    void Start()
    {
        this.ReloadParent = GetComponent<Image>(); 
        Reloadcircle.fillAmount = 0;
        reloading = false;
    }
    void OnDisable()
    {
        reloading = false;
        Reloadcircle.enabled = false;
        ReloadParent.enabled = false;
        manaCostMet.enabled = false;
    }
    void Update()
    {
        if (reloading)
        {
            timer = timer - Time.deltaTime;
            Reloadcircle.fillAmount = (timer/reloadtime);
            if (timer <= 0)
            {
                reloading = false;
            }
        }
        if(player.isActiveAndEnabled)
        {
            setManaMet(playerMana.checkCost(ability.manaCost));
        }
    }
    public void Activate()
    {
        Reloadcircle.enabled = true;
        ReloadParent.enabled = true;
        manaCostMet.enabled = true;
        this.enabled = true;
    }
    public void setManaMet(bool val)
    {
        manaCostMet.enabled = val;
    }
    public void shoot()
    {
        if(!reloading)
        {
            timer = reloadtime;
            reloading = true;
        }
    }
    public void setAbility(Ability val)
    {
        ability = val;
        reloadtime = ability.reloadtime;
    }
    public void setPlayer(PlayerController playerval, Mana manasysval)
    {
        player = playerval;
        playerMana = manasysval;
    }
}
