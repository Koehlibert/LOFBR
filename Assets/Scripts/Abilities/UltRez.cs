using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltRez : Ability
{
    private Quaternion spawndirection = new Quaternion(0, 0, 0, 0);
    protected override AbilityInfo GetAbilityInfo()
    {
        return new AbilityInfo(200, 20, new List<AIUtils.AIState> { AIUtils.AIState.Attacking, AIUtils.AIState.CheckShoot, AIUtils.AIState.CheckDistSkills, AIUtils.AIState.CheckGeneralSkills });
    }
    private void Rez(List<Vector3> locations)
    {
        foreach (Vector3 pos in locations)
        {
            CharacterFactory.Instance.CreateTeamMob(Handler.Owner.Team, pos, spawndirection);
        }
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        reloader = HUD.Instance.GetReload(HUD.Instance.UltReloader);
    }
    protected override void AbilityAction()
    {
        List<Vector3> locations = MasterScript.Instance.GetRezPositions(OwnerLevelSys.GetLevel() - 2);
        if (locations.Count > 0)
        {
            StartCoroutine("Reload");
            Rez(locations);
            base.AbilityAction();
        }
    }
    protected override bool InputPressed()
    {
        return PlayerInputRouter.Instance.UltPressed;
    }
}
