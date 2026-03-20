using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public DamageableEntity owner;
    public void SetOwner(DamageableEntity owner)
    {
        this.owner = owner;
    }
    void Update()
    {
        this.transform.SetPositionAndRotation(owner.transform.position + new Vector3(0f,2f,0f), owner.transform.rotation);
    }
}
