using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Damage))]
public class BulletBehaviourPossessing : BulletBehaviour
{
    public int NumberOfHits = 0;
    private Damage DamagingDamage;
    private Damage HealingDamage;
    public void Init(Damage damagingDamage, Damage healing, CharacterBehaviour owner, bool destroyOnHit, HumanBodyBones bone, float timer = 1.5f)
    {
        base.Init(owner, destroyOnHit, bone, timer);
        DamagingDamage = damagingDamage;
        HealingDamage = healing;
    }
    public void IncreaseCounter()
    {
        NumberOfHits++;
    }
    public Damage GetDamage()
    {
        return DamagingDamage;
    }
    public Damage GetHealing()
    {
        return HealingDamage;
    }
    public void HitAction(CharacterBehaviour hitCharacter)
    {
        if (hitCharacter.Team == Owner.Team)
        {
            HealingAction(hitCharacter);
        }
        else
        {
            //DamagingAction
        }
        IncreaseCounter();
    }
    public void HealingAction(CharacterBehaviour hitCharacter)
    {
        hitCharacter.hpsys.Heal(GetHealing());
        Owner.ToggleInteractive();
        Owner.animator.SetFloat("moveX", 0);
        Owner.animator.SetFloat("moveZ", 0);
        Owner.aIHandler.LockAI(Mathf.Infinity);
        Owner.aIHandler.movementAI.LockMovementAI();
        CameraController.Instance.SetNewTarget(hitCharacter.gameObject);
        hitCharacter.DeathEvent += Reset;
    }
    public void Reset()
    {
        Owner.ToggleInteractive();
        Owner.aIHandler.UnlockAI();
        Owner.aIHandler.movementAI.UnlockMovementAI();
        CameraController.Instance.SetTargetToDefault();
    }
}
