using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryColliderBehaviour : MonoBehaviour
{
    private DamageableEntity Owner;
    private Vector3 offset = new Vector3(0, 3, 0);
    public void Init(DamageableEntity owner)
    {
        Owner = owner;
    }
    void Update()
    {
        transform.position = Owner.transform.position + offset + Owner.transform.forward * 2;
        transform.rotation = Owner.transform.rotation;
    }
    void OnTriggerEnter(Collider col)
    {
        Damage damageComponent = col.gameObject.GetComponent<Damage>();
        if (damageComponent != null)
        {
            if (CombatUtils.CanDamage(damageComponent, Owner))
            {
                Rigidbody rb = col.GetComponent<Rigidbody>();
                rb.linearVelocity = Vector3.zero;
                rb.AddForce(Owner.transform.forward * 2000);
                col.gameObject.GetComponent<Damage>().sourceTeam = CombatUtils.Team.Player;
                col.gameObject.GetComponent<Damage>().givesXP = true;
            }
        }
    }
}
