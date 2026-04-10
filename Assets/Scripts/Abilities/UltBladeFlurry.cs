using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class UltBladeFlurry : DamagingAbility
{
    private float duration = .4f;
    private List<ObjectWithDist> flurryPos;
    private Damage damage;
    protected override AbilityInfo GetAbilityInfo()
    {
        return new AbilityInfo(225, 20, new List<AIUtils.AIState> { AIUtils.AIState.Attacking, AIUtils.AIState.CheckShoot });
    }
    private IEnumerator Flurry()
    {
        movementAI.LockMovementAI(duration * (flurryPos.Count + 1));
        Handler.DisableOtherAbilities(duration * (flurryPos.Count + 1), this);
        damage = gameObject.AddComponent<Damage>();
        damage.SetProperties(GetDamageValues());
        yield return new WaitForSeconds(0.2f * duration);
        foreach (ObjectWithDist enemy in flurryPos)
        {
            GameObject target = enemy.GetObject();
            if (target != null)
            {
                Vector3 offset = GetOffset(target.transform.position);
                Vector3 targetPos = target.transform.position;
                targetPos.y = 0;
                Handler.Owner.transform.position = targetPos + offset;
                Quaternion lookDir = Quaternion.LookRotation(-offset);
                Handler.Owner.transform.rotation = lookDir;
                Handler.Owner.animator.Play("Melee", 0, 0f);
                MobBehaviour enemyBehaviour = target?.GetComponent<MobBehaviour>();
                enemyBehaviour?.getShanked(damage);
                yield return new WaitForSeconds(duration);
            }
            else
            {
                continue;
            }
        }
        Destroy(damage);
        Handler.Owner.animator.Play("Default", 0, 0f);
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
        if (IsInteractive)
            flurryPos = CharacterTracker.Instance.GetFlurryTargets(OwnerLevelSys.GetLevel() - 1, CombatUtils.GetOpposingTeam(Handler.Owner.Team));
        if (flurryPos.Count > 0)
        {
            StartCoroutine("Reload");
            StartCoroutine("Flurry");
            base.AbilityAction();
        }
    }
    protected override bool InputPressed()
    {
        return PlayerInputRouter.Instance.UltPressed;
    }
    protected override DamageInfo GetDamageValues()
    {
        if (Handler.Owner is MainPlayerBehaviour)
        {
            if (Handler.Owner is MirrorImageBehaviour)
            {
                return new DamageInfo(0.3f * (25 + (OwnerLevelSys.GetLevel() - 0) * 10), 3, CombatUtils.Team.Player, true, false, false);
            }
            else
            {
                return new DamageInfo(25 + (OwnerLevelSys.GetLevel() - 0) * 10, 1, CombatUtils.Team.Player, true, false, false);
            }
        }
        else 
        {
            return new DamageInfo(30, 0.5f, CombatUtils.Team.Player, true, false, false);
        }
    }
    protected override void AICheck()
    {
        if (loaded && CheckManaCost())
        {
            flurryPos = CharacterTracker.Instance.GetFlurryTargets(OwnerLevelSys.GetLevel() - 1, CombatUtils.GetOpposingTeam(Handler.Owner.Team));
            if (flurryPos.Count > 2)
            {
                SetFinalAction();
            }
        }
    }
}
