using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaDrainerBehaviour : MonoBehaviour
{
    private DamageableEntity Owner;
    MainPlayerBehaviour Enemy;
    private Vector3 offset = new(0,4,0);
    private LineRenderer lRend;
    private Mana OwnerManaSys;
    private Level OwnerLevelSys;
    public void Init(DamageableEntity owner)
    {
        Owner = owner;
        OwnerManaSys = (Owner as MainPlayerBehaviour).manasys;
        OwnerLevelSys = (Owner as MainPlayerBehaviour).Levelsys;
        Enemy = MasterScript.Instance.GetOpponentPlayer(Owner.Team);
        lRend = GetComponent<LineRenderer>();
    }
    void Update()
    {
        float distance = Vector3.Distance(Owner.transform.position, Enemy.transform.position);
            if (distance >= 20 || !Enemy.isActiveAndEnabled)
            {
                Disable();
            }
            lRend.SetPosition(0, Owner.transform.position + offset);
            lRend.SetPosition(1, Enemy.transform.position + offset);
            float actualDamage = Enemy.manasys.drainMana((15 + OwnerLevelSys.GetLevel() * 5) * Time.deltaTime);
            OwnerManaSys.gainMana(actualDamage);
            if (Enemy.GetHealth().TakeDamage(actualDamage * (0.05f + 0.05f * OwnerLevelSys.GetLevel())))
            {
                Enemy.Kill();
                Disable();
            }
    }
    public void Disable()
    {
        Destroy(this.gameObject);
    }
}
