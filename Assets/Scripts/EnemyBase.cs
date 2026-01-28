using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;
public class EnemyBase : MonoBehaviour, IMortal
{
    public Health hpsys;
    private List<string> damagingTags = new List<string>() { "Bullet", "BulletPlayer" };
    void Start()
    {
        hpsys = GetComponent<Health>();
        hpsys.Initialize(MasterScript.Instance.baseMaxHp,0,0,20);
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
        MasterScript.Instance.victory = true;
        MasterScript.Instance.gameOver = true;
    }
    public Health GetHealth()
    {
        return hpsys;
    }
}