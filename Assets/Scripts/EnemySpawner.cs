using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour, ISpawner
{
    public GameObject Mob;
    public float spawntimer;
    public Vector3 pos1;
    public Vector3 pos2;
    private Quaternion spawndirection;
    private float timer;

    public float getZPos()
    {
        return pos1.z;
    }
    public void moveSpawner()
    {
        pos1.z -= 10;
        pos2.z -= 10;
    }
    public void setEnabled(bool val)
    {
        this.enabled = val;
    }

    public void speedUpSpawner(float val)
    {
        spawntimer *= val;
    }

    // Start is called before the first frame update
    void Start()
    {
        timer = spawntimer;
        pos1 = new Vector3(-10,1.5f,99);
        pos2 = new Vector3(10,1.5f,99);
        spawndirection = new Quaternion(0,180,0,0);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            Instantiate(Mob,pos1, spawndirection);
            Instantiate(Mob,pos2, spawndirection);
            timer = spawntimer;
        }
    }
}
