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
        Reset();
    }
    public new void Reset()
    {
        loaded = true;
        armorActive = false;
        if (aura)
        {
            aura.SetActive(false);
        }
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        reloader = HUD.Instance.GetReload(HUD.Instance.AltReloader);
    }
    protected override void AbilityAction()
    {
        if (armorActive)
        {
            StartCoroutine("reload");
            aura.SetActive(false);
            armorActive = false;
        }
        else if ((loaded) && player.manasys.checkCost(manaCost))
        {
            player.manasys.useMana(manaCost);
            aura.SetActive(true);
            armorActive = true;
        }
    }
    protected override bool InputPressed()
    {
        return PlayerInputRouter.Instance.AlternativePressed;
    }
}
