using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltRez : Ability
{
    public GameObject Mob;
    private Quaternion spawndirection = new Quaternion(0, 0, 0, 0);

    public override string InputString => "Ult";

    new void Start()
    {
        base.Start();
        loaded = true;
        player = GetComponent<PlayerController>();
    }
    private void Rez(List<Vector3> locations)
    {

        foreach (Vector3 pos in locations)
        {
            Instantiate(Mob, pos, spawndirection);
        }
    }

    protected override void AbilityAction()
    {
        List<Vector3> locations = MasterScript.Instance.GetRezPositions(player.levelsys.getLevel() - 2);
        if (locations.Count > 0)
        {
            StartCoroutine("reload");
            Rez(locations);
            reloader.shoot();
            player.manasys.useMana(manaCost);
        }
    }
}
