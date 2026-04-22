using UnityEngine;
public class WallMember : MonoBehaviour
{
    [SerializeField] Renderer rend;
    private CombatUtils.Team Team;
    public void Init(CombatUtils.Team team)
    {
        this.Team = team;
        rend = GetComponentInChildren<SkinnedMeshRenderer>();
        rend.material = Team == CombatUtils.Team.Player
            ? MaterialLibrary.Instance.playerMaterial
            : MaterialLibrary.Instance.enemyMaterial;
    }
}