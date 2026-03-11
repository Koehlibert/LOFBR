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
    public bool MoveLock;
    private bool LookLock;
    public bool CaresAboutHealth;
    public float Movementspeed { get; set; }
    public float Speedup;
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
        MoveLock = false;
        CaresAboutHealth = caresAboutHealth;
        Movementspeed = movementSpeed;
        TeamDirectionMultiplier = 1;
        if (Handler.Owner.Team == CombatUtils.Team.Enemy)
        {
            TeamDirectionMultiplier *= -1;
        }
        standarddirection.z *= TeamDirectionMultiplier;
    }
    public override void Checker()
    {
        if (IsInteractive)
        {
            if (!MoveLock)
            {
                Handler.MovementDirection = new Vector3(-PlayerInputRouter.Instance.Move.y, 0, PlayerInputRouter.Instance.Move.x).normalized;
            }
            Vector2 mouseScreenPosition = PlayerInputRouter.Instance.Look;
            Plane playerPlane = new Plane(Vector3.up, transform.position);
            Ray ray = Camera.main.ScreenPointToRay(mouseScreenPosition);
            if (playerPlane.Raycast(ray, out float hitDist))
            {
                Handler.LookDirection = ray.GetPoint(hitDist);
            }
            return;
        }
        if (CaresAboutHealth)
        {
            if (Handler.HealthState == AIUtils.HealthState.Hurt)
            {
                Handler.MovementDirection = GetDirection(new Vector3(0, 0, MasterScript.Instance.GetOpponentSpawnZ(CombatUtils.GetOpposingTeam(Handler.Owner.Team))));
                return;
            }
        }
        if (MovementState == AIUtils.MovementState.IsStanding)
        {
            Handler.MovementDirection = new Vector3(0, 0, 0);
            return;
        }
        if (MovementState == AIUtils.MovementState.IsMovingForward)
        {
            Handler.MovementDirection = standarddirection;
            Handler.LookDirection = Handler.Owner.transform.position + standarddirection; ;
            return;
        }
        if (MovementState == AIUtils.MovementState.IsFollowingTarget)
        {
            Handler.MovementDirection = GetDirection(Handler.closestEnemy.transform.position);
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
        MoveCharacter(Handler.MovementDirection, Handler.ForceMovement, Speedup);
        if (!MoveLock)
        {
            Speedup = 1;
        }
    }
    public Vector3 GetDirection(Vector3 target) => (target - Handler.Owner.transform.position).normalized;
    public void MoveCharacter(Vector3 direction, bool bypass = false, float speedup = 1)
    {
        Handler.Owner.AnimSpeed = 0;
        if (!MoveLock || bypass)
        {
            Handler.Owner.AnimSpeed = direction.normalized.magnitude;
            Vector3 newPos = MasterScript.Instance.CorrectTarget(transform.position + Movementspeed * Time.deltaTime * direction);
            Handler.Owner.transform.position = newPos;
            if (!bypass)
            {
                Vector3 worldMove = new Vector3(direction.normalized.x, 0f, direction.normalized.z);
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
    protected override void AbilityAction()
    {
    }
}