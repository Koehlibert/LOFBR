using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsetSupport : Skillset
{
    void OnEnable()
    {
        startingLife = 400;
        startingRegen = 4;
        regenDelay = 5;
        startingArmor = 8;
        startingSpeed = 14;
        primary = FindObjectOfType<ShootHeal>();
        primary.manaCost = 5;
        primary.reloadtime = .8f;
        secondary = FindObjectOfType<ShootPoison>();
        secondary.manaCost = 5;
        secondary.reloadtime = 1.2f;
        alternative = FindObjectOfType<ManaDrain>();
        alternative.manaCost = 15;
        alternative.reloadtime = 20f;
        skill = FindObjectOfType<UltAttackHeal>();
        skill.manaCost = 200;
        skill.reloadtime = 20f;
        ultimate = FindObjectOfType<UltRez>();
        ultimate.manaCost = 250;
        ultimate.reloadtime = 25f;
        base.StartManually();
    }
    public override void BaseUnlock()
    {
        primary.enabled = true;
        primary.activate();
        secondary.enabled = true;
        secondary.activate();
    }
    public override void LevelUnlock(int lvl)
    {
        switch (lvl)
        {
            case 2:
                aura.SetActive(true);
                break;
            case 3:
                manaLineRend.enabled = true;
                alternative.enabled = true;
                alternative.activate();
                break;
            case 4:
                skill.enabled = true;
                skill.activate();
                break;
            case 5:
                ultimate.enabled = true;
                ultimate.activate();
                break;
            default:
                break;
        }
    }
    void Update()
    {
        
    }
}
