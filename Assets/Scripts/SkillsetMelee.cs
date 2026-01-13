using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsetMelee : Skillset
{
    void OnEnable()
    {
        startingLife = 350;
        startingRegen = 7.5f;
        regenDelay = 2.5f;
        startingArmor = 35;
        startingSpeed = 16;
        primary = FindObjectOfType<Melee>();
        primary.manaCost = 8;
        primary.reloadtime = 1f;
        secondary = FindObjectOfType<Parry>();
        secondary.manaCost = 10;
        secondary.reloadtime = 1f;
        alternative = FindObjectOfType<Dash>();
        alternative.manaCost = 15;
        alternative.reloadtime = 1f;
        skill = FindObjectOfType<Immolate>();
        skill.manaCost = 20;
        skill.reloadtime = 8f;
        ultimate = FindObjectOfType<UltBladeFlurry>();
        ultimate.manaCost = 200f;
        ultimate.reloadtime = 24f;
        base.StartManually();
    }
    public override void BaseUnlock()
    {
        primary.enabled = true;
        primary.activate();
        meleeCol.SetActive(true);
        secondary.enabled = true;
        secondary.activate();
        parryCol.SetActive(true);
    }
    public override void LevelUnlock(int lvl)
    {
        switch (lvl)
        {
            case 2:
                alternative.enabled = true;
                alternative.activate();
                break;
            case 4:
                fire.SetActive(true);
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
