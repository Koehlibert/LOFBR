using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Search;
using UnityEngine;

public abstract class AIHandler : MonoBehaviour
{
    protected List<Ability> Abilities;
    protected List<AIModule> AIModules;
    public DamageableEntity Owner { get; }
    public ClosestFinder ClosestFinder { get; }
    private AIUtils.AIState AIState;
    public AIUtils.HealthState HealthState { get; set; }
    public float LockAITimer;
    private MovementAI movementAI;
    private bool IsAILocked;
    private HealthChecker healthChecker;
    private DistanceHandler distanceHandler;
    public Action FinalAction;
    public Action FallBackAction;
    public Vector3 MovementDirection;
    public bool ForceMovement;
    private void Onable()
    {
        healthChecker = Owner.gameObject.AddComponent<HealthChecker>();
        healthChecker.Init(0.7f, 0.3f);
        AIModules.Add(healthChecker);
        distanceHandler = Owner.gameObject.AddComponent<DistanceHandler>();
        AIModules.Add(distanceHandler);
        LockAITimer = 0f;
        FinalAction = null;
        FallBackAction = null;
        MovementDirection = new Vector3();
    }
    public void Init(List<Ability> abilities)
    {
        foreach (Ability ability in abilities)
        {
            Abilities.Add(ability);
        }
    }
    private void Update()
    {
        movementAI.Checker();
        if (IsAILocked)
        {
            LockAITimer -= Time.deltaTime;
            IsAILocked = LockAITimer > 0;
        }
        foreach (Ability ability in Abilities)
        {
            ability.Checker();
            if (FinalAction != null)
            {
                break;
            }
        }
        movementAI.HandleMovement();
        FinalAction?.Invoke();
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
}