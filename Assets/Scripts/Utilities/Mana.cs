using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mana : MonoBehaviour
{
    private float maxMana;
    private float mana;
    private float manaRegen;
    void Start()
    {
        maxMana = 250;
        mana = maxMana;
        manaRegen = 3.5f;
    }
    void Update()
    {
        if (mana <maxMana)
        {
            mana = Mathf.Min(mana + Time.deltaTime*manaRegen,maxMana);
        }
    }
    public void UpdateValues(int maxGainAbsolut, float regenGainAbsolut)
    {
        float previousmana = maxMana;
        maxMana += maxGainAbsolut;
        manaRegen += regenGainAbsolut;
        mana += (maxMana - previousmana);
    }
    public void UpdateValues(float maxGainPercent, float regenGainPercent)
    {
        float previousmana = maxMana;
        maxMana *= maxGainPercent;
        manaRegen *= regenGainPercent;
        mana += (maxMana-previousmana);
    }
    public void useMana(float amount)
    {
        mana -= amount;
    }
    public bool checkCost(float cost)
    {
        return (mana >= cost);
    }
    public float getPercent()
    {
        return mana/maxMana;
    }
    public string getString()
    {
        return Mathf.RoundToInt(mana) + " / " + Mathf.RoundToInt(maxMana) + "\n" + "+ " + Mathf.Round(manaRegen*10)/10 + "/sec";
    }
    public void gainMana(float amount)
    {
        mana = Mathf.Min(maxMana, mana + amount);
    }
    public float drainMana(float amount)
    {
        float drainAmount = Mathf.Min(mana, amount);
        mana -= drainAmount;
        return drainAmount;
    }
}
