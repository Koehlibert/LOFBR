using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;
public class EnemyTowerHit : DamageableEntity
{
    private TowerBehaviourEnemy tower;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        tower = GetComponentInChildren<TowerBehaviourEnemy>();
        hpsys.Initialize(300,0,0,20);
        master = FindObjectOfType<MasterScript>();
    }
    /* protected override void ConfigureCollisionRules(DamageCollisionHandler handler)
    {
        handler.AddRule(new DamageCollisionHandler.CollisionRule
        {
            tags = new List<string> { "Bullet", "BulletPlayer" },
            eventType = DamageCollisionHandler.CollisionEventType.TriggerEnter,
            destroyOnHit = true
        });
    } */
    public override CombatUtils.Team Team => CombatUtils.Team.Enemy;
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
