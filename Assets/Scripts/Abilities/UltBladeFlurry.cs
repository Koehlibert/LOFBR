using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltBladeFlurry : DamagingAbility
{
    private float duration = .4f;
    private List<ObjectWithDist> flurryPos;
    private Damage damage;
    protected override void OnEnable()
    {
        base.OnEnable();
        reloader = HUD.Instance.GetReload(HUD.Instance.UltReloader);
    }
    protected override AbilityInfo GetAbilityInfo()
    {
        return new AbilityInfo(225, 20, new List<AIUtils.AIState> { AIUtils.AIState.Attacking, AIUtils.AIState.CheckShoot });
    }
    private IEnumerator Flurry()
    {
        StartCoroutine(player.aIHandler.movementAI.LockMovement(duration * (flurryPos.Count + 1)));
        StartCoroutine(player.aIHandler.movementAI.LockView(duration * (flurryPos.Count + 1)));
        damage = gameObject.AddComponent<Damage>();
        damage.SetProperties(GetDamageValues());
        yield return new WaitForSeconds(duration);
        foreach (ObjectWithDist enemy in flurryPos)
        {
            GameObject target = enemy.GetObject();
            if (target != null)
            {
                Vector3 offset = GetOffset(target.transform.position);
                Vector3 targetPos = target.transform.position;
                targetPos.y = 0;
                player.transform.position = targetPos + offset;
                Quaternion lookDir = Quaternion.LookRotation(-offset);
                player.transform.rotation = lookDir;
                player.animator.Play("Melee", 0, 0f);
                EnemyBehaviour enemyBehaviour = target?.GetComponent<EnemyBehaviour>();
                enemyBehaviour?.getShanked(damage);
                yield return new WaitForSeconds(duration);
            }
            else
            {
                continue;
            }
        }
        Destroy(damage);
        player.animator.Play("Default", 0, 0f);
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
        dir = dir.normalized * 4;
        Vector3 temp = target + dir;
        if (Mathf.Abs(temp.x) >= 19)
        {
            dir.x = -dir.x;
        }
        if ((temp.z <= MasterScript.Instance.friendlySpawn.GetZPos() + 1) || (temp.z >= MasterScript.Instance.enemySpawn.GetZPos() - 1))
        {
            dir.y = -dir.y;
        }
        return dir;
    }
    protected override void AbilityAction()
    {
        flurryPos = MasterScript.Instance.GetFlurryTargets(player.Levelsys.GetLevel() - 1);
        if (flurryPos.Count > 0)
        {
            StartCoroutine("reload");
            StartCoroutine("Flurry");
            reloader.shoot();
            player.manasys.useMana(manaCost);
        }
    }
    protected override bool InputPressed()
    {
        return PlayerInputRouter.Instance.UltPressed;
    }
    protected override DamageInfo GetDamageValues()
    {
        return new DamageInfo(25 + (player.Levelsys.GetLevel() - 0) * 10, 0, CombatUtils.Team.Player, true, false);
    }
}
