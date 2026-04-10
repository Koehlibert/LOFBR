using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegenAura : CollisionHandler
{
    private List<DamageableEntity> RegeningObjectList;
    public override void Init(DamageableEntity owner)
    {
        base.Init(owner);
        Destroy(this.gameObject,6f);
        RegeningObjectList = new List<DamageableEntity>();
        owner.GetHealth().ActivateSuperRegen(GetBuffValue());
    }
    void Update()
    {
        this.transform.SetPositionAndRotation(Owner.transform.position + new Vector3(0f,2f,0f), Owner.transform.rotation);
    }
    protected override void HandleEnduringDamage(Collider collider)
    {
    }
    protected override void HandleDamageCollision(Collider collider)
    {
        DamageableEntity damageableEntity = collider.gameObject.GetComponent<DamageableEntity>();
        if (damageableEntity != null && damageableEntity.Team == Owner.Team && !RegeningObjectList.Contains(damageableEntity))
        {
            ActivateSuperRegen(damageableEntity);
        }
    }
    private void ActivateSuperRegen(DamageableEntity damageableEntity)
    {
        damageableEntity.GetComponent<Health>().ActivateSuperRegen(GetBuffValue());
        RegeningObjectList.Add(damageableEntity);
    }
    private void DeactivateSuperRegen(DamageableEntity damageableEntity)
    {
        damageableEntity.GetComponent<Health>().DeactivateSuperRegen();
        RegeningObjectList.Remove(damageableEntity);
    }
    void OnTriggerExit(Collider collider)
    {
        var item = RegeningObjectList.Find(x => x = collider.gameObject.GetComponent<DamageableEntity>());
        if (item != null)
        {   
            DeactivateSuperRegen(item.GetComponent<DamageableEntity>());
        }
    }
    void OnDestroy()
    {
        RegeningObjectList.RemoveAll(item => item == null);
        foreach (DamageableEntity character in RegeningObjectList)
        {
            character.GetComponent<Health>().DeactivateSuperRegen();
        }
        Owner.GetHealth().DeactivateSuperRegen();
    }
    public float GetBuffValue()
    {
        if (Owner is MainPlayerBehaviour mainPlayerBehaviour)
        {
            if (Owner is MirrorImageBehaviour)
            {
                return mainPlayerBehaviour.Levelsys.GetLevel()*2 + 5;
            }
            else
            {
                return mainPlayerBehaviour.Levelsys.GetLevel()*3 + 10;
            }
        }
        else return 10;
    }
}
