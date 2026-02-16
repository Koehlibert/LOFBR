using UnityEngine;

public class HUD : MonoBehaviour
{
    public static HUD Instance;
    public GameObject PrimaryReloader;
    public GameObject SecondaryReloader;
    public GameObject AltReloader;
    public GameObject SkillReloader;
    public GameObject UltReloader;
    private void Awake()
    {
        Instance = this;
    }
    public Reload GetReload(GameObject gameObject)
    {
        return gameObject.GetComponent<Reload>();
    }
}
