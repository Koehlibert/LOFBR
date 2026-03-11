using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Extensions;
using System;
public abstract class MainPlayerBehaviour : DamageableEntity
{
    public Level Levelsys;
    public Mana manasys;
    protected override void Start()
    {
        base.Start();
        Levelsys = new Level();
        Levelsys.Init(this);
    }
    protected override void Die()
    {
        MasterScript.Instance.DieAndRespawn(Team);
    }
    public abstract void LevelUp();
}