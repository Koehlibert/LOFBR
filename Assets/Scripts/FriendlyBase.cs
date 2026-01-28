using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;
public class FriendlyBase : MonoBehaviour, IMortal
{
    public Health hpsys;
    // Start is called before the first frame update
    void Start()
    {
        hpsys = GetComponent<Health>();
        hpsys.Initialize(MasterScript.Instance.baseMaxHp,0,0,20);
    }
    void Update()
    {
        
    }
    void OnCollisionEnter(Collision col)
    {
        if (col.HasAnyTag(new List<string>() { "BulletEnemy", "BulletEnemyPlayer" }))
        {
            if (CombatUtils.DealDamage(col, this))
            {
                Die();
            }
            Destroy(col.gameObject);
        }
    }
    public void Die()
    {
        MasterScript.Instance.victory = false;
        MasterScript.Instance.gameOver = true;
    }
    public Health GetHealth()
    {
        return hpsys;
    }
}
