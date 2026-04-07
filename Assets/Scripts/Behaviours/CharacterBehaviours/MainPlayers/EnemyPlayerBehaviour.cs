using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class EnemyPlayerBehaviour : MainPlayerBehaviour
{
    [SerializeField] Image healthbar;
    [SerializeField] Image manaBar;
    [SerializeField] Outline healthbarOutline;
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
    public override void MarkHealthbar()
    {
        IsMarked = true;
        ResetMarked = false;
        ChangeOutlineAlpha(0.5f);
    }
    private void ChangeOutlineAlpha(float alpha)
    {
        var tmp = healthbarOutline.effectColor;
        tmp.a = alpha;
        healthbarOutline.effectColor = tmp;
    }
    public override void MarkThisForDeath()
    {
        base.MarkThisForDeath();
        IsMarked = false;
        ChangeOutlineAlpha(1);
    }
    protected override IEnumerator ResetMark()
    {
        yield return base.ResetMark();
    }
}
