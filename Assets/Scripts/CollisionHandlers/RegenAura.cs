using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegenAura : Aura
{
    public override void Init(DamageableEntity owner)
    {
        base.Init(owner);
        Destroy(this.gameObject, 6f);
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
    protected override bool AdditionalCheckToAdd(CharacterBehaviour characterBehaviour)
    {
        return characterBehaviour.Team == Owner.Team;
    }
    protected override StatusEffect CreateStatusEffect(CharacterBehaviour characterBehaviour)
    {
        SuperRegenEffect superRegenStatus = characterBehaviour.gameObject.AddComponent<SuperRegenEffect>();
        superRegenStatus.Init(GetBuffValue());
        return superRegenStatus;
    }
    protected override void HandleUpdate(KeyValuePair<CharacterBehaviour, StatusEffect> entry)
    {
        (entry.Value as ArmorEffect).UpdateAction(entry.Key, GetBuffValue());
    }
}
