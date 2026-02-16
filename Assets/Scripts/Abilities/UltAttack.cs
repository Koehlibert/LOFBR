using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltAttack : ShootBasic
{
    public GameObject ultBullet;
    private ShootRightBasic ShootRight;
    private ShootLeftBasic ShootLeft;
    protected override HumanBodyBones Bone => HumanBodyBones.LeftLowerLeg;
    new void Start()
    {
        offset = new Vector3(0, 1, 0);
        loaded = true;
        reloadtime = 15f;
        manaCost = 250;
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        reloader = HUD.Instance.GetReload(HUD.Instance.UltReloader);
    }
    private IEnumerator Firstbullet()
    {
        yield return new WaitForSeconds(.2f);
        ShootLeft = player.GetComponent<ShootLeftBasic>();
        ShootRight = player.GetComponent<ShootRightBasic>();
    }
    private IEnumerator Shootanim()
    {
        ShootLeft.enabled = false;
        ShootRight.enabled = false;
        StartCoroutine(player.aIHandler.movementAI.LockMovement(1.6f));
        player.animator.Play("Backflip", 0, 0f);
        yield return new WaitForSeconds(0.6f);
        bulletinstance = CreateBullet();
        bulletinstance.GetComponent<BulletBehaviour>().Shoot(GetDamageValues());
        StartCoroutine("Resetanim");
    }
    protected override IEnumerator Reload()
    {
        loaded = false;
        yield return new WaitForSeconds(reloadtime);
        loaded = true;
    }
    private IEnumerator Resetanim()
    {
        yield return new WaitForSeconds(1f);
        player.animator.Play("Default", 0, 0f);
        ShootLeft.enabled = true;
        ShootRight.enabled = true;
    }
    protected override bool InputPressed()
    {
        return PlayerInputRouter.Instance.UltPressed;
    }
    protected override DamageInfo GetDamageValues()
    {
        return new DamageInfo(50+(player.levelsys.getLevel()-5)*6.5f, 0, CombatUtils.Team.Player, true, true);
    }
    protected override GameObject CreateBullet()
    {
        return BulletFactory.Instance.CreateUltBullet(player, false, Bone);
    }
}
