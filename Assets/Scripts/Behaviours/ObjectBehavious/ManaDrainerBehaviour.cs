using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaDrainerBehaviour : MonoBehaviour
{
    private DamageableEntity Owner;
    MainPlayerBehaviour Enemy;
    private Vector3 offset = new(0, 4, 0);
    private LineRenderer lRend;
    private Mana OwnerManaSys;
    private Level OwnerLevelSys;
    private DamageInfo DamageInfo;
    public void Init(DamageableEntity owner, DamageInfo damageInfo)
    {
        Owner = owner;
        DamageInfo = damageInfo;
        OwnerManaSys = (Owner as MainPlayerBehaviour).manasys;
        OwnerLevelSys = (Owner as MainPlayerBehaviour).Levelsys;
        Enemy = CharacterTracker.Instance.GetOpponentPlayer(Owner.Team);
        lRend = GetComponent<LineRenderer>();
    }
    void Update()
    {
        float distance = Vector3.Distance(Owner.transform.position, Enemy.transform.position);
        if (distance >= 25 || !Enemy.isActiveAndEnabled)
        {
            Disable();
        }
        lRend.SetPosition(0, Owner.transform.position + offset);
        lRend.SetPosition(1, Enemy.transform.position + offset);
        float actualDamage = Enemy.manasys.drainMana(DamageInfo.damageValue * Time.deltaTime);
        OwnerManaSys.gainMana(actualDamage);
        if (Enemy.GetHealth().TakeDamage(actualDamage * CalculateDamagePercentage()))
        {
            Enemy.Kill();
            Disable();
        }
    }
    private float CalculateDamagePercentage()
    {
        if (OwnerLevelSys != null)
        {
            return 0.05f + 0.05f * OwnerLevelSys.GetLevel();
        }
        else
        {
            return 0.15f;
        }
    }
    public void Disable()
    {
        Destroy(this.gameObject);
    }
}
