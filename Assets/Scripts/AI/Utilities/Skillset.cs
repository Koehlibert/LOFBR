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
                Ability shield = Handler.gameObject.AddComponent<UseShield>();
                Handler.AddAbility(shield, HUD.Instance.SkillReloader);
                break;
            case 4:
                Ability secondShooter = Handler.gameObject.AddComponent<ShootLeftBasic>();
                Handler.AddAbility(secondShooter, HUD.Instance.SecondaryReloader);
                UseArmorAura useArmorAura = Handler.gameObject.AddComponent<UseArmorAura>();
                Handler.AddAbility(useArmorAura);
                break;
            case 5:
                Ability mirrorImage = Handler.gameObject.AddComponent<MirrorImage>();
                Handler.AddAbility(mirrorImage, HUD.Instance.SuperReloader);
                break;
            case 6:
                Ability offside = Handler.gameObject.AddComponent<Offside>();
                Handler.AddAbility(offside, HUD.Instance.UltReloader);
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
                /* Ability shootDoublePass = Handler.gameObject.AddComponent<ShootDoublePass>();
                Handler.AddAbility(shootDoublePass, HUD.Instance.SecondaryReloader); */
                Ability possessDebug = Handler.gameObject.AddComponent<PossessDebug>();
                Handler.AddAbility(possessDebug, HUD.Instance.SecondaryReloader);
                break;
            case 2:
                
                break;
            case 3:
                Ability markForDeath = Handler.gameObject.AddComponent<MarkForDeath>();
                Handler.AddAbility(markForDeath, HUD.Instance.ThirdReloader);
                break;
            case 4:
                Ability buildWall = Handler.gameObject.AddComponent<BuildWall>();
                Handler.AddAbility(buildWall, HUD.Instance.AltReloader);
                break;
            case 5:
                Ability healAura = Handler.gameObject.AddComponent<ActivateRegenAura>();
                Handler.AddAbility(healAura, HUD.Instance.SuperReloader);
                break;
            case 6:
                Ability ultRes = Handler.gameObject.AddComponent<Resurrect>();
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
            case 3:
                //Taunt
                break;
            case 4:
                Ability manaDrain = Handler.gameObject.AddComponent<ManaDrain>();
                Handler.AddAbility(manaDrain, HUD.Instance.ThirdReloader);
                break;
            case 5:
                Ability immolate = Handler.gameObject.AddComponent<Immolate>();
                Handler.AddAbility(immolate, HUD.Instance.SuperReloader);
                break;
            case 6:
                Ability bladeFlurry = Handler.gameObject.AddComponent<BladeFlurry>();
                Handler.AddAbility(bladeFlurry, HUD.Instance.UltReloader);
                break;
            default:
                break;
        }
    }
}
public class SkillsetCrowdFavorite : Skillset
{
    public SkillsetCrowdFavorite(AIHandler handler)
    {
        Handler = handler;
        startingLife = 350;
        startingRegen = 18;
        regenDelay = 3.5f;
        startingArmor = 18;
        startingSpeed = 17.5f;
    }
    public override void LevelUnlock(int lvl)
    {
        switch (lvl)
        {
            case 1:
                Ability shooterPoison = Handler.gameObject.AddComponent<ShootPoison>();
                Handler.AddAbility(shooterPoison, HUD.Instance.PrimaryReloader);
                //Scare Enemies
                break;
            case 2:
                //Spawn protective Fans
                break;
            case 3:
                //Molotov
                break;
            case 4:
                Ability flip = Handler.gameObject.AddComponent<ShootFlip>();
                Handler.AddAbility(flip, HUD.Instance.ThirdReloader);
                //Convert
                break;
            case 5:
                //Convert
                break;
            case 6:
                //Flitzer
                break;
            default:
                break;
        }
    }
}