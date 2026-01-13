using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shock : MonoBehaviour
{
    public float maxRange = 1f;
    private CapsuleCollider col;
    private Vector3 fullV;
    // Start is called before the first frame update
    void Start()
    {
        fullV = new Vector3(14,0,14);
        gameObject.transform.localScale = new Vector3(0,0.2f,0);
        col = GetComponent<CapsuleCollider>();
    }
    // Update is called once per frame
    void Update()
    {
        if (col.radius<=maxRange)
        {
            col.radius+=1.5f*Time.deltaTime;
            gameObject.transform.localScale = fullV*(col.radius/maxRange);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
