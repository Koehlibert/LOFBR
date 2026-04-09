using UnityEngine;

[CreateAssetMenu(fileName = "TeamMaterials", menuName = "Resources/TeamMaterials")]
public class TeamMaterials : ScriptableObject
{
    public Material playerMaterial;
    public Material enemyMaterial;
    public Material transparentPlayerMaterial;
    public Material transparentEnemyMaterial;
}
