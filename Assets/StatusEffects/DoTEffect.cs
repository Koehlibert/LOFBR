using UnityEngine;

public class DoTEffect : StatusEffectTimer
{
    public override bool CanStack => false;
    private float DoTDamage;
    private Health OwnerHPSys;
    public void Init(float timer, float poisonDamage)
    {
        base.Init(timer);
        DoTDamage = poisonDamage;
    }
    public override void ActivateAction(CharacterBehaviour characterBehaviour)
    {
        OwnerHPSys = characterBehaviour.hpsys;
    }
    public override void DeactivateAction(CharacterBehaviour characterBehaviour)
    {
    }
    void Update()
    {
        OwnerHPSys.TakeDamage(DoTDamage * Time.deltaTime);
    }
    public void UpdateAction(CharacterBehaviour characterBehaviour, float newPoisonValue)
    {
        DoTDamage = newPoisonValue;
    }
}