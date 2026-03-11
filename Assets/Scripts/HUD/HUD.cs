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
        PrimaryReloader.SetActive(false);
        SecondaryReloader.SetActive(false);
        AltReloader.SetActive(false);
        SkillReloader.SetActive(false);
        UltReloader.SetActive(false);  
    }
    public Reload GetReload(GameObject gameObject)
    {
        return gameObject.GetComponent<Reload>();
    }
}
