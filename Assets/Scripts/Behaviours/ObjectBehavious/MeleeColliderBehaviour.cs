using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeColliderBehaviour : MonoBehaviour
{
    private DamageableEntity Owner;
    private Vector3 offset = new Vector3(0, 1.5f,0);
    public void Init(DamageableEntity owner)
    {
        Owner = owner;
    }
    void Update()
    {
        transform.position = Owner.transform.position + Owner.transform.forward*1.25f + offset;
    }
}
