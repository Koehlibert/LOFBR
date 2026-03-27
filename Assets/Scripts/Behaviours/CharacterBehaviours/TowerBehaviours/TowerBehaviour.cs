using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public abstract class TowerBehaviour : DamageableEntity
{
    protected float range;
    protected float lookRange;
    protected GameObject currentenemy;
    protected float cooldown;
    [SerializeField] private GameObject bullet;
    protected Vector3 offset;
    protected bool loaded;
    protected float reloadtime;
    protected GameObject bulletinstance;
    protected ClosestFinder closestFinder;
    private DamageInfo damageInfo;
    public override void Init()
    {
        base.Init();
        hpsys.Initialize(300, 0, 0, 20);
        closestFinder = new ClosestFinder(Team, this.gameObject);
        offset = new Vector3(0, 7, 0);
        range = 25;
        lookRange = 35;
        loaded = true;
        reloadtime = 1.25f;
        damageInfo = new DamageInfo(45, 0, this.Team);
    }
    protected virtual void Update()
    {
        currentenemy = closestFinder.FindClosestNoTower();
        if (currentenemy == null)
        {
            return;
        }
        float enemyDist = CombatUtils.GetDistance(this.gameObject, currentenemy);
        if (enemyDist <= lookRange)
        {
            transform.LookAt(new Vector3(currentenemy.transform.position.x, transform.position.y, currentenemy.transform.position.z));
            animator.SetFloat("speedPercent", 0);
            if ((enemyDist <= range) && loaded)
            {
                Attack();
            }
        }
    }
    void Attack()
    {
        if (loaded)
        {
            StartCoroutine("Shootanim");
            StartCoroutine("Reload");
        }
    }
    private IEnumerator Shootanim()
    {
        animator.Play("Throw", 0, 0f);
        yield return new WaitForSeconds(0.15f);
        bulletinstance = BulletFactory.Instance.CreateBullet(this, true, HumanBodyBones.RightHand);
        Rigidbody bulletrig = bulletinstance.GetComponent<Rigidbody>();
        bulletinstance.GetComponent<BulletBehaviour>().Shoot(damageInfo, 1500);
        bulletrig = null;
        StartCoroutine("Resetanim");
    }
    private IEnumerator Reload()
    {
        loaded = false;
        yield return new WaitForSeconds(reloadtime);
        loaded = true;
    }
    private IEnumerator Resetanim()
    {
        yield return new WaitForSeconds(0.25f);
        animator.Play("Default", 0, 0f);
    }
    protected override void Die()
    {
        if (Team == CombatUtils.Team.Enemy)
        {
            CharacterTracker.Instance.allEnemiesTowers.Remove(this.gameObject);
        }
        else
        {
            CharacterTracker.Instance.allFriendliesTowers.Remove(this.gameObject);
        }
        Destroy(this.gameObject);
    }
    public override Health GetHealth()
    {
        return hpsys;
    }
}
