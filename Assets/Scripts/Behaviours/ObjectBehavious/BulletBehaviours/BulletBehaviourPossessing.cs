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
        DamagingDamage.SetProperties(healingInfo);
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
    private void DamagingAction(DamageableEntity hitCharacter)
    {
        damage = DamagingDamage;
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
            ActiveCharacterManager.Instance.ChangeActiveCharacter(characterBehaviour);
            hitCharacter.DeathEvent += ActiveCharacterManager.Instance.ResetActiveCharacter;
            Ability shootDoublePass = characterBehaviour.gameObject.AddComponent<ShootDoublePass>();
            characterBehaviour.aIHandler.AddAbility(shootDoublePass, HUD.Instance.SecondaryReloader);
        }
    }
}
