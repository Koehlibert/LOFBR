using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltBladeFlurry : Ability
{
    private float duration = .4f;
    private List<ObjectWithDist> flurryPos;
    private Damage damage;
    new void Start()
    {
        base.Start();
        loaded = true;
    }
    void Update()
    {
        if (Input.GetButtonDown("Ult")&&(loaded)&&player.manasys.checkCost(manaCost))
        {
            flurryPos = MasterScript.Instance.GetFlurryTargets(player.levelsys.getLevel() -1);
            if (flurryPos.Count > 0)
            {
                StartCoroutine("reload");
                StartCoroutine("Flurry");
                reloader.shoot();
                player.manasys.useMana(manaCost);
            }
        }
    }
    private IEnumerator Flurry()
    {
        StartCoroutine(player.LockMovement(duration*(flurryPos.Count + 1)));
        StartCoroutine(player.LockView(duration*(flurryPos.Count + 1)));
        damage = gameObject.AddComponent<Damage>();
        damage.SetProperties(25 + player.levelsys.getLevel() * 10, 0, CombatUtils.Team.Player, false, true);
        yield return new WaitForSeconds(duration);
        foreach (ObjectWithDist enemy in flurryPos)
        {
            GameObject target = enemy.GetObject();
            if (target)
            {
                Vector3 offset = GetOffset(target.transform.position);
                player.transform.position = target.transform.position + offset;
                Quaternion lookDir = Quaternion.LookRotation(-offset);
                player.transform.rotation = lookDir;
                player.animator.Play("Melee",0,0f);
                target.GetComponent<EnemyBehaviour>().getShanked(damage);
                yield return new WaitForSeconds(duration);
            }
            else
            {
                continue;
            }
        }
        Destroy(damage);
        player.animator.Play("Default",0,0f);
    }
    private Vector3 GetOffset(Vector3 target)
    {
        Vector3 dir = Random.insideUnitCircle;
        while (dir.magnitude == 0)
        {
            dir = Random.insideUnitCircle;
        }
        dir.z = dir.y;
        dir.y = 0;
        dir = dir.normalized*4;
        Vector3 temp = target + dir;
        if (Mathf.Abs(temp.x) >= 19)
        {
            dir.x = -dir.x;
        }
        if ((temp.z <= MasterScript.Instance.friendlySpawn.getZPos() + 1)||(temp.z >= MasterScript.Instance.enemySpawn.getZPos() - 1))
        {
            dir.y = -dir.y;
        }
        return dir;
    }
}
