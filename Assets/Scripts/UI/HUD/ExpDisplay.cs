using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class ExpDisplay : MonoBehaviour
{
    public PlayerController player;
    [SerializeField] Slider expslider;
    [SerializeField] Text Level;
    void Start()
    {
        player = CharacterTracker.Instance.player;
        expslider = GetComponent<Slider>();
    }
    void Update()
    {
        Level.text = "Level " + player.Levelsys.GetLevel();
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
