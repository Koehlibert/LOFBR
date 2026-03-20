using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorAura : MonoBehaviour
{
    private CapsuleCollider aura;
    private List<GameObject> objectList;
    private DamageableEntity Owner;
    void Start()
    {
        aura = GetComponent<CapsuleCollider>();
        Owner = FindAnyObjectByType<PlayerController>();
    }
    void OnEnable()
    {
        objectList = new List<GameObject>();
    }
    void Update()
    {
        if (this.gameObject)
        {
           if(Owner.isActiveAndEnabled)
            {
                transform.position = Owner.transform.position;
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
            //other.gameObject.GetComponent<Health>().AddArmor(5+OwnerLevelSys.GetLevel());
        }
    }
    void OnTriggerExit(Collider other)
    {
       if (objectList.Contains(other.gameObject))
       {
            //other.gameObject.GetComponent<Health>().AddArmor(-(5+OwnerLevelSys.GetLevel()));
            objectList.Remove(other.gameObject);
       } 
    }
    void OnDestroy()
    {
        objectList.RemoveAll(item => item == null);
        foreach (GameObject minion in objectList)
        {
            //minion.gameObject.GetComponent<Health>().AddArmor(-(5+OwnerLevelSys.GetLevel()));
        }
    }
}
