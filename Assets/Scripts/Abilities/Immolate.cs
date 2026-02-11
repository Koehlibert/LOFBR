using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Immolate : DamagingAbility
{
    public GameObject partSys;
    private GameObject fire;
    private bool isOnFire;
    private float manaDrain;
    new void Start()
    {
        base.Start();
        loaded = true;
        reloadtime = 8f;
        fire = FindAnyObjectByType<FireBehaviour>().gameObject;
        fire.SetActive(false);
        isOnFire = false;
    }
    void OnDisable()
    {
        Reset();
    }
    protected override void Update()
    {
        if (isOnFire)
        {
            if (player.manasys.checkCost(manaDrain * Time.deltaTime))
            {
                player.manasys.useMana(manaDrain * Time.deltaTime);
            }
            else
            {
                TurnOff();
            }
        }
        base.Update();
    }
    public new void Reset()
    {
        loaded = true;
        isOnFire = false;
        if (fire)
        {
            fire.SetActive(false);
        }
    }
    private void TurnOn()
    {
        reloader.shoot();
        player.manasys.useMana(manaCost);
        fire.SetActive(true);
        fire.GetComponent<Damage>().SetProperties(GetDamageValues());
        isOnFire = true;
    }
    private void TurnOff()
    {
        StartCoroutine("reload");
        fire.SetActive(false);
        isOnFire = false;
    }
    protected override void AbilityAction()
    {
        if (isOnFire)
        {
            Debug.Log("Fire Off :(");
            TurnOff();
        }
        else
        {
            Debug.Log("Fire!");
            TurnOn();
        }
    }
    protected override bool InputPressed()
    {
        return PlayerInputRouter.Instance.SkillPressedThisFrame;
    }
    protected override DamageInfo GetDamageValues()
    {
        return new DamageInfo(3.5f * player.levelsys.getLevel(), 0, player.Team, true, true);
    }

    public override void Checker()
    {
        throw new System.NotImplementedException();
    }
}
