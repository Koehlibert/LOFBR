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
    public DamageableEntity Owner { get; private set; }
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
        rb.useGravity = false;
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
            if (Owner)
            {
                rb.transform.position = Owner.animator.GetBoneTransform(bone).position + Owner.transform.forward;
            }
            else
            {
                DelayedDestroy();
            }
        }
    }
    public virtual void Activate(DamageInfo damageInfo, bool activateGravity = false)
    {
        damage.SetProperties(damageInfo);
        if (activateGravity)
            rb.useGravity = true;
        col.enabled = true;
    }
    public virtual void Shoot(DamageInfo damageInfo)
    {
        Shoot(damageInfo, 2000);
    }
    public virtual void Shoot(DamageInfo damageInfo, float force)
    {
        Activate(damageInfo, true);
        rb?.AddForce(Owner.transform.forward * force);
        rb = null;
        DelayedDestroy();
    }
    public void UnsetRB()
    {
        rb = null;
    }
    protected virtual void DelayedDestroy(float delay)
    {
        GetComponent<Rigidbody>().useGravity = true;
        Destroy(gameObject, delay);
    }
    public void DelayedDestroy()
    {
        DelayedDestroy(timer);
    }
    public void StartDebugging()
    {
        damage.DamageDealt += () => Debug.Log("huh");
    }
}
