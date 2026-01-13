using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltRez : Ability
{
    private MasterScript master;
    public GameObject Mob;
    private Quaternion spawndirection = new Quaternion(0,0,0,0);
    new void Start()
    {
        base.Start();
        master = FindObjectOfType<MasterScript>();
        loaded = true;
        player = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Ult")&&(loaded)&&player.manasys.checkCost(manaCost))
        {
            List<Vector3> locations = master.GetRezPositions(player.levelsys.getLevel() - 2);
            if(locations.Count > 0)
            {
                StartCoroutine("reload");
                Rez(locations);
                reloader.shoot();
                player.manasys.useMana(manaCost);
            }
        }
    }
    private void Rez(List<Vector3> locations)
    {
        
        foreach(Vector3 pos in locations)
        {
            Instantiate(Mob,pos, spawndirection);
        }
    }
}
