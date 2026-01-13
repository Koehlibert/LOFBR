using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
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
        if ((col.gameObject.CompareTag("Bullet"))||(col.gameObject.CompareTag("BulletPlayer")))
        {
            if (hpsys.TakeDamage(col.gameObject.GetComponent<Damage>().GetDamage()))
            {
                master.victory = true;
                master.gameOver = true;
            }
            Destroy(col.gameObject);
        }
    }
}
