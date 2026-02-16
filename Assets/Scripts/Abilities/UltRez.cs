using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltRez : Ability
{
    public GameObject Mob;
    private Quaternion spawndirection = new Quaternion(0, 0, 0, 0);
    new void Start()
    {
        base.Start();
        loaded = true;
        player = GetComponent<PlayerController>();
    }
    private void Rez(List<Vector3> locations)
    {
        Debug.Log("Rezing");
        foreach (Vector3 pos in locations)
        {
            Instantiate(Mob, pos, spawndirection);
        }
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        reloader = HUD.Instance.GetReload(HUD.Instance.UltReloader);
    }
    protected override void AbilityAction()
    {
        List<Vector3> locations = MasterScript.Instance.GetRezPositions(player.levelsys.getLevel() - 2);
        Debug.Log(locations.Count);
        if (locations.Count > 0)
        {
            StartCoroutine("reload");
            Rez(locations);
            reloader.shoot();
            player.manasys.useMana(manaCost);
        }
    }
    protected override bool InputPressed()
    {
        return PlayerInputRouter.Instance.UltPressed;
    }
}
