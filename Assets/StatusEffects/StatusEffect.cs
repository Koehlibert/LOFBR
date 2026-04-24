using UnityEngine;

public abstract class StatusEffect : MonoBehaviour
{
    public abstract bool CanStack { get; }
    public abstract void ActivateAction(CharacterBehaviour characterBehaviour);
    public abstract void DeactivateAction(CharacterBehaviour characterBehaviour);
    public virtual void UpdateAction(CharacterBehaviour characterBehaviour)
    {
    }
}