using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorAura : MonoBehaviour
{
    private CapsuleCollider aura;
    private List<GameObject> objectList;
    private PlayerController player;
    void Start()
    {
        aura = GetComponent<CapsuleCollider>();
        player = FindObjectOfType<PlayerController>();
    }
    void OnEnable()
    {
        objectList = new List<GameObject>();
    }
    void Update()
    {
        if (this.gameObject)
        {
           if(player.isActiveAndEnabled)
            {
                transform.position = player.transform.position;
            }
            else
            {
                transform.position = new Vector3(0,-10,0);
            } 
        }
        
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Friendly")&&(!other.GetComponent<TowerBehaviourFriendly>()))
        {
            objectList.Add(other.gameObject);
            other.gameObject.GetComponent<Health>().AddArmor(5+player.levelsys.getLevel());
        }
    }
    void OnTriggerExit(Collider other)
    {
       if (objectList.Contains(other.gameObject))
       {
            other.gameObject.GetComponent<Health>().AddArmor(-(5+player.levelsys.getLevel()));
            objectList.Remove(other.gameObject);
       } 
    }
    void OnDestroy()
    {
        objectList.RemoveAll(item => item == null);
        foreach (GameObject minion in objectList)
        {
            minion.gameObject.GetComponent<Health>().AddArmor(-(5+player.levelsys.getLevel()));
        }
    }
}
