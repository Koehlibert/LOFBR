using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Search;
using UnityEngine;

public abstract class AIHandler : MonoBehaviour
{
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
    public Vector3 MovementTarget;
    private void Onable()
    {
        healthChecker = Owner.gameObject.AddComponent<HealthChecker>();
        healthChecker.Init(0.7f, 0.3f);
        AIModules.Add(healthChecker);
        distanceHandler = Owner.gameObject.AddComponent<DistanceHandler>();
        AIModules.Add(distanceHandler);
        LockAITimer = 0f;
        FinalAction = null;
        MovementTarget = new Vector3();
    }
    public void Init(List<AIModule> aIModules)
    {
        foreach (AIModule aIModule in aIModules)
        {
            AIModules.Add(aIModule);
        }
    }
    private void Update()
    {
        if (IsAILocked)
        {
            LockAITimer -= Time.deltaTime;
            IsAILocked = LockAITimer > 0;
        }
        foreach (AIModule aIModule in AIModules)
        {
            aIModule.Checker();
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