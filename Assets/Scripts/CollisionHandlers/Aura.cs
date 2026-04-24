using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Interfaces;
using UnityEngine;
using UnityEngine.TextCore.Text;

public abstract class Aura : CollisionHandler
{
    protected List<CharacterBehaviour> AffectedCharacters = new();
    protected Dictionary<CharacterBehaviour, StatusEffect> activeEffects = new();
    public override void Init(DamageableEntity owner)
    {
        Owner = owner;
        Owner.DeathEvent += SelfDestruct;
        base.Init(owner);
    }
    void Update()
    {
        this.transform.SetPositionAndRotation(Owner.transform.position + new Vector3(0f,2f,0f), 
                                              Owner.transform.rotation);
        AffectedCharacters.RemoveAll(item => item == null);
    }
    protected override void HandleEnduringDamage(Collider collider)
    {
    }
    protected override void HandleDamageCollision(Collider collider)
    {
        bool isValid = CheckIfValidColliderToAdd(collider);
        if (isValid)
        {
            ApplyStatus(collider);
        }
    }
    void OnTriggerExit(Collider collider)
    {
        bool isValid = CheckIfValidColliderToRemove(collider);
        if (isValid)
        {
            UnApplyStatus(collider);
        }
    }
    void OnDestroy()
    {
        AffectedCharacters.RemoveAll(item => item == null);
        while (AffectedCharacters.Count > 0)
        {
            UnApplyStatus(AffectedCharacters[0]);
        }
    }
    protected virtual void ApplyStatus(Collider collider)
    {
        ApplyStatus(collider.gameObject.GetComponentInParent<CharacterBehaviour>());
    }
    protected virtual void ApplyStatus(CharacterBehaviour characterBehaviour)
    {
        StatusEffect instance = CreateStatusEffect(characterBehaviour);
        characterBehaviour.AddStatusEffect(instance);
        activeEffects[characterBehaviour] = instance;
        AffectedCharacters.Add(characterBehaviour);
    }
    protected void UnApplyStatus(Collider collider)
    {
        UnApplyStatus(collider.gameObject.GetComponentInParent<CharacterBehaviour>());
    }
    protected void UnApplyStatus(CharacterBehaviour characterBehaviour)
    {
        if (!activeEffects.TryGetValue(characterBehaviour, out var effect)) return;
        characterBehaviour.RemoveStatusEffect(effect);
        AffectedCharacters.Remove(characterBehaviour);
    }
    public virtual void UpdateVals()
    {
        foreach(KeyValuePair<CharacterBehaviour, StatusEffect> entry in activeEffects)
        {
            HandleUpdate(entry);
        }
    }
    protected abstract void HandleUpdate(KeyValuePair<CharacterBehaviour, StatusEffect> entry);
    protected bool CheckIfValidColliderToAdd(Collider collider)
    {
        CharacterBehaviour tmp = collider.gameObject.GetComponentInParent<CharacterBehaviour>();
        if (tmp == null)
            return false;
        if (!AdditionalCheckToAdd(tmp))
            return false;
        return !AffectedCharacters.Contains(tmp);
    }
    protected bool CheckIfValidColliderToRemove(Collider collider)
    {
        CharacterBehaviour tmp = collider.gameObject.GetComponentInParent<CharacterBehaviour>();
        if (!AdditionalCheckToRemove(tmp))
            return false;
        return AffectedCharacters.Contains(tmp);
    }
    protected virtual bool AdditionalCheckToAdd(CharacterBehaviour characterBehaviour)
    {
        return true;
    }
    protected virtual bool AdditionalCheckToRemove(CharacterBehaviour characterBehaviour)
    {
        return true;
    }
    protected abstract StatusEffect CreateStatusEffect(CharacterBehaviour characterBehaviour);
    protected void SelfDestruct()
    {
        Destroy(this.gameObject);
    }
}
