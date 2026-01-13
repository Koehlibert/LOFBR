using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkShockWave : MonoBehaviour
{
    public List<GameObject> objectList;
    private EnemyPlayerBehaviour player;
    public bool manyEnemies;
    void Start()
    {
        objectList = new List<GameObject>();
        player = FindObjectOfType<EnemyPlayerBehaviour>();
        manyEnemies = false;
    }

    // Update is called once per frame
    void Update()
    {
        objectList.RemoveAll(item => item == null);
        if (objectList.Count >= 2)
        {
            if(player.isActiveAndEnabled)
            {
                Vector3 center = new Vector3(0, 0, 0);
                float count  = 0;
                foreach (GameObject enemy in objectList){
                center += enemy.transform.position;
                count++;
                }
                center = center / count;
                manyEnemies = true;
                player.GoShock(center);   
            }
        }
        else
        {
            manyEnemies = false;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.CompareTag("Player")&&(!other.GetComponent<TowerBehaviourFriendly>()))||other.gameObject.CompareTag("Friendly")&&(!other.GetComponent<TowerBehaviourFriendly>()))
        {
            objectList.Add(other.gameObject);
        }
    }
    void OnTriggerExit(Collider other)
    {
       if ((objectList.Contains(other.gameObject))&&(!other.GetComponent<TowerBehaviourFriendly>()))
       {
           objectList.Remove(other.gameObject);
       } 
    }
}
