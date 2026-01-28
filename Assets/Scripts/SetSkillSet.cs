using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSkillSet : MonoBehaviour
{
    public void setFighterID()
    {
        PlayerPrefs.SetInt("classID", 1);
    }
    public void setSupportID()
    {
        PlayerPrefs.SetInt("classID", 2);
    }
    public void setMeleeID()
    {
        PlayerPrefs.SetInt("classID", 3);
    }
}
