using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class Base : DamageableEntity
{
    public override void Init()
    {
        base.Init();
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
