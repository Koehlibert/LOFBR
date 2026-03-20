using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpawnerBehaviour : MonoBehaviour
{
    protected float spawntimer;
    private Vector3 pos1;
    private Vector3 pos2;
    private Quaternion spawndirection;
    private float timer;
    protected abstract CombatUtils.Team Team {get;}
    protected float Direction;
    void Start()
    {
        spawntimer = 3.5f;
        timer = spawntimer;
        Direction = Team == CombatUtils.Team.Player ? -1 : 1;
        pos1 = new Vector3(-10,1.5f,Direction * 99);
        pos2 = new Vector3(10,1.5f, Direction * 99);
        spawndirection = Team == CombatUtils.Team.Player ? new Quaternion(0,0,0,0) : new Quaternion(0,180,0,0);
    }
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            CharacterFactory.Instance.CreateTeamMob(Team, pos1, spawndirection);
            CharacterFactory.Instance.CreateTeamMob(Team, pos2, spawndirection);
            timer = spawntimer;
        }
    }
    public float GetZPos()
    {
        return pos1.z;
    }
    public void MoveSpawner()
    {
        pos1.z -= Direction * 10;
        pos2.z -= Direction * 10;
    }
    public void SetEnabled(bool val)
    {
        this.enabled = val;
    }
    public void SpeedUpSpawner(float val)
    {
        spawntimer *= val;
    }
}
