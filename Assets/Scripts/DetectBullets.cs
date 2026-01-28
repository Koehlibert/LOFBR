using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectBullets : MonoBehaviour
{
    private EnemyPlayerBehaviour player;
    private List<GameObject> objectList;
    void Start()
    {
        player = FindAnyObjectByType<EnemyPlayerBehaviour>();
        objectList = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position;
        objectList.RemoveAll(item => item == null);
        if (objectList.Count >= 2)
        {
            if(player.isActiveAndEnabled)
            {
                player.UseShield();   
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.CompareTag("Bullet"))||(other.gameObject.CompareTag("BulletPlayer")))
        {
            objectList.Add(other.gameObject);
        }
    }
    void OnTriggerExit(Collider other)
    {
       if (objectList.Contains(other.gameObject))
       {
           objectList.Remove(other.gameObject);
       } 
    }
}
