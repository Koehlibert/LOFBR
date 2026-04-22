using System;
using System.Collections;
using UnityEngine;

public class MovementAI : AIModule
{
    public GameObject Target { get; set; }
    private Vector3 standarddirection = new Vector3(0f, 0f, 1f);
    private float TeamDirectionMultiplier;
    private AIUtils.MovementState MovementState { get; set; }
    private AIUtils.MovementState LastMovementState { get; set; }
    public bool MoveLock;
    private bool LookLock;
    public bool CaresAboutHealth;
    public float Movementspeed { get; set; }
    public float Speedup = 1;
    private Vector3 MovementTarget;
    public event Action OnTargetReached;
    private float CircleDirection = 1;
    private float CircleRadius = 7.5f;
    private float CircleSpeed = 1.5f;
    private float Angle = 0;
    private bool IsDefensive;
    private float DefensiveDistance = 12;
    private float DistanceToDash = 7.5f;
    public bool ForceMovement;
    public event Action CouldDash;
    private Vector3 MovementDirection;
    public Vector3 LookDirection;
    public void Init(bool isInteractive, AIHandler aIHandler, float movementSpeed, bool caresAboutHealth)
    {
        base.Init(isInteractive, aIHandler);
        MovementDirection = new Vector3();
        Speedup = 1;
        MoveLock = false;
        CaresAboutHealth = caresAboutHealth;
        Movementspeed = movementSpeed;
        TeamDirectionMultiplier = 1;
        ForceMovement = false;
        if (Handler.Owner.Team == CombatUtils.Team.Enemy)
        {
            TeamDirectionMultiplier *= -1;
        }
        standarddirection.z *= TeamDirectionMultiplier;
        MovementState = AIUtils.MovementState.IsMovingForward;
        LastMovementState = AIUtils.MovementState.IsMovingForward;
        LookDirection = new Vector3();
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
            MovementDirection = new Vector3(-PlayerInputRouter.Instance.Move.y, 0, PlayerInputRouter.Instance.Move.x).normalized;
            Vector2 mouseScreenPosition = PlayerInputRouter.Instance.Look;
            Plane playerPlane = new Plane(Vector3.up, transform.position);
            Ray ray = Camera.main.ScreenPointToRay(mouseScreenPosition);
            if (playerPlane.Raycast(ray, out float hitDist))
            {
                LookDirection = ray.GetPoint(hitDist);
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
                SetMovementDirection(GetDirection(new Vector3(0, 0, MasterScript.Instance.GetOpponentSpawnZ(CombatUtils.GetOpposingTeam(Handler.Owner.Team)))));
                Handler.SetAIState(AIUtils.AIState.Retreating);
                //Handler.ReenableOtherAbilities();
            }
            if (MovementState == AIUtils.MovementState.IsCircling)
            {
                SetMovementDirection(GetDirection(GetCircularTarget(Handler.closestEnemyNoTower)));
            }
            if (MovementState == AIUtils.MovementState.IsStanding)
            {
                SetMovementDirection(new Vector3(0, 0, 0));
            }
            if (MovementState == AIUtils.MovementState.IsMovingForward)
            {
                SetMovementDirection(GetDirection(Handler.transform.position + standarddirection));
                LookDirection = Handler.Owner.transform.position + standarddirection;
            }
            if (MovementState == AIUtils.MovementState.IsFollowingTarget)
            {
                SetMovementDirection(GetDirection(Handler.closestEnemy.transform.position));
            }
            if (MovementState == AIUtils.MovementState.IsGoingToPlace)
            {
                SetMovementDirection(GetDirection(MovementTarget));
            }
        }
    }
    public void SetMovementTarget(Vector3 movementTarget)
    {
        MovementTarget = movementTarget;
    }
    public void HandleMovement()
    {
        MoveCharacter(MovementDirection, ForceMovement, Speedup);
    }
    public Vector3 GetDirection(Vector3 target)
    {
        Vector3 direction = MasterScript.Instance.CorrectTarget(target) - Handler.Owner.transform.position;
        if (IsDefensive)
        {
            direction = GetDefensiveDirection(direction);
        }
        direction.y = 0;
        if (direction.magnitude > DistanceToDash)
        {
            MovementDirection = direction;
            CouldDash?.Invoke();
            CouldDash = null;
        }
        if (IsDefensive && direction.z < 1)
            direction.z = 0;
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
    private Vector3 GetDefensiveDirection(Vector3 direction)
    {
        GameObject furthestEnemy = CharacterTracker.Instance.GetFurthestEnemy(Handler.Owner.Team);
        if (furthestEnemy != null)
        {
            float safeZ = furthestEnemy.transform.position.z - TeamDirectionMultiplier * DefensiveDistance - Handler.Owner.transform.position.z;
            direction.z = Handler.Owner.Team == CombatUtils.Team.Player ? Mathf.Min(direction.z, safeZ) : Mathf.Max(direction.z, safeZ);
        }
        return direction;
    }
    public void MoveCharacter(Vector3 direction, bool bypass = false, float speedup = 1)
    {
        Handler.Owner.AnimSpeed = 0; //still necessary?
        if (!MoveLock || bypass)
        {
            Handler.Owner.AnimSpeed = direction.normalized.magnitude;//still necessary?
            direction = Movementspeed * speedup * Time.deltaTime * direction;
            Vector3 newPos = MasterScript.Instance.CorrectTarget(transform.position + direction);
            Handler.Owner.transform.position = newPos;
            if (MovementState == AIUtils.MovementState.IsGoingToPlace && FlatDistance(newPos, MovementTarget) < 1)
            {
                OnTargetReached?.Invoke();
                OnTargetReached = null;
            }
            if (!bypass)
            {
                Vector3 worldMove = new(direction.normalized.x, 0f, direction.normalized.z);
                worldMove = Vector3.ClampMagnitude(worldMove, 1f);
                Vector3 localMove = transform.InverseTransformDirection(worldMove);
                if (Handler.Owner.animator != null)
                {
                    Handler.Owner.animator.SetFloat("moveX", localMove.x);
                    Handler.Owner.animator.SetFloat("moveZ", localMove.z);
                }
            }
        }
    }
    public void HandleLook()
    {
        if (!LookLock)
        {
            Vector3 dir = LookDirection - transform.position;
            dir.y = 0;
            if (dir.sqrMagnitude > 0.001f)
            {
                transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
            }
        }
    }
    public void ResetAfterDeath()
    {
        LookLock = false;
        MoveLock = false;
        ForceMovement = false;
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
    public void LockMovementAI(float duration)
    {
        StartCoroutine(LockMovement(duration));
        StartCoroutine(LockView(duration));
    }
    public void LockMovementAI()
    {
        MoveLock = true;
        LookLock = true;
    }
    public void UnlockMovementAI()
    {
        MoveLock = false;
        LookLock = false;
    }
    private float FlatDistance(Vector3 pos1, Vector3 pos2)
    {
        pos1.y = 0;
        pos2.y = 0;
        return Vector3.Distance(pos1, pos2);
    }
    public void SetMovementState(AIUtils.MovementState movementState)
    {
        MovementState = movementState;
    }
    public void SetMovementDirection(Vector3 direction)
    {
        MovementDirection = direction;
    }
    public Vector3 GetMovementDirection()
    {
        return MovementDirection;
    }
    public void SetEvenLookDirection(Vector3 direction)
    {
        direction.y = 0;
        LookDirection = direction;
    }
    public IEnumerator SetForcemovement(float duration)
    {
        ForceMovement = true;
        yield return new WaitForSeconds(duration);
        ForceMovement = false;
    }
    public void SetCircleBehaviour(float newRadius, float newSpeed)
    {
        CircleRadius = newRadius;
        CircleSpeed = newSpeed;
    }
}