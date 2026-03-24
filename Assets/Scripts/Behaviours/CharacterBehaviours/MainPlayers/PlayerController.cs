using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerController : MainPlayerBehaviour
{
    public override void Init()
    {
        ClassID = PlayerPrefs.GetInt("classID");
        base.Init(ClassID);
        EnableDamageFlash();
    }
    public override CombatUtils.Team Team => CombatUtils.Team.Player;
    void FixedUpdate()
    {
        StackingHandler.PushAwayFromNearbyObjects(this.gameObject);
        if (PlayerInputRouter.Instance.CheatedPressed)
        {
            hpsys.AddArmor(1000);
            Levelsys.GainExp(100);
            EnemyPlayer.Levelsys.GainExp(100);
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
    public void OnHealXP()
    {
        Levelsys.GainExp(5);
    }
}
