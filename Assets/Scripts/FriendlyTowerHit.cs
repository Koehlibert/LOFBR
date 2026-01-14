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
        master = FindObjectOfType<MasterScript>();
    }
    protected override void ConfigureCollisionRules(DamageCollisionHandler handler)
    {
        handler.AddRule(new DamageCollisionHandler.CollisionRule
        {
            tags = new List<string> { "BulletEnemy", "BulletEnemyPlayer" },
            eventType = DamageCollisionHandler.CollisionEventType.TriggerEnter,
            destroyOnHit = true
        });
    }
    public override void Die()
    {
        master.allFriendliesTowers.Remove(this.gameObject);
        Destroy(this.gameObject);
    }
    public override Health GetHealth()
    {
        return hpsys;
    }
}
