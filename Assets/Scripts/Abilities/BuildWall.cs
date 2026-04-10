using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class BuildWall : SelectionAbility
{
    protected float DistanceToCheck = 40;
    protected float DistanceToTrigger = 30;
    private InDistanceTracker inDistanceTrackerEnemies;
    private InDistanceTracker inDistanceTrackerFriendlies;
    private float MemberWidth = 2f;
    private GameObject Wall = null;
    private int NEnemiesToTrigger = 2;
    private int NFriendliesToTrigger = 2;
    private ClosestFinder closestFriendlyFinder;
    protected override AbilityInfo GetAbilityInfo()
    {
        return new AbilityInfo(120, 20f, new List<AIUtils.AIState> { AIUtils.AIState.Attacking, AIUtils.AIState.CheckShoot, AIUtils.AIState.CheckDistSkills, AIUtils.AIState.Retreating });
    }
    protected override void AdditionalInit()
    {
        if (!IsInteractive)
        {
            closestFriendlyFinder = new(Handler.Owner.Team, Handler.Owner.Team, this.gameObject);
            inDistanceTrackerFriendlies = closestFriendlyFinder.StartTrackingDist(DistanceToCheck, false);
            inDistanceTrackerEnemies = Handler.ClosestFinder.StartTrackingDist(DistanceToCheck, true, CombatUtils.GetOpposingTeam(Handler.Owner.Team));
            inDistanceTrackerEnemies.ShouldDebug = true;
        }
    }
    protected override void HandleSelection()
    {
        if (Wall == null)
            Wall = CreateWall();
        Ray ray = Camera.main.ScreenPointToRay(PlayerInputRouter.Instance.Look);
        Plane groundPlane = new(Vector3.up, Vector3.zero);
        if (groundPlane.Raycast(ray, out float distance))
        {
            Vector3 worldPoint = ray.GetPoint(distance);
            MoveWall(worldPoint);
        }
        if (ConfirmInputPressed())
        {
            ToggleSelecting();
            AbilityAction();
        }
    }
    protected override void AbilityAction()
    {
        base.AbilityAction();
        StartCoroutine(Reload());
        if (!IsInteractive)
        {
            Wall = CreateWall();
            Vector3 targetPos = (Handler.Owner.transform.position + Handler.closestEnemy.transform.position) / 2f;
            MoveWall(targetPos);
        }
        Wall.GetComponent<WallBehaviour>().Activate();
    }
    protected override bool InputPressed()
    {
        return PlayerInputRouter.Instance.SkillPressedThisFrame;
    }
    protected override void AICheck()
    {
        if (loaded)
        {
            if (CheckManaCost())
            {
                GameObject closestFriendly = closestFriendlyFinder.FindClosestNoTower();
                if (closestFriendly != null)
                {
                    float closestFriendlyDistance = CombatUtils.GetDistance(Handler.Owner.gameObject, closestFriendly);
                    if ((closestFriendlyDistance < DistanceToTrigger) &&
                        inDistanceTrackerEnemies.GetOverCount(NEnemiesToTrigger) && inDistanceTrackerFriendlies.GetOverCount(NFriendliesToTrigger) &&
                        Handler.distanceToClosest < DistanceToTrigger)
                    {
                        SetFinalAction();
                    }
                }
            }
        }
    }
    private void MoveWall(Vector3 targetPos)
    {
        targetPos.y = 0;
        targetPos.x = CorrectWallX(targetPos.x, GetMemberCount());
        Wall.transform.position = targetPos;
    }
    protected int GetMemberCount()
    {
        int count = 1;
        if (Handler.Owner is MainPlayerBehaviour)
        {
            int level = (Handler.Owner as MainPlayerBehaviour).Levelsys.GetLevel();
            count = level switch
            {
                < 2 => 1,
                < 4 => 2,
                < 5 => 3,
                < 7 => 4,
                < 9 => 5,
                _ => 6
            };
        }
        return count;
    }
    protected float GetWallMemberHP()
    {
        if (Handler.Owner is MainPlayerBehaviour)
        {
            if (Handler.Owner is MirrorImageBehaviour)
            {
                return 45;
            }
            else
            {
                return 80;
            }
        }
        else
        {
            return 60;
        }
    }
    protected float CorrectWallX(float x, int memberCount)
    {
        return Mathf.Clamp(x, MasterScript.Instance.lowerAreaLimitX + memberCount / 2 * MemberWidth, MasterScript.Instance.upperAreaLimitX - memberCount / 2 * MemberWidth);
    }
    protected override void DisableSelection()
    {
        if (Wall != null)
            Destroy(Wall);
        base.DisableSelection();
    }
    protected GameObject CreateWall()
    {
        return BulletFactory.Instance.CreateWall(Handler.Owner, GetMemberCount(), MemberWidth, GetWallMemberHP());
    }
}
