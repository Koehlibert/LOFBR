using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class ExpDisplay : MonoBehaviour
{
    public PlayerController player;
    public Slider expslider;
    void Start()
    {
        player = MasterScript.Instance.player;
        expslider = GetComponent<Slider>();
    }
    void Update()
    {
        if (player.isActiveAndEnabled)
        {
            if (player.Levelsys.IsMaxLevel())
            {
                expslider.value = 1;
            }
            else
            {
                expslider.value = player.Levelsys.ExpPercentage();
            }
        }
    }
}
