using UnityEngine;

public class SuperRegenEffect : StatusEffect
{
    public override bool CanStack => false;
    private float SuperRegenValue;
    private Health OwnerHPSys;
    public void Init(float regenToGive)
    {
        SuperRegenValue = regenToGive;
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
        OwnerHPSys.superRegen(SuperRegenValue);
    }
    public void UpdateAction(CharacterBehaviour characterBehaviour, float newRegenValue)
    {
        SuperRegenValue = newRegenValue;
    }
}