using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class EnemyPlayerBehaviour : MainPlayerBehaviour
{
    public Image healthbar;
    public Image manaBar;
    public override void Init()
    {
        base.Init(2);
        healthbar.fillAmount = hpsys.healthDisplay();
        manaBar.fillAmount = manasys.getPercent();
    }
    public override CombatUtils.Team Team => CombatUtils.Team.Enemy;
    void FixedUpdate()
    {
        StackingHandler.PushAwayFromNearbyObjects(this.gameObject);
        UpdateBars();
    }
    private void UpdateBars()
    {
        float hpval = hpsys.healthDisplay();
        healthbar.fillAmount = Mathf.Lerp(healthbar.fillAmount, hpval, 5f * Time.deltaTime);
        manaBar.fillAmount = manasys.getPercent();
    }
}
