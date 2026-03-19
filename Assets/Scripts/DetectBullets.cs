using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectBullets : MonoBehaviour
{
    private DamageableEntity Owner;
    private List<GameObject> objectList;
    private int NBulletsToTrigger;
    public event Action BulletsDetected;
    public void Init(DamageableEntity owner, int nBulletsToTrigger)
    {
        Owner = owner;
        NBulletsToTrigger = nBulletsToTrigger;
    }
    void Update()
    {
        transform.position = Owner.transform.position;
        objectList.RemoveAll(item => item == null);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet") || other.gameObject.CompareTag("BulletPlayer"))
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
    protected virtual void SetupCollisionHandler()
    {
        //DamageCollisionHandler handler = new DamageCollisionHandler(Owner);
    }
}
