using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTimeTower : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject,1.2f);
        GetComponent<SphereCollider>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
