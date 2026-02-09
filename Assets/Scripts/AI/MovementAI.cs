using System.Buffers;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class MovementAI : AIModule
{
    public GameObject Target { get; set; }
    private float MovementSpeed;
    private Vector3 standarddirection = new Vector3(0f, 0f, 1f);
    private float TeamDirectionMultiplier;
    public AIUtils.MovementState MovementState { get; }
    public bool CaresAboutHealth;
    protected void OnEnable()
    {
        TeamDirectionMultiplier = 1;
        if (Handler.Owner.Team == CombatUtils.Team.Enemy)
        {
            TeamDirectionMultiplier *= -1;
        }
        standarddirection.z *= TeamDirectionMultiplier;
    }
    public override void Checker()
    {
        if (CaresAboutHealth)
        {
            if (Handler.HealthState == AIUtils.HealthState.Hurt)
            {
                Handler.MovementTarget = new Vector3(0, 0, MasterScript.Instance.GetOpponentSpawnZ(CombatUtils.GetOpposingTeam(Handler.Owner.Team)));
                return;
            }
        }
        if (MovementState == AIUtils.MovementState.IsMovingForward)
        {
            //Move Handler Forward
            return;
        }
        if (MovementState == AIUtils.MovementState.IsFollowingTarget)
        {
            //Move Handler to target; don't lock AI
            return;
        }
        if (MovementState == AIUtils.MovementState.IsGoingToPlace)
        {
            //Move Handler towards Place; lock AI
            return;
        }
    }
    public void HandleMovement()
    {
        Handler.Owner.transform.Translate((Handler.MovementTarget - Handler.transform.position).normalized * MovementSpeed * Time.deltaTime);
    }
}