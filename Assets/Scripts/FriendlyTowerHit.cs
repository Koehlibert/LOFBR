using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;
public class FriendlyTowerHit : DamageableEntity
{
    private TowerBehaviourFriendly tower;
    protected override void Start()
    {
        base.Start();
        tower = GetComponentInChildren<TowerBehaviourFriendly>();
        hpsys.Initialize(300,0,0,20);
    }
    /* protected override void ConfigureCollisionRules(DamageCollisionHandler handler)
    {
        handler.AddRule(new DamageCollisionHandler.CollisionRule
        {
            tags = new List<string> { "BulletEnemy", "BulletEnemyPlayer" },
            eventType = DamageCollisionHandler.CollisionEventType.TriggerEnter,
            destroyOnHit = true
        });
    } */
    public override CombatUtils.Team Team => CombatUtils.Team.Player;
    public override void Die()
    {
        MasterScript.Instance.allFriendliesTowers.Remove(this.gameObject);
        Destroy(this.gameObject);
    }
    public override Health GetHealth()
    {
        return hpsys;
    }
}
