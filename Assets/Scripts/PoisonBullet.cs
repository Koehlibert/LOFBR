using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonBullet : MonoBehaviour
{
    private Rigidbody rb;
    private EnemyPlayerBehaviour enemy;
    private float speed = 25;
    private float focusDistance = 17.5f;
    private float rotationSpeed = 2f;
    private bool isFollowingTarget;
    private bool faceTarget = true;
    private Vector3 tempVector;
    private GameObject target;
    private ClosestFinder closestFinder;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        enemy = FindAnyObjectByType<EnemyPlayerBehaviour>();
        closestFinder = new ClosestFinder(enemy, this.gameObject);
    }
    void Update()
    {
        if (!target)
        {
            target = closestFinder.FindClosestEnemy();
        }
        if (target)
        {
            if (Vector3.Distance(transform.position, target.transform.position) < focusDistance)
            {
                isFollowingTarget = true;
            }
            else
            {
                isFollowingTarget = false;
            }
            Vector3 targetDirection = target.transform.position - transform.position;        
            if (faceTarget)
            {
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, rotationSpeed * Time.deltaTime, 0.0F);
                MoveForward(Time.deltaTime);
                if (isFollowingTarget)
                {
                    transform.rotation = Quaternion.LookRotation(newDirection);
                }
            }
            else
            {            
                if (isFollowingTarget)
                {
                    tempVector = targetDirection.normalized;
                    transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
                }
                else
                {
                    MoveForward(Time.deltaTime);
                }
            }
        }
        else
        {
            MoveForward(Time.deltaTime);
        }
    }
    public GameObject[] findClosestEnemy(List<GameObject> allEnemies)
    {
        GameObject[] closeEnemies = new GameObject[2];
        if (allEnemies.Count != 0)
        {
            float secondclosestDistance = Mathf.Infinity;
            float closestDistance = Mathf.Infinity;
            foreach (GameObject currenemy in allEnemies)
            {
                if (!currenemy)
                {
                    continue;
                }
                float distanceToEnemy = Vector3.Distance(currenemy.transform.position,this.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    secondclosestDistance = closestDistance;
                    closestDistance = distanceToEnemy;
                    closeEnemies[1] = closeEnemies[0];
                    closeEnemies[0] = currenemy;
                }
            }
        }
        return closeEnemies;
    }
    private void MoveForward (float rate)
    {
        transform.Translate(Vector3.forward * rate * speed, Space.Self);
        Vector3 temp = transform.position;
        temp.y = 0.4f;
        transform.position = temp;
    }
}