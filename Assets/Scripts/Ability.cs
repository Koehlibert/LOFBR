using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    public PlayerController player;
    public float reloadtime;
    protected bool loaded;
    public Reload reloader;
    public float manaCost;
    protected InputActionAsset InputActions;
    private InputAction ability;
    public abstract string InputString { get; }
    protected virtual void Start()
    {
        player = GetComponent<PlayerController>();
    }
    protected virtual void Awake()
    {
        Debug.Log(InputString);
        ability = InputSystem.actions.FindAction(InputString);
    }
    void OnEnable()
    {
        InputActions.FindActionMap("Player").Enable();
        Reset();
        player = GetComponent<PlayerController>();
    }
    protected virtual void Update()
    {
        if(!player)
        {
            player = GetComponent<PlayerController>();
        }
        if(ability.WasPressedThisFrame() && (loaded) && player.manasys.checkCost(manaCost))
        {
            AbilityAction();
        }
    }
    protected abstract void AbilityAction();
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
