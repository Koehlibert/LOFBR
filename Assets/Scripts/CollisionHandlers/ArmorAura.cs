using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorAura : CollisionHandler
{
    private List<ObjectWithAddedArmor> ArmoredAllies;
    public override void Init(DamageableEntity owner)
    {
        Owner = owner;
        base.Init(owner);
        ArmoredAllies = new List<ObjectWithAddedArmor>();
    }
    void Update()
    {
        transform.position = Owner.transform.position;
        this.transform.SetPositionAndRotation(Owner.transform.position + new Vector3(0f,2f,0f), Owner.transform.rotation);
        ArmoredAllies.RemoveAll(item => item.ArmoredEntity == null);
    }
    protected override void HandleEnduringDamage(Collider collider)
    {
    }
    protected override void HandleDamageCollision(Collider collider)
    {
        DamageableEntity damageableEntity = collider.gameObject.GetComponent<DamageableEntity>();
        var item = ArmoredAllies.Find(x => x.ArmoredEntity == damageableEntity);
        if (damageableEntity != null && damageableEntity.Team == Owner.Team && item == null)
        {
            AddArmorToTarget(damageableEntity);
        }
    }
    void OnTriggerExit(Collider collider)
    {
        var item = ArmoredAllies.Find(x => x.ArmoredEntity == collider.gameObject.GetComponent<DamageableEntity>());
        if (item != null)
        {
            RemoveArmorFromTarget(item);
        }
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
        damageableEntity.hpsys.AddArmor(addedArmor);
        ArmoredAllies.Add(new ObjectWithAddedArmor(damageableEntity, addedArmor));
    }
    private void RemoveArmorFromTarget(ObjectWithAddedArmor objectWithAddedArmor)
    {
        objectWithAddedArmor.ArmoredEntity.hpsys.AddArmor(-objectWithAddedArmor.AddedArmor);
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