using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBehaviour : MonoBehaviour
{
    private DamageableEntity Owner;
    private Vector3 offset = new(0,4,0);
    public void Init(DamageableEntity owner)
    {
        Owner = owner;
    }
    void Update()
    {
        transform.position = Owner.transform.position + offset;
    }
}
