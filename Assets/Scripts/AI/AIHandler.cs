using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Search;
using UnityEngine;

public abstract class AIHandler : MonoBehaviour
{
    protected List<Ability> Abilities;
    protected List<AIModule> AIModules;
    public DamageableEntity Owner { get; set; }
    public ClosestFinder ClosestFinder { get; }
    private AIUtils.AIState AIState;
    public AIUtils.HealthState HealthState { get; set; }
    public float LockAITimer;
    public MovementAI movementAI;
    private bool IsAILocked;
    protected HealthChecker healthChecker;
    protected DistanceHandler distanceHandler;
    public Action FinalAction;
    public Action FallBackAction;
    public Vector3 MovementDirection;
    public bool ForceMovement;
    public virtual void Init(DamageableEntity owner, List<Ability> abilities, List<AIModule> aIModules)
    {
        Owner = owner;
        AIModules = aIModules;
        Abilities = abilities;        
        healthChecker = Owner.gameObject.AddComponent<HealthChecker>();
        healthChecker.Init(0.7f, 0.3f);
        AIModules.Add(healthChecker);
        distanceHandler = Owner.gameObject.AddComponent<DistanceHandler>();
        AIModules.Add(distanceHandler);
        movementAI = Owner.gameObject.AddComponent<MovementAI>();
        Abilities.Add(movementAI);
        LockAITimer = 0f;
        FinalAction = null;
        FallBackAction = null;
        ForceMovement = false;
        MovementDirection = new Vector3();
    }
    private void Update()
    {
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
        movementAI?.HandleMovement();
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
    public IEnumerator SetForcemovement(float duration)
    {
        ForceMovement = true;
        yield return new WaitForSeconds(duration);
        ForceMovement = false;
    }
}