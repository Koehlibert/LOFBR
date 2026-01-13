using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.CompareTag("BulletEnemy"))||(other.gameObject.CompareTag("BulletEnemyPlayer")))
        {
            if (hpsys.TakeDamage(other.gameObject.GetComponent<Damage>().GetDamage()))
            {
                Die();
            }
            other.gameObject.SetActive(false);
            Destroy(other.gameObject);
        }
    }
    public void Die()
    {
        master.allFriendliesTowers.Remove(this.gameObject);
        Destroy(this.gameObject);
    }
}
