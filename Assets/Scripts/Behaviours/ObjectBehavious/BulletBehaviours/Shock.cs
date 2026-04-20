using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shock : MonoBehaviour
{
    public float maxRange = 1f;
    private CapsuleCollider col;
    private Vector3 fullV;
    void Start()
    {
        fullV = new Vector3(14,0,14);
        gameObject.transform.localScale = new Vector3(0,0.2f,0);
        col = GetComponent<CapsuleCollider>();
    }
    void Update()
    {
        if (col.radius<=maxRange)
        {
            col.radius+=1.35f*Time.deltaTime;
            gameObject.transform.localScale = fullV*(col.radius/maxRange);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
