using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;

public class Stomp : DamagingAbility
{
    private HumanBodyBones Bone = HumanBodyBones.LeftLowerLeg;
    public GameObject bullet;
    protected float ShockRadiusToCheck = 8;
    protected List<Ability> DisabledAbilities;
    private bool IsShocking;
    private InDistanceTracker inDistanceTracker;
    protected override void AdditionalInit()
    {
        IsShocking = false;
        soundType = AbilitySoundType.Stomp;
    }
    protected override void AbilityAction()
    {
        StartCoroutine("Shootanim");
        StartCoroutine("Reload");
        base.AbilityAction();           
    }
    protected override bool InputPressed()
    {
        return PlayerInputRouter.Instance.AlternativePressed;
    }
    protected override DamageInfo GetDamageValues()
    {
        return new DamageInfo(70 + (OwnerLevelSys.GetLevel() - 2) * 6, 0, Handler.Owner.Team, true, false, false);
    }
    private IEnumerator Shootanim()
    {
        if (IsInteractive)
            Handler.DisableOtherAbilities(this);
        movementAI.LockMovementAI(0.95f);
        Handler.Owner.animator.Play("Stomp", 0, 0f);
        yield return new WaitForSeconds(0.7f);
        GameObject wave = BulletFactory.Instance.CreateShockwave(Handler.Owner, false, Bone);
        wave.GetComponent<Damage>().SetProperties(GetDamageValues());
        Handler.ReenableOtherAbilities();
        IsShocking = false;
        PlaySound();
    }
    protected override AbilityInfo GetAbilityInfo()
    {
        return new AbilityInfo(80, 5, new List<AIUtils.AIState> { AIUtils.AIState.Attacking, AIUtils.AIState.CheckShoot, AIUtils.AIState.CheckDistSkills });
    }
    protected override void AICheck()
    {
        if (IsShocking)
        {
            List<GameObject> closest2Enemies = Handler.ClosestFinder.FindNClosest(2, true);
            (bool tmp, Vector3 ShockPoint) = ExistsPointWithinRadius(closest2Enemies, ShockRadiusToCheck * 0.75f);
            movementAI.SetMovementState(AIUtils.MovementState.IsGoingToPlace);
            if (inDistanceTracker.GetOverCount(2))
            {
                movementAI.SetMovementTarget(Handler.Owner.transform.position);
                Handler.ClosestFinder.StopTrackingDist(inDistanceTracker);
            }
        }
        else
        {
            if (loaded && CheckManaCost())
            {
                List<GameObject> closest3Enemies = Handler.ClosestFinder.FindNClosest(3, true);
                (bool ShouldShock, Vector3 ShockPoint) = ExistsPointWithinRadius(closest3Enemies, ShockRadiusToCheck);
                if (ShouldShock)
                {
                    IsShocking = true;
                    inDistanceTracker = Handler.ClosestFinder.StartTrackingDist(ShockRadiusToCheck, true);
                    movementAI.SetMovementState(AIUtils.MovementState.IsGoingToPlace);
                    movementAI.SetMovementTarget(ShockPoint);
                    Handler.DisableOtherAbilities(this);
                    movementAI.OnTargetReached += AbilityAction;
                }
            }
        }
    }
    (bool pointExists, Vector3 point) ExistsPointWithinRadius(List<GameObject> enemies, float r)
    {
        if (enemies.Count < 3)
            return (false, Vector3.zero);
        Vector3 centroid = new Vector3();
        foreach (GameObject enemy in enemies)
        {
            centroid += enemy.transform.position;
        }
        centroid /= enemies.Count;
        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(enemy.transform.position, centroid);
            if (distance >= r)
                return (false, Vector3.zero);
        }
        return (true, centroid);
    }
    void DrawDebugCircle(Vector3 center, float radius, int segments = 32)
    {
        float angleStep = 360f / segments;

        for (int i = 0; i < segments; i++)
        {
            float angle1 = Mathf.Deg2Rad * angleStep * i;
            float angle2 = Mathf.Deg2Rad * angleStep * (i + 1);

            Vector3 p1 = center + new Vector3(Mathf.Cos(angle1), 0, Mathf.Sin(angle1)) * radius;
            Vector3 p2 = center + new Vector3(Mathf.Cos(angle2), 0, Mathf.Sin(angle2)) * radius;

            Debug.DrawLine(p1, p2, Color.red);
        }
    }
}
