using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIModule : MonoBehaviour
{
    protected bool IsActive { get; set; }
    public bool IsInteractive;
    protected AIHandler Handler { get; set; }
    public abstract void Checker();
    protected AIUtils.AIState ActiveState { get; set; }
    public virtual void Init(bool isInteractive, AIHandler aIHandler)
    {
        Handler = aIHandler;
        IsInteractive = isInteractive;
    }
}