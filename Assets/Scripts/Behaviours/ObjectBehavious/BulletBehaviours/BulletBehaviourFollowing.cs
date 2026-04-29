using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviourFollowing : BulletBehaviour
{
    protected float speed;
    protected float focusDistance = 17.5f;
    protected float rotationSpeed = 2.75f;
    protected bool isFollowingTarget;
    protected GameObject target;
    protected ClosestFinder closestFinder;
    protected bool WasFired;
    protected bool OnlyHurt;
    protected bool ChasePlayer;
    public void Init(DamageableEntity owner, bool destroyOnHit, HumanBodyBones bone, bool onlyHurt, bool chasePlayer,
                     CombatUtils.Team targetTeam, float speed = 35f, float focusDistance = 15,
                     float rotationSpeed = 2.75f, float timer = 1.2F)
    {
        OnlyHurt = onlyHurt;
        ChasePlayer = chasePlayer;
        WasFired = false;
        this.speed = speed;
        this.focusDistance = focusDistance;
        this.rotationSpeed = rotationSpeed;
        base.Init(owner, destroyOnHit, bone, timer);
        closestFinder = new ClosestFinder(owner.Team, targetTeam, this.gameObject);
    }
    public override void Shoot(DamageInfo damageInfo)
    {
        Activate(damageInfo);
        SetFired();
        DelayedDestroy();
    }
    protected override void FixedUpdate()
    {
        if (WasFired)
        {
            target = closestFinder.FindClosestNoTower(ChasePlayer, OnlyHurt);
            if (!target)
            {
                MoveForward();
            }
            if (target)
            {
                if (Vector3.Distance(transform.position, target.transform.position) < focusDistance)
                {
                    isFollowingTarget = true;
                }
                else
                {
                    isFollowingTarget = false;
                }
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, target.transform.position - transform.position, rotationSpeed * Time.deltaTime, 0.0F);
                MoveForward();
                if (isFollowingTarget)
                {
                    transform.rotation = Quaternion.LookRotation(newDirection);
                }
            }
            else
            {
                MoveForward();
            }
        }
        else
        {
            base.FixedUpdate();
        }
    }
    private void MoveForward()
    {
        MoveForward(Time.deltaTime);
    }
    protected virtual void MoveForward(float rate)
    {
        transform.Translate(Vector3.forward * rate * speed, Space.Self);
        Vector3 temp = transform.position;
        temp.y = 0.4f;
        transform.position = temp;
    }
    protected void SetFired()
    {
        WasFired = true;
    }
}