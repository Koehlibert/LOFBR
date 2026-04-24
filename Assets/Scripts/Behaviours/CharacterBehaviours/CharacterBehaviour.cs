using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Unity.Services.Analytics;
using UnityEngine.Animations;
using NUnit.Framework.Interfaces;

public abstract class CharacterBehaviour : DamageableEntity
{
    protected Renderer rend;
    public AIHandler aIHandler;
    protected MainPlayerBehaviour EnemyPlayer;
    private List<StatusEffect> StatusEffects = new();
    public override void Init()
    {
        base.Init();
        EnemyPlayer = CharacterTracker.Instance.GetOpponentPlayer(Team);
        rend = GetComponentInChildren<SkinnedMeshRenderer>();
        if (rend != null)
        {
            rend.material = Team == CombatUtils.Team.Player
                ? MaterialLibrary.Instance.playerMaterial
                : MaterialLibrary.Instance.enemyMaterial;
        }
        CreateAIHandler();
        InitializeAIHandler();
        InitializeHPSys();
    }
    protected virtual void FixedUpdate()
    {
        StackingHandler.PushAwayFromNearbyObjects(this.gameObject);
    }
    public void AddStatusEffect(StatusEffect statusEffect)
    {
        if (StatusEffects.Contains(statusEffect) && !statusEffect.CanStack)
            return;
        StatusEffects.Add(statusEffect);
        statusEffect.ActivateAction(this);
    }
    public void RemoveStatusEffect(StatusEffect statusEffect)
    {
        if (StatusEffects.Contains(statusEffect))
        {
            StatusEffects.Remove(statusEffect);
            statusEffect.DeactivateAction(this);
            Destroy(statusEffect);
        }
    }
    protected virtual void InitializeAIHandler()
    {
        aIHandler.Init(this, new List<Ability>(), new List<AIModule>(), 0, false);
    }
    protected void CreateAIHandler()
    {
        aIHandler = gameObject.AddComponent<AIHandler>();
    }
    protected virtual void InitializeHPSys()
    {
        hpsys.Initialize(100, 0, 0, 0);
    }
    public void StartGetPushed()
    {
        aIHandler.LockAI(Mathf.Infinity);
        aIHandler.transform.LookAt(MasterScript.Instance.GetOpponentBase(aIHandler.Owner.Team).transform);
        animator.SetBool("Pushed", true);
    }
    public void StopGetPushed()
    {
        aIHandler.UnlockAI();
        animator.SetBool("Pushed", false);
    }
}