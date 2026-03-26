using System;
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
    public AIUtils.MovementState MovementState { get; set; }
    private AIUtils.MovementState LastMovementState { get; set; }
    public bool MoveLock;
    private bool LookLock;
    public bool CaresAboutHealth;
    public float Movementspeed { get; set; }
    public float Speedup;
    private Vector3 MovementTarget;
    public event Action OnTargetReached;
    private float CircleDirection = 1;
    private float CircleRadius = 7.5f;
    private float CircleSpeed = 2f;
    private float Angle = 0;
    private bool IsDefensive;
    private float DefensiveDistance = 8;
    private float DistanceToDash = 8;
    public event Action CouldDash;
    protected override void OnEnable()
    {
        base.OnEnable();
    }
    protected override AbilityInfo GetAbilityInfo()
    {
        return new AbilityInfo(0, 0, new List<AIUtils.AIState>());
    }
    public void Init(bool isInteractive, AIHandler aIHandler, float movementSpeed, bool caresAboutHealth)
    {
        base.Init(isInteractive, aIHandler);
        Speedup = 1;
        MoveLock = false;
        CaresAboutHealth = caresAboutHealth;
        Movementspeed = movementSpeed;
        TeamDirectionMultiplier = 1;
        if (Handler.Owner.Team == CombatUtils.Team.Enemy)
        {
            TeamDirectionMultiplier *= -1;
        }
        standarddirection.z *= TeamDirectionMultiplier;
        LastMovementState = AIUtils.MovementState.IsMovingForward;
    }
    public override void Checker()
    {
        IsDefensive = false;
        if (MoveLock)
            return;
        if (LastMovementState != MovementState)
        {
            //Debug.Log("Changed Movementstate from " + LastMovementState + " to " + MovementState);
            Speedup = 1;
            CircleDirection *= -1;
            if (Handler.closestEnemyNoTower != null)
                Angle = Mathf.Atan2(transform.position.z - Handler.closestEnemyNoTower.transform.position.z, transform.position.x - Handler.closestEnemyNoTower.transform.position.x);
            LastMovementState = MovementState;
        }
        if (IsInteractive)
        {
            Handler.MovementDirection = new Vector3(-PlayerInputRouter.Instance.Move.y, 0, PlayerInputRouter.Instance.Move.x).normalized;
            Vector2 mouseScreenPosition = PlayerInputRouter.Instance.Look;
            Plane playerPlane = new Plane(Vector3.up, transform.position);
            Ray ray = Camera.main.ScreenPointToRay(mouseScreenPosition);
            if (playerPlane.Raycast(ray, out float hitDist))
            {
                Handler.LookDirection = ray.GetPoint(hitDist);
            }
            return;
        }
        else
        {
            if (CaresAboutHealth)
            {
                if (Handler.HealthState == AIUtils.HealthState.Hurt)
                {
                    MovementState = AIUtils.MovementState.IsRetreating;
                }
                if (Handler.HealthState == AIUtils.HealthState.PartiallyDamaged)
                {
                    IsDefensive = true;
                }
            }
            if (MovementState == AIUtils.MovementState.IsRetreating)
            {
                Handler.MovementDirection = GetDirection(new Vector3(0, 0, MasterScript.Instance.GetOpponentSpawnZ(CombatUtils.GetOpposingTeam(Handler.Owner.Team))));
                Handler.SetAIState(AIUtils.AIState.Retreating);
                Handler.ReenableOtherAbilities();
            }
            if (MovementState == AIUtils.MovementState.IsCircling)
            {
                Handler.MovementDirection = GetDirection(GetCircularTarget(Handler.closestEnemyNoTower));
            }
            if (MovementState == AIUtils.MovementState.IsStanding)
            {
                Handler.MovementDirection = new Vector3(0, 0, 0);
            }
            if (MovementState == AIUtils.MovementState.IsMovingForward)
            {
                Handler.MovementDirection = standarddirection;
                Handler.LookDirection = Handler.Owner.transform.position + standarddirection; ;
            }
            if (MovementState == AIUtils.MovementState.IsFollowingTarget)
            {
                Handler.MovementDirection = GetDirection(Handler.closestEnemy.transform.position);
            }
            if (MovementState == AIUtils.MovementState.IsGoingToPlace)
            {
                Handler.MovementDirection = GetDirection(MovementTarget);
            }
        }
    }
    public void SetMovementTarget(Vector3 movementTarget)
    {
        MovementTarget = movementTarget;
    }
    public void HandleMovement()
    {
        MoveCharacter(Handler.MovementDirection, Handler.ForceMovement, Speedup);
    }
    public Vector3 GetDirection(Vector3 target)
    {
        Vector3 direction = MasterScript.Instance.CorrectTarget(target) - Handler.Owner.transform.position;
        if (IsDefensive)
        {
            direction.z = GetDefensiveZ(direction.z);
        }
        direction.y = 0;
        if (direction.magnitude > DistanceToDash)
        {
            CouldDash?.Invoke();
        }
        if (direction.magnitude > 1)
            direction = direction.normalized;
        return direction;
    }
    private Vector3 GetCircularTarget(GameObject objectToCircle)
    {
        Angle += CircleSpeed * CircleDirection * Time.deltaTime;
        float x = Mathf.Cos(Angle) * CircleRadius;
        float z = Mathf.Sin(Angle) * CircleRadius;
        return objectToCircle.transform.position + new Vector3(x, 0f, z);
    }
    private float GetDefensiveZ(float z)
    {
        GameObject furthestEnemy = Handler.ClosestFinder.GetFurthestNoTower();
        if (furthestEnemy != null)
        {
            return furthestEnemy.transform.position.z + DefensiveDistance;
        }
        else
            return z;
    }
    public void MoveCharacter(Vector3 direction, bool bypass = false, float speedup = 1)
    {
        Handler.Owner.AnimSpeed = 0; //still necessary?
        if (!MoveLock || bypass)
        {
            Handler.Owner.AnimSpeed = direction.normalized.magnitude;//still necessary?
            Vector3 newPos = MasterScript.Instance.CorrectTarget(transform.position + Movementspeed * Time.deltaTime * direction * speedup);
            Handler.Owner.transform.position = newPos;
            if (MovementState == AIUtils.MovementState.IsGoingToPlace && FlatDistance(newPos, MovementTarget) < 1)
            {
                OnTargetReached?.Invoke();
            }
            if (!bypass)
            {
                Vector3 worldMove = new(direction.normalized.x, 0f, direction.normalized.z);
                worldMove = Vector3.ClampMagnitude(worldMove, 1f);
                Vector3 localMove = transform.InverseTransformDirection(worldMove);
                Handler.Owner.animator.SetFloat("moveX", localMove.x);
                Handler.Owner.animator.SetFloat("moveZ", localMove.z);
            }
        }
    }
    public void HandleLook()
    {
        if (!LookLock)
        {
            Vector3 dir = Handler.LookDirection - transform.position;
            dir.y = 0;
            if (dir.sqrMagnitude > 0.001f)
            {
                transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
            }
        }
    }
    public IEnumerator LockMovement(float duration)
    {
        MoveLock = true;
        yield return new WaitForSeconds(duration);
        Speedup = 1;
        MoveLock = false;
    }
    public IEnumerator LockView(float duration)
    {
        LookLock = true;
        yield return new WaitForSeconds(duration);
        LookLock = false;
    }
    protected override bool InputPressed()
    {
        return false;
    }
    private float FlatDistance(Vector3 pos1, Vector3 pos2)
    {
        pos1.y = 0;
        pos2.y = 0;
        return Vector3.Distance(pos1, pos2);
    }
}