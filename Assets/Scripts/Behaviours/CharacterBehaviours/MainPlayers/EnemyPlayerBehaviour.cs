using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class EnemyPlayerBehaviour : MainPlayerBehaviour
{    
    public override void Init()
    {
        this.Team = CombatUtils.Team.Enemy;
        base.Init(1);
        healthbar.fillAmount = hpsys.healthDisplay();
        manaBar.fillAmount = manasys.getPercent();
    }
}
