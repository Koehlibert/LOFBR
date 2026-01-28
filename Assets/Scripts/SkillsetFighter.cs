using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsetFighter : Skillset
{
    void OnEnable()
    {
        startingLife = 300;
        startingRegen = 3;
        regenDelay = 4;
        startingArmor = 5;
        startingSpeed = 12;
        primary = FindAnyObjectByType<ShootRight>();
        primary.manaCost = 5;
        primary.reloadtime = 1.5f;
        secondary = FindAnyObjectByType<ShootLeft>();
        secondary.manaCost = 5;
        secondary.reloadtime = 1.5f;
        alternative = FindAnyObjectByType<AltAttack>();
        alternative.manaCost = 75;
        alternative.reloadtime = 6f;
        skill = FindAnyObjectByType<UseShield>();
        skill.manaCost = 120;
        skill.reloadtime = 6f;
        ultimate = FindAnyObjectByType<UltAttack>();
        ultimate.manaCost = 250;
        ultimate.reloadtime = 15f;
        base.StartManually();
    }
    public override void BaseUnlock()
    {
        primary.enabled = true;
        primary.activate();
    }
    public override void LevelUnlock(int lvl)
    {
        switch (lvl)
        {
            case 2:
                alternative.enabled = true;
                alternative.activate();
                break;
            case 3:
                skill.enabled = true;
                skill.activate();
                break;
            case 4:
                secondary.enabled = true;
                secondary.activate();
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
