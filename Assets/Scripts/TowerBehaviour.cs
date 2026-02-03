using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TowerBehaviour : DamageableEntity
{
    protected float range;
    protected GameObject currentenemy;
    protected float cooldown;
    [SerializeField] private GameObject bullet;
    protected Vector3 offset;
    protected bool loaded;
    protected float reloadtime;
    protected GameObject bulletinstance;
    protected ClosestFinder closestFinder;
    private DamageInfo damageInfo;
    protected override void Start()
    {
        base.Start();
        hpsys.Initialize(300, 0, 0, 20);
        closestFinder = new ClosestFinder(Team, this.gameObject);
        offset = new Vector3(0, 7, 0);
        range = 25;
        loaded = true;
        reloadtime = 1.25f;
        damageInfo = new DamageInfo(60, 0, this.Team);
    }
    protected virtual void Update()
    {
        currentenemy = closestFinder?.FindClosestNoTower();
        animator.SetFloat("speedPercent", 0);
        if (CombatUtils.InRange(this.gameObject, currentenemy, range) && (loaded))
        {
            animator.Play("Throw", 0, 0f);
            Attack(currentenemy);
        }
    }
    void Attack(GameObject target)
    {
        Vector3 dir = target.transform.position - transform.position;
        dir.y = 0;
        transform.rotation = Quaternion.LookRotation(dir);
        bulletinstance = Instantiate(bullet, transform.position + offset + 1.5f * (transform.position - target.transform.position).normalized, transform.rotation);
        Rigidbody bulletrig = bulletinstance.GetComponent<Rigidbody>();
        bulletinstance.GetComponent<Damage>().SetProperties(damageInfo);
        bulletrig.AddForce(gameObject.transform.forward * 1750);
        bulletinstance.GetComponent<DestroyAfterTime>().DelayedDestroy(2);
        StartCoroutine("Reload");
    }
    private IEnumerator Reload()
    {
        loaded = false;
        yield return new WaitForSeconds(reloadtime);
        loaded = true;
    }
    public override CombatUtils.Team Team => CombatUtils.Team.Player;
    public override void Die()
    {
        if (Team == CombatUtils.Team.Enemy)
        {
            MasterScript.Instance.allEnemiesTowers.Remove(this.gameObject);
        }
        else
        {
            MasterScript.Instance.allFriendliesTowers.Remove(this.gameObject);
        }
        Destroy(this.gameObject);
    }
    public override Health GetHealth()
    {
        return hpsys;
    }
}
