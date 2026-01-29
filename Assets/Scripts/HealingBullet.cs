using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingBullet : MonoBehaviour
{
    private Rigidbody rb;
    private float speed = 30;
    private float focusDistance = 17.5f;
    float rotationSpeed = 2.5f;
    private bool isFollowingTarget;
    private bool faceTarget = true;
    private Vector3 tempVector;
    private GameObject target;
    List<GameObject> allFriendlies;
    List<GameObject> fullHPFriendlies;
    private ClosestFinder closestFinder;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        allFriendlies = MasterScript.Instance.allFriendlies;
        fullHPFriendlies = new List<GameObject>();
        closestFinder = new ClosestFinder(CombatUtils.Team.Player, this.gameObject);
    }
    void Update()
    {
        if(!target)
        {
            target = closestFinder.FindClosestHurtFriendly();
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
    private void MoveForward (float rate)
    {
        transform.Translate(Vector3.forward * rate * speed, Space.Self);
        Vector3 temp = transform.position;
        temp.y = 0.4f;
        transform.position = temp;
    }
}
