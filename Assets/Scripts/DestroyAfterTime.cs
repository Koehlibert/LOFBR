using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    private SphereCollider col;
    void Start()
    {
        col = GetComponent<SphereCollider>();
        col.enabled = false;
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
        GetComponent<Rigidbody>().useGravity = true;
        Destroy(gameObject,1.2f);
    }
}
