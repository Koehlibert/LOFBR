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
}
