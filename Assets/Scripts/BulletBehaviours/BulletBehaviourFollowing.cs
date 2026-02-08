using System.Collections;
using System.Collections.Generic;
using UnityEditor.Overlays;
using UnityEngine;

public class BulletBehaviourFollowing : BulletBehaviour
{
    protected float speed;
    private float focusDistance = 17.5f;
    private float rotationSpeed = 2.75f;
    private bool isFollowingTarget;
    private GameObject target;
    private ClosestFinder closestFinder;
    private bool WasFired;
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
        damage.SetProperties(damageInfo);
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
                return;
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
                MoveForward(Time.deltaTime);
                if (isFollowingTarget)
                {
                    transform.rotation = Quaternion.LookRotation(newDirection);
                }
            }
            else
            {
                MoveForward(Time.deltaTime);
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