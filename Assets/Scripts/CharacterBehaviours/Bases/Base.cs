using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;
public abstract class Base : DamageableEntity
{
    protected override void Start()
    {
        hpsys = GetComponent<Health>();
        hpsys.Initialize(MasterScript.Instance.baseMaxHp,0,0,20);
    }
    protected override void Die()
    {
        MasterScript.Instance.gameOver = true;
        if (Team == CombatUtils.Team.Enemy)
        {
            MasterScript.Instance.victory = true;
        }
        else
        {
            MasterScript.Instance.victory = false;
        }
    }
}
