using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseShield : Ability
{
    public GameObject shield;
    private GameObject shieldInstance;
    protected override AbilityInfo GetAbilityInfo()
    {
        return new AbilityInfo(50, 12, new List<AIUtils.AIState> { AIUtils.AIState.Attacking, AIUtils.AIState.CheckShoot });
    }
    private IEnumerator DestroyShield()
    {
        Handler.Owner.GetHealth().AddArmor(100);
        if (IsInteractive)
        {
            (Handler.Owner as PlayerController).DisableDamageFlash();
        }
        yield return new WaitForSeconds(1.5f);
        Handler.Owner.GetHealth().AddArmor(-100);
        if (IsInteractive)
        {
            (Handler.Owner as PlayerController).EnableDamageFlash();
        }
        GameObject.Destroy(shieldInstance);
    }

    protected override void AbilityAction()
    {
        shieldInstance = BulletFactory.Instance.CreateShield(Handler.Owner);
        StartCoroutine("reload");
        StartCoroutine("DestroyShield");
        reloader.shoot();
        OwnerManaSys.useMana(manaCost);
    }
    protected override bool InputPressed()
    {
        return PlayerInputRouter.Instance.SkillPressed;
    }
    /* protected override void AICheck()
    {
        if (loaded)
        {
            List<GameObject> closest3Enemies = Handler.ClosestFinder.FindNClosest(3, true);
            (bool ShouldShock, Vector3 ShockPoint) = ExistsPointWithinRadius(closest3Enemies, ShockRadiusToCheck);
            if (ShouldShock)
            {
                IsShocking = true;
                Debug.Log("Shield");
                inDistanceTracker = Handler.ClosestFinder.StartTrackingDist(ShockRadiusToCheck, true);
                Handler.movementAI.MovementState = AIUtils.MovementState.IsGoingToPlace;
                Handler.movementAI.SetMovementTarget(ShockPoint);
                Handler.DisableOtherAbilities(this);
                Handler.movementAI.OnTargetReached += AbilityAction;
            }
        }
    } */
}
