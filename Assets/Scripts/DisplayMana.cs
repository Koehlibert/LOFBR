using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayMana : MonoBehaviour
{
    public Image Reloadcircle;
    public Image ReloadParent;
    public PlayerController player;
    public bool reloading;
    public Text Mana;
    // Start is called before the first frame update
    void Start()
    {
        this.ReloadParent = GetComponent<Image>(); 
        Reloadcircle.fillAmount = 0;
        reloading = false;
    }
    void Update()
    {
        if(!player)
        {
            player = FindAnyObjectByType<PlayerController>();
        }
        Reloadcircle.fillAmount = player.manasys.getPercent();
        Mana.text = player.manasys.getString();
    }
}
