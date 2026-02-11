using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.XR;

public class MovementAI : Ability
{
    public GameObject Target { get; set; }
    private Vector3 standarddirection = new Vector3(0f, 0f, 1f);
    private float TeamDirectionMultiplier;
    public AIUtils.MovementState MovementState { get; }
    public bool moveLock;
    public bool CaresAboutHealth;
    public float Movementspeed { get; set; }
    protected void OnEnable()
    {
        TeamDirectionMultiplier = 1;
        if (Handler.Owner.Team == CombatUtils.Team.Enemy)
        {
            TeamDirectionMultiplier *= -1;
        }
        standarddirection.z *= TeamDirectionMultiplier;
    }
    protected override void Update()
    {

    }
    public override void Checker()
    {
        if (!IsInteractive)
        {
            if (IsInteractive)
            {
                Handler.MovementDirection = new Vector3(-PlayerInputRouter.Instance.Move.y, 0, PlayerInputRouter.Instance.Move.x).normalized;
                return;
            }
        }
        if (CaresAboutHealth)
        {
            if (Handler.HealthState == AIUtils.HealthState.Hurt)
            {
                Handler.MovementDirection = GetDirection(new Vector3(0, 0, MasterScript.Instance.GetOpponentSpawnZ(CombatUtils.GetOpposingTeam(Handler.Owner.Team))));
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
        MoveCharacter(Handler.MovementDirection, Handler.ForceMovement);
    }
    private Vector3 GetDirection(Vector3 target) => target - Handler.Owner.transform.position;
    public void MoveCharacter(Vector3 direction, bool bypass = false)
    {
        Handler.Owner.AnimSpeed = 0;
        if (!moveLock || bypass)
        {
            Handler.Owner.AnimSpeed = direction.normalized.magnitude;
            transform.position = MasterScript.Instance.CorrectTarget(transform.position + direction * Movementspeed * Time.deltaTime);
        }
        Handler.Owner.animator.SetFloat("speedPercent", Handler.Owner.AnimSpeed);
    }
    public IEnumerator LockMovement(float duration)
    {
        moveLock = true;
        yield return new WaitForSeconds(duration);
        moveLock = false;
    }
    protected override bool InputPressed()
    {
        return false;
    }
    protected override void AbilityAction()
    {
    }
}