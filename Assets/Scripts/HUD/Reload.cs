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
    void Start()
    {
        player = MasterScript.Instance.player;
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
            timer -= Time.deltaTime;
            Reloadcircle.fillAmount = timer/reloadtime;
            if (timer <= 0)
            {
                reloading = false;
            }
        }
        if(player.isActiveAndEnabled)
        {
            setManaMet(player.manasys.checkCost(ability.manaCost));
        }
    }
    public void Activate()
    {
        this.enabled = true;
        Reloadcircle.enabled = true;
        ReloadParent.enabled = true;
        manaCostMet.enabled = true;
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
    public void SetAbility(Ability val)
    {
        ability = val;
        reloadtime = ability.reloadtime;
    }
}
