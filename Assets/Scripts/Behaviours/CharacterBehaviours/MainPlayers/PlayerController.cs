using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerController : MainPlayerBehaviour
{
    public override void Init()
    {
        this.Team = CombatUtils.Team.Player;
        ClassID = PlayerPrefs.GetInt("classID");
        base.Init(ClassID);
        EnableDamageFlash();
        healthbar = HUD.Instance.PlayerHealthImage;
        healthbarOutline = HUD.Instance.PlayerHealthOutline;
    }
    void FixedUpdate()
    {
        StackingHandler.PushAwayFromNearbyObjects(this.gameObject);
        if (PlayerInputRouter.Instance.CheatedPressed)
        {
            hpsys.AddArmor(1000);
            Levelsys.GainExp(100);
            //EnemyPlayer.Levelsys.GainExp(100);
        }
    }
    private void OnTakeDamage()
    {
        HUD.Instance.SetDamageImage(1 - hpsys.healthDisplay());
        AudioManager.Instance.PlayerHurt();
    }
    public void DisableDamageFlash()
    {
        CollisionHandler.OnHitCallback -= OnTakeDamage;
    }
    public void EnableDamageFlash()
    {
        CollisionHandler.OnHitCallback += OnTakeDamage;
    }
}
