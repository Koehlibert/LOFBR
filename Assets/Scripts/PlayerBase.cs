using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    public Health hpsys;
    public MasterScript master;
    // Start is called before the first frame update
    void Start()
    {
        hpsys = GetComponent<Health>();
        hpsys.Initialize(master.baseMaxHp,0,0,20);
    }
    void Update()
    {
        
    }
    void OnCollisionEnter(Collision col)
    {
        if ((col.gameObject.CompareTag("BulletEnemy"))||(col.gameObject.CompareTag("BulletEnemyPlayer")))
        {
            if (hpsys.TakeDamage(col.gameObject.GetComponent<Damage>().GetDamage()))
            {
                master.victory = false;
                master.gameOver = true;
            }
            Destroy(col.gameObject);
        }

    }
}
