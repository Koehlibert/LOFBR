using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skillset : MonoBehaviour
{
    public Ability primary;
    public Ability secondary;
    public Ability alternative;
    public Ability skill;
    public Ability ultimate;
    private int classID;
    protected float manaCostPrimary;
    protected float manaCostSecondary;
    protected float manaCostAlternative;
    protected float manaCostSkill;
    protected float manaCostUltimate;
    private PlayerController player;
    private Mana manasys;
    private List<Ability> abilities;
    private List<Reload> reloadList;
    private LineRenderer lRend;
    protected bool wasCalled = false;
    protected GameObject meleeCol;
    protected GameObject parryCol;
    protected GameObject fire;
    protected GameObject aura;
    protected LineRenderer manaLineRend;
    protected float startingLife;
    protected float startingRegen;
    protected float startingArmor;
    protected float regenDelay;
    protected float startingSpeed;
    protected virtual void StartManually()
    {
        if (!wasCalled)
        {
            meleeCol =  FindAnyObjectByType<MeleeCollider>().gameObject;
            meleeCol.SetActive(false);
            parryCol = FindAnyObjectByType<ParryColliderBehaviour>().gameObject;
            parryCol.SetActive(false);
            fire =  FindAnyObjectByType<FireBehaviour>().gameObject;
            fire.SetActive(false);
            aura = FindAnyObjectByType<ArmorAura>().gameObject;
            aura.SetActive(false);
            manaLineRend = GetComponent<LineRenderer>();
            manaLineRend.enabled = false;
            abilities = new List<Ability>();
            abilities.Add(primary);
            abilities.Add(secondary);
            abilities.Add(alternative);
            abilities.Add(skill);
            abilities.Add(ultimate);
            reloadList = new List<Reload>();
            reloadList.Add(GameObject.Find("PrimaryReloader").GetComponent<Reload>());
            reloadList.Add(GameObject.Find("SecondaryReloader").GetComponent<Reload>());
            reloadList.Add(GameObject.Find("AltReloader").GetComponent<Reload>());
            reloadList.Add(GameObject.Find("SkillReloader").GetComponent<Reload>());
            reloadList.Add(GameObject.Find("UltReloader").GetComponent<Reload>());
            player = FindAnyObjectByType<PlayerController>();
            manasys = player.manasys;
            for (int i = 0; i<5; i++)
            {
                reloadList[i].setPlayer(player, manasys);
                abilities[i].setReloader(reloadList[i]);
                reloadList[i].enabled = false;
            }
            wasCalled = true;
        }
    }
    public virtual void BaseUnlock()
    {
    }
    public virtual void LevelUnlock(int lvl)
    {
    }
    public (float hpval, float regenval, float delay, float armorval) GetHPVals()
    {
        return (startingLife, startingRegen, regenDelay, startingArmor);
    }
    public float GetSpeed()
    {
        return startingSpeed;
    }
    void Update()
    {
        
    }
    public void SetID(int val)
    {
        classID = val;
    }
}
