using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AreaManager : MonoBehaviour
{
    protected abstract CombatUtils.Team Team { get; }
    public float upperAreaLimitX = 18;
    public float lowerAreaLimitX = -18;
    public GameObject Area;
    public GameObject Floor;
    public GameObject RespawnPoint;
    public SpawnerBehaviour Spawner;
    protected float MoveDirection;
    public float duration = 3f;
    private float elapsed = 0f;
    private bool isShrinking = false;
    private Vector3 areaStartScale;
    private float previousScaleZ;
    public abstract void Init();
    public void StartShrink()
    {
        areaStartScale = Area.transform.localScale;
        previousScaleZ = areaStartScale.z;
        if (previousScaleZ <= 0.1f) return;
        Spawner.SpeedUpSpawner(1f);
        isShrinking = true;
        elapsed = 0f;
    }
    void LateUpdate()
    {
        if (!isShrinking) return;
        elapsed += Time.deltaTime;
        float t = elapsed / duration;
        t = Mathf.Clamp01(t);
        Area.transform.localScale = Vector3.Lerp(
                    areaStartScale,
                    areaStartScale + new Vector3(0, 0, -0.1f),
                    t);
        float currentScaleZ = Area.transform.localScale.z;
        float scaleFactor = Area.transform.localScale.z / previousScaleZ;
        previousScaleZ = currentScaleZ;
        MoveAlong(CharacterTracker.Instance.allEnemiesTowers, scaleFactor);
        MoveAlong(CharacterTracker.Instance.allFriendliesTowers, scaleFactor);
        MoveAlong(CharacterTracker.Instance.enemyPlayer, scaleFactor);
        MoveAlong(CharacterTracker.Instance.player, scaleFactor);
        Spawner.MoveSpawner(scaleFactor);
        if (t >= 1f)
            isShrinking = false;
    }
    private void MoveAlong(List<GameObject> gameObjects, float scaleFactor)
    {
        foreach (GameObject obj in gameObjects)
        {
            MoveAlong(obj, scaleFactor);
        }
    }
    private void MoveAlong(GameObject gameObject, float scaleFactor)
    {
        if (Mathf.Sign(gameObject.transform.position.z) != Mathf.Sign(MoveDirection))
        {
            Vector3 dir = gameObject.transform.position;
            dir.z *= scaleFactor;
            gameObject.transform.position = dir;
        }
    }
    private void MoveAlong(DamageableEntity damageableEntity, float scaleFactor)
    {
        MoveAlong(damageableEntity.gameObject, scaleFactor);
    }

}