using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTimeHeal : MonoBehaviour
{
    private SphereCollider col;
    private HealingBullet behaviour;
    void Start()
    {
        col = GetComponent<SphereCollider>();
        col.enabled = false;
        behaviour = GetComponent<HealingBullet>();
        behaviour.enabled = false;
    }
    void Awake()
    {
        col = GetComponent<SphereCollider>();
        col.enabled = false;
    }
    void Update()
    {
        
    }
    public void DelayedDestroy()
    {
        col.enabled = true;
        behaviour.enabled = true;
        GetComponent<Rigidbody>().useGravity = true;
        Destroy(gameObject,1.2f);
    }
}
