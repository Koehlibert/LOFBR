using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIModule : MonoBehaviour
{
    protected bool IsActive { get; set; }
    protected bool IsInteractive { get; set; }
    protected AIHandler Handler { get; set; }
    public abstract void Checker();
    protected AIUtils.AIState ActiveState { get; set; }
    protected virtual void OnEnable()
    {
        Handler = GetComponent<AIHandler>();
        IsInteractive = Handler.Owner is MainPlayerBehaviour;
    }
    protected void SetFinalAction(Action action, Vector3 target, AIUtils.AIState aIState, float lockAITimer)
    {
        Handler.FinalAction = action;
        Handler.MovementDirection = target;
        Handler.SetAIState(aIState);
        Handler.LockAI(lockAITimer);
    }
    protected void Skip()
    {
    }
}