using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class EnemyPlayerBehaviour : MainPlayerBehaviour
{
    [SerializeField] Image manaBar;
    
    public override void Init()
    {
        this.Team = CombatUtils.Team.Enemy;
        base.Init(3);
        healthbar.fillAmount = hpsys.healthDisplay();
        manaBar.fillAmount = manasys.getPercent();
    }
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
        if (ResetMarked)
        {
            ChangeOutlineAlpha(0);
            ResetMarked = false;
        }
        if (IsMarked)
        {
            IsMarked = false;
            ResetMarked = true;
        }
    }
}
