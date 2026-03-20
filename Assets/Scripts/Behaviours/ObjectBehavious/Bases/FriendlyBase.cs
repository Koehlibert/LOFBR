using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;
public class FriendlyBase : Base
{
    public override CombatUtils.Team Team => CombatUtils.Team.Enemy;
}
