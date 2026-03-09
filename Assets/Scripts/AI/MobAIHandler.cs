using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Search;
using UnityEngine;

public class MobAIHandler : AIHandler
{
    public override void Init(DamageableEntity owner, List<Ability> abilities, List<AIModule> aIModules)
    {
        base.Init(owner, abilities, aIModules);
        ShootRightBasic shooter = Owner.gameObject.AddComponent<ShootRightBasic>();
        shooter.IsInteractive = false;
        Abilities.Add(shooter);
    }
}