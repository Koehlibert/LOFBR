using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTowerHit : MonoBehaviour, IMortal
{
    private TowerBehaviourEnemy tower;
    public Health hpsys;
    private MasterScript master;
    // Start is called before the first frame update
    void Start()
    {
        tower = GetComponentInChildren<TowerBehaviourEnemy>();
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
        if ((other.gameObject.CompareTag("Bullet"))||(other.gameObject.CompareTag("BulletPlayer")))
        {
            if (hpsys.TakeDamage(other.gameObject.GetComponent<Damage>().GetDamage()))
            {
                Die();
            }
            other.gameObject.SetActive(false);
            if (!other.gameObject.CompareTag("BulletPlayerShockwave")&&(!other.gameObject.CompareTag("MeleePlayer")))
            {
                Destroy(other.gameObject);
            }
        }
    }
    public void Die()
    {
        master.allEnemiesTowers.Remove(this.gameObject);
        Destroy(this.gameObject);
    }
}
