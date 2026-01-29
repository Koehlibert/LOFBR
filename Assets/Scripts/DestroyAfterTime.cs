using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class DestroyAfterTime : MonoBehaviour
{
    private SphereCollider col;
    void Awake()
    {
        col = GetComponent<SphereCollider>();
        col.enabled = false;
    }
    public void DelayedDestroy()
    {
        col.enabled = true;
        GetComponent<Rigidbody>().useGravity = true;
        Destroy(gameObject,1.5f);
    }
}
