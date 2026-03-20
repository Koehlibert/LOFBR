using System.Collections;
using System.Collections.Generic;

public class Level
{
    private int level;
    private float exp;
    private int[] levelexp;
    private int maxlevel;
    private MainPlayerBehaviour Player;
    public void Init(MainPlayerBehaviour player)
    {
        Player = player;
        level = 1;
        levelexp = new int[]{0,40,120,200,350,500,700,850,1100,1400,1800};
        maxlevel = 10;
        exp = 0;
    }
    public int GetLevel()
    {
        return level;
    }
    public bool CheckLevel(int val)
    {
        return (level >= val);
    }
    public void GainExp(int amount)
    {
        exp += amount;
        if ((exp >= levelexp[level])&& !IsMaxLevel() & Player.isActiveAndEnabled)
        {
            level++;
            Player.LevelUp();
        }
    }
    public bool IsMaxLevel()
    {
        return (level == maxlevel);
    }
    public float ExpPercentage()
    {
        return ((exp - levelexp[level-1])/(levelexp[level]-levelexp[level -1]));
    }
}
