using System;
using System.Buffers;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class DistanceHandler : AIModule
{
    private float AttentionDistance;
    private float CheckDistance;
    private float Followdistance;
    private float Attackdistance;
    public override void Checker()
    {
        GameObject closestCurrentEnemy = Handler.ClosestFinder.FindClosest();
        if (closestCurrentEnemy == null)
        {
            closestCurrentEnemy = Handler.Owner.enemyBase;
        }
        float distance = CombatUtils.GetDistance(Handler.Owner.gameObject, closestCurrentEnemy);
        if (distance > AttentionDistance)
        {
            Handler.SetAIState(AIUtils.AIState.MoveOnly);
            return;
        }
        if (distance > CheckDistance)
        {
            Handler.SetAIState(AIUtils.AIState.CheckGeneralSkills);
            return;
        }
        if (distance > Followdistance)
        {
            Handler.SetAIState(AIUtils.AIState.CheckDistSkills);
            return;
        }
        if (distance > Attackdistance)
        {
            Handler.SetAIState(AIUtils.AIState.CheckShoot);
            return;
        }
        else
        {
            Handler.SetAIState(AIUtils.AIState.Attacking);
            return;
        }
    }
}