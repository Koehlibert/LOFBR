using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTimeTower : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject,1.75f);
        GetComponent<SphereCollider>().enabled = true;
    }
}
