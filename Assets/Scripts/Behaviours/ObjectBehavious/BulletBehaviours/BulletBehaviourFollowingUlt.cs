using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BulletBehaviourFollowingUlt : BulletBehaviourFollowing
{
    public float count;
    private GameObject target;
    protected ClosestFinder closestFinder;
    protected override void FixedUpdate()
    {
        if (count <= 0)
        base.FixedUpdate();
    }
    protected override void MoveForward(float rate)
    {
        transform.Translate(Vector3.forward * rate * speed, Space.Self);
        Vector3 temp = transform.position;
        temp.y = Math.Max(temp.y, 0.3f);
        transform.position = temp;
    }
}
