using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resurrect : Ability
{
    private Quaternion spawndirection = new Quaternion(0, 0, 0, 0);
    private int TombsToTrigger = 4;
    protected override AbilityInfo GetAbilityInfo()
    {
        return new AbilityInfo(200, 20, new List<AIUtils.AIState> { AIUtils.AIState.Attacking, AIUtils.AIState.CheckShoot, AIUtils.AIState.CheckDistSkills, AIUtils.AIState.CheckGeneralSkills });
    }
    private void Rez(List<Vector3> locations)
    {
        foreach (Vector3 pos in locations)
        {
            CharacterFactory.Instance.RezMob(Handler.Owner.Team, pos, spawndirection);
        }
    }
    protected override void AbilityAction()
    {
        List<Vector3> locations = CharacterTracker.Instance.GetRezPositions(OwnerLevelSys.GetLevel() - 2, Handler.Owner.Team);
        if (locations.Count > 0)
        {
            StartCoroutine(ShootAnim(locations));
            StartCoroutine(Reload());
            base.AbilityAction();
        }
    }
    private int GetRezMobCount()
    {
        if (Handler.Owner is MainPlayerBehaviour)
        {
            if (Handler.Owner is MirrorImageBehaviour)
            {
                return 1 + ((OwnerLevelSys.GetLevel() > 5) ? 1 : 0);
            }
            else
            {
                return OwnerLevelSys.GetLevel() - 2;
            }
        }
        else
        {
            return 1;
        }
    }
    private IEnumerator ShootAnim(List<Vector3> locations)
    {
        Handler.DisableOtherAbilities(this);
        movementAI.LockMovementAI(1.5f);
        Handler.Owner.animator.SetTrigger("Res");
        yield return new WaitForSeconds(0.4f);
        Rez(locations);
        Handler.ReenableOtherAbilities();
    }
    protected override bool InputPressed()
    {
        return PlayerInputRouter.Instance.UltPressed;
    }
    protected override void AICheck()
    {
        if (loaded && CharacterTracker.Instance.GetRezPoolCount(Handler.Owner.Team) >= TombsToTrigger)
            SetFinalAction();
    }
}
