using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    public PlayerController player;
    public float reloadtime;
    protected bool loaded;
    public Reload reloader;
    public float manaCost;
    protected virtual void Start()
    {
        player = GetComponent<PlayerController>();
    }
    void OnEnable()
    {
        Reset();
        player = GetComponent<PlayerController>();
    }
    void Update()
    {
        if(!player)
        {
            player = GetComponent<PlayerController>();
        }
    }
    public void Activate()
    {
        reloader.Activate();
    }
    public void Reset()
    {
        loaded = true;
    }
    private IEnumerator reload()
    {
        loaded = false;
        yield return new WaitForSeconds(reloadtime);
        loaded = true;
    }
    public void setReloader(Reload val)
    {
        reloader = val;
        reloader.setAbility(this);
    }
}
