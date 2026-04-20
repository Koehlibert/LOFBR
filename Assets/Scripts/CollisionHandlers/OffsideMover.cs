using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class OffsideMover : CollisionHandler
{
    private List<CharacterBehaviour> ObjectsToMove;
    protected Vector3 MoveDirection = new Vector3(0f, 0f, 10f);
    public void Init(DamageableEntity owner, DamageInfo damageInfo)
    {
        Init(owner);
        this.transform.position = new Vector3(0, 0, owner.transform.position.z);
        Destroy(this.gameObject,2.5f);
        ObjectsToMove = new List<CharacterBehaviour>();
        if (owner.Team == CombatUtils.Team.Enemy)
        {
            MoveDirection.z *= -1;
        }
        gameObject.AddComponent<Damage>().SetProperties(damageInfo);
    }
    void Update()
    {
        Vector3 actualMovement = MoveDirection * Time.deltaTime;
        transform.Translate(actualMovement);
        if (Mathf.Abs(transform.position.z - MasterScript.Instance.GetOpponentSpawnZ(Owner.Team)) < 2)
        {
            Destroy(this.gameObject);
        }
        foreach (CharacterBehaviour characterBehaviour in ObjectsToMove)
        {
            if (characterBehaviour != null)
            characterBehaviour.transform.position += actualMovement;
        }
    }
    protected override void HandleEnduringDamage(Collider collider)
    {
    }
    protected override void HandleDamageCollision(Collider collider)
    {
        CharacterBehaviour characterBehaviour = collider.gameObject.GetComponentInParent<CharacterBehaviour>();
        if (characterBehaviour != null && CombatUtils.CanDamage(Owner.Team, characterBehaviour.Team) && !ObjectsToMove.Contains(characterBehaviour) && characterBehaviour is not TowerBehaviour)
        {
            StartPushing(characterBehaviour);
        }
    }
    private void StartPushing(CharacterBehaviour characterBehaviour)
    {
        characterBehaviour.StartGetPushed();
        ObjectsToMove.Add(characterBehaviour);
    }
    void OnDestroy()
    {
        ObjectsToMove.RemoveAll(item => item == null);
        foreach (CharacterBehaviour characterBehaviour in ObjectsToMove)
        {
            characterBehaviour.StopGetPushed();
        }
    }
}
