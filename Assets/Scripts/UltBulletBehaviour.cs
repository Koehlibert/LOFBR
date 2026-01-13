using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UltBulletBehaviour : MonoBehaviour
{
    public int count;
    private MasterScript master;
    private GameObject closestCurrentEnemy;
    public GameObject target;
    void Start()
    {
        count = 8;
        master = FindObjectOfType<MasterScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (count <= 0)
        {
            Destroy(this.gameObject);
            return;
        }
        if (!target)
        {
            target = findClosestEnemy(master.allEnemies)[0];
        }  
        if(!target)
        {
            Destroy(this.gameObject);
            return;
        }   
        transform.position = transform.position + (target.transform.position - transform.position).normalized*Time.deltaTime*30;
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
}
