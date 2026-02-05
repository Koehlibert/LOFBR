using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaDrain : DamagingAbility
{
    private LineRenderer lRend;
    private EnemyPlayerBehaviour enemy;
    private float durationTime = 4f;
    private Vector3 offset = new Vector3(0, 2, 0);
    private bool isDraining;
    new void Start()
    {
        base.Start();
        lRend = GetComponent<LineRenderer>();
        loaded = true;
        enemy = FindAnyObjectByType<EnemyPlayerBehaviour>();
        lRend.enabled = false;
        isDraining = false;
    }
    protected override void Update()
    {
        if (isDraining)
        {
            float distance = Vector3.Distance(player.transform.position, enemy.transform.position);
            if (distance >= 20 || !enemy.isActiveAndEnabled)
            {
                isDraining = false;
                lRend.enabled = false;
            }
            lRend.SetPosition(0, player.transform.position + offset);
            lRend.SetPosition(1, enemy.transform.position + offset);
            float actualDamage = enemy.manasys.drainMana((5 + player.levelsys.getLevel() * 2) * Time.deltaTime);
            if (enemy.GetHealth().TakeDamage(actualDamage * (0.05f + 0.05f * player.levelsys.getLevel())))
            {
                enemy.Die();
            }
            player.manasys.gainMana(actualDamage);
        }
        if (InputPressed() && (loaded) && player.manasys.checkCost(manaCost) && enemy)
        {
            AbilityAction();
        }
    }
    private IEnumerator duration()
    {
        lRend.enabled = true;
        isDraining = true;
        yield return new WaitForSeconds(durationTime + player.levelsys.getLevel() * 0.8f);
        lRend.enabled = false;
        isDraining = false;
    }
    private void OnDisable()
    {
        isDraining = false;
    }
    protected override void AbilityAction()
    {
        float distance = Vector3.Distance(player.transform.position, enemy.transform.position);
        if (distance <= 20)
        {
            player.manasys.useMana(manaCost);
            StartCoroutine("reload");
            StartCoroutine("duration");
            reloader.shoot();
        }
    }
    protected override bool InputPressed()
    {
        return PlayerInputRouter.Instance.AlternativePressed;
    }
    protected override DamageInfo GetDamageValues()
    {
        return new DamageInfo(0, 0, CombatUtils.Team.Player, true, true);
    }
}
