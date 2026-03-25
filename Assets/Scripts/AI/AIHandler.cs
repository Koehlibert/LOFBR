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
    public MovementAI movementAI;
    private bool IsAILocked;
    protected HealthChecker healthChecker;
    protected DistanceHandler distanceHandler;
    public Action FinalAction;
    public Action FallBackAction;
    public Vector3 MovementDirection;
    public Vector3 LookDirection;
    public bool ForceMovement;
    public GameObject closestEnemy;
    public GameObject closestEnemyNoTower;
    public float distanceToClosest;
    public bool IsInteractive;
    protected List<Ability> DisabledAbilities;
    public virtual void Init(DamageableEntity owner, List<Ability> abilities, List<AIModule> aIModules, float movementSpeed, bool caresAboutHealth = false)
    {
        Owner = owner;
        IsInteractive = Owner is PlayerController;
        ClosestFinder = new ClosestFinder(Owner.Team, Owner.gameObject);
        Abilities = abilities;
        foreach (Ability ability in Abilities)
        {
            ability.Init(IsInteractive, this);
        }
        AIModules = aIModules;
        healthChecker = Owner.gameObject.AddComponent<HealthChecker>();
        healthChecker.Init(0.7f, 0.3f);
        AIModules.Add(healthChecker);
        distanceHandler = Owner.gameObject.AddComponent<DistanceHandler>();
        distanceHandler.Init(50, 30, 25, 12);
        AIModules.Add(distanceHandler);
        movementAI = Owner.gameObject.AddComponent<MovementAI>();
        movementAI.Init(IsInteractive, this, movementSpeed, caresAboutHealth);
        foreach (AIModule aIModule in AIModules)
        {
            aIModule.Init(IsInteractive, this);
        }
        LockAITimer = 0f;
        FinalAction = null;
        FallBackAction = null;
        ForceMovement = false;
        MovementDirection = new Vector3();
        LookDirection = new Vector3();
        DisabledAbilities = new List<Ability>();
    }
    public void AddAbility(Ability ability)
    {
        ability.Init(IsInteractive, this);
        Abilities.Add(ability);
    }
    public void AddAbility(Ability ability, GameObject reloadObject)
    {
        ability.Init(IsInteractive, this, reloadObject);
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
        movementAI.MovementState = AIUtils.MovementState.IsMovingForward;
        if (AIState != AIUtils.AIState.MoveOnly)
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
    public void SetEvenLookDirection(Vector3 direction)
    {
        direction.y = 0;
        LookDirection = direction;
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
    public IEnumerator SetForcemovement(float duration)
    {
        ForceMovement = true;
        yield return new WaitForSeconds(duration);
        ForceMovement = false;
    }
    public void DisableOtherAbilities(Ability abilityToKeep)
    {
        foreach (Ability ability in Abilities)
        {
            if (ability != abilityToKeep)
            {
                ability.enabled = false;
                DisabledAbilities.Add(ability);
            }
        }
    }
    public void ReenableOtherAbilities()
    {
        foreach (Ability ability in DisabledAbilities)
        {
            ability.enabled = true;
        }
        DisabledAbilities.Clear();
    }
}