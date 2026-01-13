using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    private float damage;
    private float poison;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetDamage(float damageValue, float poisonValue)
    {
        damage = damageValue;
        poison = poisonValue;
    }
    public void SetDamage(float damageValue)
    {
        damage = damageValue;
    }
    public (float poisonValue, float damageValue) GetDamage()
    {
        return (poison, damage);
    }
    public (float poisonValue, float damageValue) GetDamage(float val)
    {
        return (poison*val, damage*val);
    }
}
