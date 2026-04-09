using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.XR;
public class MirrorImage : Ability
{
    private int ClassID;
    protected override AbilityInfo GetAbilityInfo()
    {
        return new AbilityInfo(120, 25, new List<AIUtils.AIState> { AIUtils.AIState.Attacking, AIUtils.AIState.CheckShoot, AIUtils.AIState.CheckDistSkills });
    }
    protected override bool InputPressed()
    {
        return PlayerInputRouter.Instance.ThirdSkillPressedThisFrame;
    }
    protected override void AdditionalInit()
    {
        ClassID = (Handler.Owner is MainPlayerBehaviour mainPlayerBehaviour) ? mainPlayerBehaviour.ClassID : 1;
    }
    protected override void AbilityAction()
    {
        StartCoroutine(MirrorAnimation());
        base.AbilityAction();
        StartCoroutine(Reload());
    }
    private IEnumerator MirrorAnimation()
    {
        movementAI.LockMovementAI(0.5f);
        yield return new WaitForSeconds(0.5f);
        int nImages = GetImageCount() + 1;
        List<Vector3> positions = GetTargetPositions(nImages);
        int playerLocIdx = UnityEngine.Random.Range(0, GetImageCount());
        Handler.transform.position = positions[playerLocIdx];
        for (int i = 0; i < nImages; i++)
        {
            if (i == playerLocIdx)
                continue;
            CharacterFactory.Instance.CreateMirrorEntity(Handler.Owner.Team, positions[i], Quaternion.identity, ClassID, GetLevelToGive());
        }
    }
    private int GetImageCount()
    {
        int count = 1;
        if (Handler.Owner is MainPlayerBehaviour)
        {
            int level = (Handler.Owner as MainPlayerBehaviour).Levelsys.GetLevel();
            count = level switch
            {
                < 5 => 1,
                < 8 => 2,
                _ => 3
            };
        }
        return count;
    }
    private int GetLevelToGive()
    {
        int levelNumber = 1;
        if (Handler.Owner is MainPlayerBehaviour)
        {
            int level = (Handler.Owner as MainPlayerBehaviour).Levelsys.GetLevel();
            levelNumber = level switch
            {
                < 6 => 2,
                < 8 => 3,
                < 9 => 4,
                _ => 5
            };
        }
        return levelNumber;
    }
    private List<Vector3> GetTargetPositions(int nImages)
    {
        List<Vector3> targetPositions = new();
        float radius = 2.5f;
        Vector3 thisPos = MasterScript.Instance.CorrectTarget(Handler.Owner.transform.position, radius);
        for (int i = 0; i < nImages; i++)
        {
            float angle = i * Mathf.PI * 2f / nImages;

            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;

            Vector3 point = thisPos + new Vector3(x, 0f, z);
            targetPositions.Add(point);
        }
        return targetPositions;
    }
}