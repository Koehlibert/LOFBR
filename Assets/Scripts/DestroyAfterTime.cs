using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class DestroyAfterTime : MonoBehaviour
{
    private float timer = 1.5f;
    private SphereCollider col;
    void Awake()
    {
        col = GetComponent<SphereCollider>();
        col.enabled = false;
        timer = 1.5f;
    }
    public void DelayedDestroy(float delay)
    {
        col.enabled = true;
        GetComponent<Rigidbody>().useGravity = true;
        Destroy(gameObject,delay);
    }
    public void DelayedDestroy()
    {
        DelayedDestroy(timer);
    }
}
