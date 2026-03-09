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
    private float AttackDistance;
    public void Init(float attentionDistance, float checkDistance, float followdistance, float attackDistance)
    {
        AttentionDistance = attentionDistance;
        CheckDistance = checkDistance;
        Followdistance = followdistance;
        AttackDistance = attackDistance;
    }
    public override void Checker()
    {
        Handler.closestEnemy = Handler?.ClosestFinder?.FindClosest();
        if (Handler.closestEnemy == null)
        {
            Handler.closestEnemy = Handler.Owner.enemyBase;
        }
        Handler.distanceToClosest = CombatUtils.GetDistance(Handler.Owner.gameObject, Handler.closestEnemy);
        if (Handler.distanceToClosest < AttackDistance)
        {
            Handler.SetAIState(AIUtils.AIState.Attacking);
            return;
        }
        else if (Handler.distanceToClosest < Followdistance)
        {
            Handler.SetAIState(AIUtils.AIState.CheckShoot);
            return;
        }
        else if (Handler.distanceToClosest < CheckDistance)
        {
            Handler.SetAIState(AIUtils.AIState.CheckDistSkills);
            return;
        }
        else if (Handler.distanceToClosest > AttentionDistance)
        {
            Handler.SetAIState(AIUtils.AIState.CheckGeneralSkills);
            return;
        }
        else
        {
            Handler.SetAIState(AIUtils.AIState.MoveOnly);
            return;
        }
    }
}