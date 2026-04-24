using UnityEngine;

public class ArmorEffect : StatusEffect
{
    public override bool CanStack => false;
    private float AddedArmor;
    public void Init(float armorToAdd)
    {
        AddedArmor = armorToAdd;
    }
    public override void ActivateAction(CharacterBehaviour characterBehaviour)
    {
        characterBehaviour.hpsys.AddArmor(AddedArmor);
    }
    public override void DeactivateAction(CharacterBehaviour characterBehaviour)
    {
        characterBehaviour.hpsys.AddArmor(-AddedArmor);
    }
    public void UpdateAction(CharacterBehaviour characterBehaviour, float newArmor)
    {
        characterBehaviour.hpsys.AddArmor(-AddedArmor);
        characterBehaviour.hpsys.AddArmor(newArmor);
        AddedArmor = newArmor;
    }
}