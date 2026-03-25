using System.Collections.Generic;
using UnityEngine;
using System;
using NUnit.Framework;

public class ArmorAuraCollisionHandler : CollisionHandler
{
    private List<ObjectWithAddedArmor> ArmoredAllies;
    public override void Init(DamageableEntity owner)
    {
        base.Init(owner);
        ArmoredAllies = new List<ObjectWithAddedArmor>();
    }
    protected override void HandleEnduringDamage(Collider collider)
    {
    }
    protected override void HandleDamageCollision(Collider collider)
    {
        DamageableEntity damageableEntity = collider.gameObject.GetComponent<DamageableEntity>();
        if (damageableEntity != null && damageableEntity.Team == Owner.Team)
        {
            AddArmorToTarget(damageableEntity);
        }
    }
    void OnTriggerExit(Collider collider)
    {
        var item = ArmoredAllies.Find(x => x.ArmoredEntity = collider.gameObject.GetComponent<DamageableEntity>());
        if (item != null)
        {
            RemoveArmorFromTarget(item);
        }
    }
    void Update()
    {
        this.transform.SetPositionAndRotation(Owner.transform.position + new Vector3(0f,2f,0f), Owner.transform.rotation);
        ArmoredAllies.RemoveAll(item => item.ArmoredEntity == null);
    }
    void OnDestroy()
    {
        ArmoredAllies.RemoveAll(item => item == null);
        foreach (ObjectWithAddedArmor objectWithAddedArmor in ArmoredAllies)
        {
            RemoveArmorFromTarget(objectWithAddedArmor);
        }
    }
    private void AddArmorToTarget(DamageableEntity damageableEntity)
    {
        float addedArmor = GetArmorToAdd();
        damageableEntity.GetComponent<Health>().AddArmor(addedArmor);
        ArmoredAllies.Add(new ObjectWithAddedArmor(damageableEntity, addedArmor));
    }
    private void RemoveArmorFromTarget(ObjectWithAddedArmor objectWithAddedArmor)
    {
        objectWithAddedArmor.ArmoredEntity.GetComponent<Health>().AddArmor(-objectWithAddedArmor.AddedArmor);
        ArmoredAllies.Remove(objectWithAddedArmor);
    }
    private float GetArmorToAdd()
    {
        if (Owner is MainPlayerBehaviour mainPlayerBehaviour)
        {
            return 10 + 3 * mainPlayerBehaviour.Levelsys.GetLevel();
        }
        else return 10;
    }
}
public class ObjectWithAddedArmor
{
    public DamageableEntity ArmoredEntity;
    public float AddedArmor;
    public ObjectWithAddedArmor(DamageableEntity armoredEntity, float addedArmor)
    {
        ArmoredEntity = armoredEntity;
        AddedArmor = addedArmor;
    }
}