using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class healthdisplay : MonoBehaviour
{
    public PlayerController player;
    private Slider healthslider;
    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerController>();
        healthslider = GetComponent<Slider>();
    }
    void Update()
    {
        if (player.isActiveAndEnabled)
        {
            float hpval = player.GetHealth().healthDisplay();
            if (healthslider.value > hpval)
            {
                healthslider.value = Mathf.Max(hpval, healthslider.value - Time.deltaTime);
            }
            else if (healthslider.value < hpval)
            {
                healthslider.value = Mathf.Min(hpval, healthslider.value + Time.deltaTime);
            }
        }
    }
}
