using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class TowerBehaviour : CharacterBehaviour
{
    protected float range;
    protected float lookRange;
    protected GameObject currentenemy;
    protected float cooldown;
    protected GameObject bulletinstance;
    protected ClosestFinder closestFinder;
    public override void Init()
    {
        base.Init();
    }
    public void Init(CombatUtils.Team team)
    {
        this.Team = team;
        Init();
    }
    protected override void InitializeAIHandler()
    {
        base.InitializeAIHandler();
        ThrowBall throwBall = gameObject.AddComponent<ThrowBall>();
        aIHandler.Init(this, new List<Ability> { throwBall }, new List<AIModule>(), 0, false);
        aIHandler.LockMovement(Mathf.Infinity);
    }
    protected override void InitializeHPSys()
    {
        hpsys.Initialize(300, 0, 0, 20);
    }
    protected override void Die()
    {
        if (Team == CombatUtils.Team.Enemy)
        {
            CharacterTracker.Instance.allEnemiesTowers.Remove(this.gameObject);
        }
        else
        {
            CharacterTracker.Instance.allFriendliesTowers.Remove(this.gameObject);
        }
        Destroy(this.gameObject);
    }
    public override Health GetHealth()
    {
        return hpsys;
    }
}
