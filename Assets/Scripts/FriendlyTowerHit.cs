using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;
public class FriendlyTowerHit : MonoBehaviour, IMortal
{
    private TowerBehaviourFriendly tower;
    public Health hpsys;
    private MasterScript master;
    // Start is called before the first frame update
    void Start()
    {
        tower = GetComponentInChildren<TowerBehaviourFriendly>();
        hpsys = GetComponent<Health>();
        hpsys.Initialize(300,0,0,20);
        master = FindObjectOfType<MasterScript>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.HasAnyTag(new List<string>(){"BulletEnemy", "BulletEnemyPlayer"}))
        {
            if (CombatUtils.DealDamage(other, this))
            {
                Die();
            }
            Destroy(other.gameObject);
        }
    }
    public void Die()
    {
        master.allFriendliesTowers.Remove(this.gameObject);
        Destroy(this.gameObject);
    }
    public Health GetHealth()
    {
        return this.hpsys;
    }
}
