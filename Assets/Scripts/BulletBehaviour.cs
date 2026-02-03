using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class BulletBehaviour : MonoBehaviour
{
    protected DamageableEntity Owner;
    public Damage damage;
    public bool destroyOnHit;
    public CombatUtils.Team team;
    protected float timer;
    private SphereCollider col;
    private Rigidbody rb;
    public void Init(DamageableEntity owner, bool destroyOnHit, Damage damage, float timer = 1.5f)
    {
        col = GetComponent<SphereCollider>();
        rb = GetComponent<Rigidbody>();
        col.enabled = false;
        this.Owner = owner;
        this.damage = damage;
        this.team = owner.Team;
        this.timer = timer;
        if (destroyOnHit)
        {
            damage.DamageDealt += () => Destroy(this.gameObject);
        }
    }
    protected virtual void FixedUpdate()
    {
        if (rb)
        {
            rb.transform.position = Owner.animator.GetBoneTransform(HumanBodyBones.LeftLowerLeg).position + Owner.transform.forward;
        }
    }
    public virtual void Shoot(DamageInfo damageInfo)
    {
        damage.SetProperties(damageInfo);
        rb?.AddForce(Owner.transform.forward * 2250);
        DelayedDestroy();
        rb = null;
    }
    protected virtual void DelayedDestroy(float delay)
    {
        col.enabled = true;
        GetComponent<Rigidbody>().useGravity = true;
        Destroy(gameObject,delay);
    }
    public void DelayedDestroy()
    {
        DelayedDestroy(timer);
    }
}
