using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.TextCore.Text;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class BulletBehaviourPossessing : BulletBehaviour
{
    public int NumberOfHits = 0;
    private Damage DamagingDamage;
    private Damage HealingDamage;
    public void Init(DamageInfo damagingInfo, DamageInfo healingInfo, CharacterBehaviour owner, bool destroyOnHit, HumanBodyBones bone, float timer = 1.5f)
    {
        base.Init(owner, destroyOnHit, bone, timer);
        DamagingDamage = this.gameObject.AddComponent<Damage>();
        DamagingDamage.SetProperties(damagingInfo);
        HealingDamage = this.gameObject.AddComponent<Damage>();
        HealingDamage.SetProperties(healingInfo);
    }
    public void IncreaseCounter()
    {
        NumberOfHits++;
    }
    public void HitAction(DamageableEntity hitCharacter)
    {
        if (hitCharacter == Owner)
        {
            return;
        }
        if (hitCharacter is CharacterBehaviour)
        {
            if (hitCharacter.Team == Owner.Team)
            {
                HealingAction(hitCharacter);
            }
            else
            {
                DamagingAction(hitCharacter);
            }
            IncreaseCounter();
        }
    }
    private const int MaxPossessionChainLength = 3;
    private void DamagingAction(DamageableEntity hitCharacter)
    {
        damage = DamagingDamage;
        Owner.GetComponent<ShootDoublePass>()?.EndPossession();
    }
    public Damage GetActiveDamage()
    {
        return damage;
    }
    public void HealingAction(DamageableEntity hitCharacter)
    {
        damage = HealingDamage;
        if (hitCharacter is CharacterBehaviour characterBehaviour)
        {
            if (characterBehaviour is TowerBehaviour)
                return;
            ShootDoublePass firingAbility = Owner.GetComponent<ShootDoublePass>();
            int chainCount = (firingAbility != null ? firingAbility.PossessionChainCount : 0) + 1;
            if (chainCount >= MaxPossessionChainLength)
            {
                ApplyFinalChainEffect(characterBehaviour);
            }
            else
            {
                ActiveCharacterManager.Instance.ChangeActiveCharacter(characterBehaviour);
                hitCharacter.DeathEvent += ActiveCharacterManager.Instance.ResetActiveCharacter;
                ShootDoublePass shootDoublePass = characterBehaviour.gameObject.AddComponent<ShootDoublePass>();
                shootDoublePass.IsPossessionGranted = true;
                shootDoublePass.PossessionChainCount = chainCount;
                characterBehaviour.aIHandler.AddAbility(shootDoublePass, HUD.Instance.SecondaryReloader);
            }
            firingAbility.EndPossession();
            Destroy(this.gameObject);
        }
    }
    private void ApplyFinalChainEffect(CharacterBehaviour target)
    {
        Debug.Log("yay");
    }
}
