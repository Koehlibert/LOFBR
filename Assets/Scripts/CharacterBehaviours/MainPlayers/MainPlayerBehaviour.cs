using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Extensions;
using System;
using UnityEditor.SceneManagement;
public abstract class MainPlayerBehaviour : DamageableEntity
{
    public Level Levelsys;
    public Mana manasys;
    protected MainPlayerBehaviour EnemyPlayer;
    protected override void Start()
    {
        base.Start();
        EnemyPlayer = MasterScript.Instance.GetOpponentPlayer(Team);
        manasys = this.gameObject.AddComponent<Mana>();
        Levelsys = new Level();
        Levelsys.Init(this);
    }
    protected override void Die()
    {
        MasterScript.Instance.DieAndRespawn(Team);
    }
    public abstract void LevelUp();
}