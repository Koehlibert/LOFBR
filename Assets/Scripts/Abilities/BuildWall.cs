using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class BuildWall : Ability
{
    private bool IsSelecting = false;
    //private float MarkDistance = 30f;
    private float MemberWidth = 2f;
    private GameObject Wall = null;
    protected override AbilityInfo GetAbilityInfo()
    {
        return new AbilityInfo(15, 2f, new List<AIUtils.AIState> { AIUtils.AIState.Attacking, AIUtils.AIState.CheckShoot, AIUtils.AIState.CheckDistSkills });
    }
    protected override void AdditionalInit()
    {
        reloadtime = 20;
    }
    protected override void InteractiveCheck()
    {
        if (!IsSelecting)
        {
            if (InputPressed() && loaded && CheckManaCost())
            {
                ToggleSelecting();
            }
        }
        else
        {
            if (Wall == null)
                Wall = CreateWall();
            Ray ray = Camera.main.ScreenPointToRay(PlayerInputRouter.Instance.Look);
            Plane groundPlane = new(Vector3.up, Vector3.zero);
            if (groundPlane.Raycast(ray, out float distance))
            {
                Vector3 worldPoint = ray.GetPoint(distance);
                worldPoint.y = 0;
                worldPoint.x = CorrectWallX(worldPoint.x, GetMemberCount());
                Wall.transform.position = worldPoint;
            }
            if (ConfirmInputPressed())
            {
                ToggleSelecting();
                AbilityAction();
            }
        }
    }
    private void ToggleSelecting()
    {
        IsSelecting = !IsSelecting;
        if (IsSelecting)
        {
            Handler.DisableOtherAbilities(this);
            Time.timeScale = 0.35f;
        }
        else
        {
            Handler.ReenableOtherAbilities();
            Time.timeScale = 1;
        }
    }
    /* protected override void AbilityAction()
    {
        if (!IsInteractive)
        {
            AbilityAction(Handler.closestEnemy.GetComponent<DamageableEntity>());
        }
    } */
    protected override void AbilityAction()
    {
        base.AbilityAction();
        StartCoroutine(Reload());
        if (Wall == null)
        {
            Wall = CreateWall();
        }
        Wall.GetComponent<WallBehaviour>().Activate();
    }
    protected override bool InputPressed()
    {
        return PlayerInputRouter.Instance.SkillPressedThisFrame;

    }
    /* protected override void AICheck()
    {
        if (loaded && CheckManaCost() && Handler.distanceToClosest < MarkDistance)
        {
            SetFinalAction();
        }
    } */
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
    protected float CorrectWallX(float x, int memberCount)
    {
        return Mathf.Clamp(x, MasterScript.Instance.lowerAreaLimitX + memberCount / 2 * MemberWidth, MasterScript.Instance.upperAreaLimitX - memberCount / 2 * MemberWidth);
    }
    protected bool ConfirmInputPressed()
    {
        return PlayerInputRouter.Instance.SkillPressedThisFrame;
    }
    protected GameObject CreateWall()
    {
        return BulletFactory.Instance.CreateWall(Handler.Owner, GetMemberCount(), MemberWidth);
    }
}
