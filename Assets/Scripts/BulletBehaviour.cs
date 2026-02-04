using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Damage))]
public class BulletBehaviour : MonoBehaviour
{
    protected DamageableEntity Owner;
    protected Damage damage;
    public CombatUtils.Team team;
    protected float timer;
    private SphereCollider col;
    protected Rigidbody rb;
    private HumanBodyBones bone;
    public virtual void Init(DamageableEntity owner, bool destroyOnHit, HumanBodyBones bone, float timer = 1.5f)
    {
        this.col = GetComponent<SphereCollider>();
        this.rb = GetComponent<Rigidbody>();
        this.damage = GetComponent<Damage>();
        col.enabled = false;
        this.Owner = owner;
        this.team = owner.Team;
        this.timer = timer;
        this.bone = bone;
        if (destroyOnHit)
        {
            damage.DamageDealt += () => Destroy(this.gameObject);
        }
    }
    protected virtual void FixedUpdate()
    {
        if (rb)
        {
            rb.transform.position = Owner.animator.GetBoneTransform(bone).position + Owner.transform.forward;
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
