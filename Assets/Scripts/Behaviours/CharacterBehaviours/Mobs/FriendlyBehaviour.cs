using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System;
public class FriendlyBehaviour : MobBehaviour
{
    public override CombatUtils.Team Team => CombatUtils.Team.Player;
}