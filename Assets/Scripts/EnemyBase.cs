using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;
public class EnemyBase : MonoBehaviour, IMortal
{
    public Health hpsys;
    private MasterScript master;
    private List<string> damagingTags = new List<string>() { "Bullet", "BulletPlayer" };
    void Start()
    {
        master = FindAnyObjectByType<MasterScript>();
        hpsys = GetComponent<Health>();
        hpsys.Initialize(master.baseMaxHp,0,0,20);
    }
    void OnCollisionEnter(Collision col)
    {
        if ((col.HasAnyTag(damagingTags)))
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
        master.victory = true;
        master.gameOver = true;
    }
    public Health GetHealth()
    {
        return hpsys;
    }
}