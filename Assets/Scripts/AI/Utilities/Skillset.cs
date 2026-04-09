using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skillset
{
    protected float startingLife;
    protected float startingRegen;
    protected float startingArmor;
    protected float regenDelay;
    protected float startingSpeed;
    public abstract void LevelUnlock(int lvl);
    public AIHandler Handler;
    public (float hpval, float regenval, float delay, float armorval) GetHPVals()
    {
        return (startingLife, startingRegen, regenDelay, startingArmor);
    }
    public float GetSpeed()
    {
        return startingSpeed;
    }
}
public class SkillsetFighter : Skillset
{
    public SkillsetFighter(AIHandler handler)
    {
        Handler = handler;
        startingLife = 350;
        startingRegen = 7.5f;
        regenDelay = 4;
        startingArmor = 15;
        startingSpeed = 18;
    }
    public override void LevelUnlock(int lvl)
    {
        switch (lvl)
        {
            case 1:
                Ability shooter = Handler.gameObject.AddComponent<ShootRightBasic>();
                Handler.AddAbility(shooter, HUD.Instance.PrimaryReloader);
                break;
            case 2:
                Ability stomper = Handler.gameObject.AddComponent<Stomp>();
                Handler.AddAbility(stomper, HUD.Instance.AltReloader);
                break;
            case 3:
                Ability buildWall = Handler.gameObject.AddComponent<BuildWall>();
                Handler.AddAbility(buildWall, HUD.Instance.SkillReloader);
                /* Ability shield = Handler.gameObject.AddComponent<UseShield>();
                Handler.AddAbility(shield, HUD.Instance.SkillReloader); */
                break;
            case 4:
                Ability secondShooter = Handler.gameObject.AddComponent<ShootLeftBasic>();
                Handler.AddAbility(secondShooter, HUD.Instance.SecondaryReloader);
                break;
            case 5:
                Ability ultAttack = Handler.gameObject.AddComponent<UltAttack>();
                Handler.AddAbility(ultAttack, HUD.Instance.UltReloader);
                break;
            default:
                break;
        }
    }
}
public class SkillsetSupport : Skillset
{
    public SkillsetSupport(AIHandler handler)
    {
        Handler = handler;
        startingLife = 350;
        startingRegen = 7.5f;
        regenDelay = 2.5f;
        startingArmor = 8;
        startingSpeed = 15;
    }
    public override void LevelUnlock(int lvl)
    {
        switch (lvl)
        {
            case 1:
                Ability shooterHeal = Handler.gameObject.AddComponent<ShootHeal>();
                Handler.AddAbility(shooterHeal, HUD.Instance.PrimaryReloader);
                Ability markForDeath = Handler.gameObject.AddComponent<MarkForDeath>();
                Handler.AddAbility(markForDeath, HUD.Instance.SecondaryReloader);
                /* Ability shooterPoison = Handler.gameObject.AddComponent<ShootPoison>();
                Handler.AddAbility(shooterPoison, HUD.Instance.PrimaryReloader); */
                break;
            case 2:
                Ability armorAura = Handler.gameObject.AddComponent<UseArmorAura>();
                Handler.AddAbility(armorAura);
                break;
            case 3:
                Ability manaDrain = Handler.gameObject.AddComponent<ManaDrain>();
                Handler.AddAbility(manaDrain, HUD.Instance.AltReloader);
                break;
            case 4:
                Ability healAura = Handler.gameObject.AddComponent<UltAttackHeal>();
                Handler.AddAbility(healAura, HUD.Instance.SkillReloader);
                break;
            case 5:
                Ability ultRes = Handler.gameObject.AddComponent<UltRez>();
                Handler.AddAbility(ultRes, HUD.Instance.UltReloader);
                break;
            default:
                break;
        }
    }
}
public class SkillsetMelee : Skillset
{
    public SkillsetMelee(AIHandler handler)
    {
        Handler = handler;
        startingLife = 400;
        startingRegen = 25;
        regenDelay = 1.5f;
        startingArmor = 30;
        startingSpeed = 20f;
    }
    public override void LevelUnlock(int lvl)
    {
        switch (lvl)
        {
            case 1:
                Ability melee = Handler.gameObject.AddComponent<Melee>();
                Handler.AddAbility(melee, HUD.Instance.PrimaryReloader);
                Ability parry = Handler.gameObject.AddComponent<Parry>();
                Handler.AddAbility(parry, HUD.Instance.SecondaryReloader);
                break;
            case 2:
                Ability dash = Handler.gameObject.AddComponent<Dash>();
                Handler.AddAbility(dash, HUD.Instance.AltReloader);
                break;
            case 4:
                Ability immolate = Handler.gameObject.AddComponent<Immolate>();
                Handler.AddAbility(immolate, HUD.Instance.SkillReloader);
                break;
            case 5:
                Ability bladeFlurry = Handler.gameObject.AddComponent<UltBladeFlurry>();
                Handler.AddAbility(bladeFlurry, HUD.Instance.UltReloader);
                break;
            default:
                break;
        }
    }
}