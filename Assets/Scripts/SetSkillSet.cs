using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSkillSet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
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
