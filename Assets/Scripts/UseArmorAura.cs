using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseArmorAura : Ability
{
    private GameObject aura;
    private bool armorActive;

    public override string InputString => "Alternative";

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
}
