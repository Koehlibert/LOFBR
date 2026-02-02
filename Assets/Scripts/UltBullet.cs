using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class UltBullet : MonoBehaviour
{
    public float count;
    public abstract CombatUtils.Team Team { get; }
    protected GameObject closestCurrentEnemy;
    private GameObject target;
    protected ClosestFinder closestFinder;
    protected virtual void Start()
    {
        closestFinder = new ClosestFinder(CombatUtils.Team.Player, this.gameObject);
        count = 5f;
    }
    protected void Update()
    {
        if (count <= 0)
        {
            Destroy(this.gameObject);
            return;
        }
        if (!target)
        {
            target = closestFinder.FindClosestNoTower(false);
        }  
        if(!target)
        {
            Destroy(this.gameObject);
            return;
        }   
        transform.position = transform.position + 30 * Time.deltaTime * (target.transform.position - transform.position).normalized;
    }
}
