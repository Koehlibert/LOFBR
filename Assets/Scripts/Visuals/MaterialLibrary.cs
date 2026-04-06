using UnityEngine;

public static class MaterialLibrary
{
    private static TeamMaterials _instance;
    public static TeamMaterials Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<TeamMaterials>("TeamMaterials");

                if (_instance == null)
                {
                    Debug.LogError("TeamMaterials asset not found in Resources!");
                }
            }
            return _instance;
        }
    }
}