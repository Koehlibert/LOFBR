using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    private int level;
    private float exp;
    private int[] levelexp;
    private int maxlevel;
    void Start()
    {
        level = 1;
        levelexp = new int[]{0,40,120,200,350,500,700,850,1100,1400,1800};
        maxlevel = 10;
        exp = 0;
    }

    // Update is called once per frame
    void Update()
    {
    }
    public int getLevel()
    {
        return level;
    }
    public bool checkLevel(int val)
    {
        return (level >= val);
    }
    public void gainExp(int amount)
    {
        exp += amount;
        if ((exp >= levelexp[level])&&(level < maxlevel))
        {
            level++;
            SendMessage("LevelUp");
        }
    }
    public bool isMaxLevel()
    {
        return (level == maxlevel);
    }
    public float expPercentage()
    {
        return ((exp - levelexp[level-1])/(levelexp[level]-levelexp[level -1]));
    }
}
