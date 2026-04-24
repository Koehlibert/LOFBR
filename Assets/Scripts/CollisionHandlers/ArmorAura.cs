using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using UnityEngine;

public class ArmorAura : Aura
{
    private float GetArmorToAdd()
    {
        if (Owner is MainPlayerBehaviour mainPlayerBehaviour)
        {
            if (Owner is MirrorImageBehaviour)
            {
                return 5 + 1 * mainPlayerBehaviour.Levelsys.GetLevel();
            }
            else
            {
                return 10 + 3 * mainPlayerBehaviour.Levelsys.GetLevel();
            }
        }
        else 
        {
            return 8;
        }
    }
    protected override bool AdditionalCheckToAdd(CharacterBehaviour characterBehaviour)
    {
        return characterBehaviour.Team == Owner.Team;
    }
    protected override StatusEffect CreateStatusEffect(CharacterBehaviour characterBehaviour)
    {
        ArmorEffect armorStatus = characterBehaviour.gameObject.AddComponent<ArmorEffect>();
        armorStatus.Init(GetArmorToAdd());
        return armorStatus;
    }
    protected override void HandleUpdate(KeyValuePair<CharacterBehaviour, StatusEffect> entry)
    {
        (entry.Value as ArmorEffect).UpdateAction(entry.Key, GetArmorToAdd());
    }
}