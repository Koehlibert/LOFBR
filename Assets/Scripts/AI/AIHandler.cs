using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Search;
using UnityEngine;

public class AIHandler : MonoBehaviour
{
    protected List<Ability> Abilities;
    protected List<AIModule> AIModules;
    public DamageableEntity Owner { get; set; }
    public ClosestFinder ClosestFinder { get; set; }
    public AIUtils.AIState AIState;
    public AIUtils.HealthState HealthState { get; set; }
    public float LockAITimer;
    private MovementAI movementAI;
    private bool IsAILocked = false;
    protected HealthChecker healthChecker;
    protected DistanceHandler distanceHandler;
    private Action FinalAction;
    public Action FallBackAction;
    public GameObject closestEnemy;
    public GameObject closestEnemyNoTower;
    public float distanceToClosest;
    public bool IsInteractive;
    protected List<Ability> DisabledAbilities;
    internal GameObject closestHurtFriendly;

    public virtual void Init(DamageableEntity owner, List<Ability> abilities, List<AIModule> aIModules, float movementSpeed, bool caresAboutHealth = false)
    {
        Owner = owner;
        IsInteractive = Owner is PlayerController;
        ClosestFinder = new ClosestFinder(Owner.Team, Owner.gameObject);
        movementAI = Owner.gameObject.AddComponent<MovementAI>();
        movementAI.Init(IsInteractive, this, movementSpeed, caresAboutHealth);
        Abilities = abilities;
        foreach (Ability ability in Abilities)
        {
            ability.Init(IsInteractive, this, movementAI);
        }
        AIModules = aIModules;
        healthChecker = Owner.gameObject.AddComponent<HealthChecker>();
        healthChecker.Init(0.5f, 0.3f);
        AIModules.Add(healthChecker);
        distanceHandler = Owner.gameObject.AddComponent<DistanceHandler>();
        distanceHandler.Init(50, 30, 25, 12);
        AIModules.Add(distanceHandler);
        foreach (AIModule aIModule in AIModules)
        {
            aIModule.Init(IsInteractive, this);
        }
        LockAITimer = 0f;
        FinalAction = null;
        FallBackAction = null;
        DisabledAbilities = new List<Ability>();
    }
    public void AddAbility(Ability ability)
    {
        ability.Init(IsInteractive, this, movementAI);
        Abilities.Add(ability);
    }
    public void AddAbility(Ability ability, GameObject reloadObject)
    {
        ability.Init(IsInteractive, this, movementAI, reloadObject);
        Abilities.Add(ability);
    }
    private void OnDisable()
    {
        ReenableOtherAbilities();
    }
    private void Update()
    {
        if (PauseGame.Instance.isPaused)
            return;
        if (IsAILocked)
        {
            LockAITimer -= Time.deltaTime;
            IsAILocked = LockAITimer > 0;
        }
        distanceHandler.Checker();
        movementAI.SetMovementState(AIUtils.MovementState.IsMovingForward);
        if (IsAILocked)
        {
            return;
        }
        else if (AIState != AIUtils.AIState.MoveOnly)
        {
            foreach (Ability ability in Abilities)
            {
                if (!ability.enabled)
                    continue;
                ability.Checker();
                if (FinalAction != null)
                {
                    break;
                }
            }
            if (FinalAction == null)
            {
                
            }
        }
        movementAI.Checker();
        movementAI.HandleMovement();
        movementAI.HandleLook();
        if (FinalAction == null)
        {
            FinalAction = FallBackAction;
        }
        FinalAction?.Invoke();
        FinalAction = null;
    }
    public void SetAIState(AIUtils.AIState aIState)
    {
        if (!IsAILocked)
        {
            AIState = aIState;
        }
    }
    public void LockAI(float timer)
    {
        IsAILocked = true;
        LockAITimer = timer;
    }
    public void UnlockAI()
    {
        IsAILocked = false;
        LockAITimer = 0;
    }
    public IEnumerator DisableOtherAbilities(float duration, Ability abilityToKeep)
    {
        DisableOtherAbilities(abilityToKeep);
        yield return new WaitForSeconds(duration);
        ReenableOtherAbilities();
    }
    public void DisableOtherAbilities(Ability abilityToKeep)
    {
        foreach (Ability ability in Abilities)
        {
            if (ability != abilityToKeep && (!ability.ShouldStayActive))
            {
                ability.Deactivate();
                DisabledAbilities.Add(ability);
            }
        }
    }
    public void ReenableOtherAbilities()
    {
        foreach (Ability ability in DisabledAbilities)
        {
            ability.Activate();
        }
        DisabledAbilities.Clear();
    }
    public void SetFinalAction(Action action)
    {
        FinalAction = action;
    }
    public void LockMovementAI(float duration)
    {
        movementAI.LockMovementAI(duration);
    }
    public void LockMovement(float duration)
    {
        StartCoroutine(movementAI.LockMovement(duration));
    }
}