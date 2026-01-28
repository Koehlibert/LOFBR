using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ExpDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerController player;
    public Slider expslider;
    void Start()
    {
        player = GameObject.FindAnyObjectByType<PlayerController>();
        expslider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isActiveAndEnabled)
        {
            if (player.levelsys.isMaxLevel())
            {
                expslider.value = 1;
            }
            else
            {
                expslider.value = player.levelsys.expPercentage();
            }
        }
    }
}
