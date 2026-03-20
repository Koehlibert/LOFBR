using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public static HUD Instance;
    public GameObject PrimaryReloader;
    public GameObject SecondaryReloader;
    public GameObject AltReloader;
    public GameObject SkillReloader;
    public GameObject UltReloader;
    public Image DamageImage;
    private Color flashcolor = new(1f, 0f, 0f, 0.1f);
    private float flashspeed;
    private void Awake()
    {
        Instance = this;
        PrimaryReloader.SetActive(false);
        SecondaryReloader.SetActive(false);
        AltReloader.SetActive(false);
        SkillReloader.SetActive(false);
        UltReloader.SetActive(false);
        flashspeed = 2.5f;
    }
    public void Update()
    {
        UpdateDamageImage();
    }
    void UpdateDamageImage()
    {
        DamageImage.color = Color.Lerp(DamageImage.color, Color.clear, flashspeed * Time.deltaTime);
    }
    public Reload GetReload(GameObject gameObject)
    {
        return gameObject.GetComponent<Reload>();
    }
    public void SetDamageImage(float alpha)
    {
        flashcolor.a = 0.8f * alpha;
        DamageImage.color = flashcolor;
    }
}
