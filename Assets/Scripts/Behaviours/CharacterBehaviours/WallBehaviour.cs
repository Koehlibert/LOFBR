using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System;
using TMPro;
using Unity.Services.Analytics;
public class WallBehaviour : DamageableEntity
{
    private int MemberCount;
    private List<GameObject> Members;
    private Bounds memberBounds;
    private float MemberWidth;
    private BoxCollider col;
    public override void Init()
    {
        base.Init();
        Members = new List<GameObject>();
        float midPoint = (MemberCount + 1) / 2;
        for (int i = 1; i <= MemberCount; i++)
        {
            GameObject member = CharacterFactory.Instance.CreateWallMember(this.Team, new Vector3((i - midPoint) * MemberWidth, 0, 0), this.gameObject);
            Renderer rend = member.GetComponentInChildren<SkinnedMeshRenderer>();
            rend.material = Team == CombatUtils.Team.Player
                ? MaterialLibrary.Instance.playerMaterial
                : MaterialLibrary.Instance.enemyMaterial;
            Color color = rend.material.color;
            color.a = 0.5f;
            rend.material.color = color;
            if (i == 1)
                memberBounds = rend.bounds;
            else
                memberBounds.Encapsulate(rend.bounds);
            Members.Add(member);
        }
        col = gameObject.AddComponent<BoxCollider>();
        col.center = transform.InverseTransformPoint(memberBounds.center);
        col.size = transform.InverseTransformVector(memberBounds.size);
        col.enabled = false;
        LastHit = false;
        hpsys.Initialize(80 * MemberCount, 0, 0, 0);
        aIHandler = gameObject.AddComponent<AIHandler>();
        aIHandler.Init(this, new List<Ability>(), new List<AIModule>(), 0, false);
    }
    public void Init(CombatUtils.Team team, int memberCount, float memberWidth)
    {
        this.Team = team;
        this.MemberCount = memberCount;
        this.MemberWidth = memberWidth;
        Init();
    }
    protected override void Die()
    {
        Destroy(this.gameObject);
    }
    public void Activate()
    {
        col.enabled = true;
        col.isTrigger = true;
        foreach (GameObject member in Members)
        {
            Renderer rend = member.GetComponentInChildren<SkinnedMeshRenderer>();
            Color color = rend.material.color;
            color.a = 1f;
            rend.material.color = color;
        }
        StartCoroutine(DelayedDestroy(5f));
    }
    private IEnumerator DelayedDestroy(float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        Die();
    }
}