using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputRouter : MonoBehaviour
{
    public Vector2 Move { get; private set; }
    public Vector2 Look { get; private set; }
    public bool CheatedPressed { get; private set; }
    public bool PrimaryPressed { get; private set; }
    public bool SecondaryPressed { get; private set; }
    public bool SecondaryToggledThisFrame { get; private set; }
    public bool SecondaryPressedThisFrame { get; private set; }
    public bool SecondaryReleasedThisFrame { get; private set; }
    public bool AlternativePressed { get; private set; }
    public bool SkillPressed { get; private set; }
    public bool SkillToggledThisFrame { get; private set; }
    public bool SkillPressedThisFrame { get; private set; }
    public bool SkillReleasedThisFrame { get; private set; }
    public bool UltPressed { get; private set; }
    public bool PausePressed { get; private set; }
    public PlayerInputActions Input { get; private set; }
    public static PlayerInputRouter Instance;
    private void Awake()
    {
        Input = new PlayerInputActions();
        Instance = this;
    }

    private void OnEnable()
    {
        Input.Player.Enable();

        Input.Player.Move.performed += ctx => Move = ctx.ReadValue<Vector2>();
        Input.Player.Move.canceled += _ => Move = Vector2.zero;

        Input.Player.Look.performed +=
            ctx => Look = ctx.ReadValue<Vector2>();
        Input.Player.Look.canceled += _ => Look = Vector2.zero;

        Input.Player.AttackPrimary.performed += _ => PrimaryPressed = true;
        Input.Player.AttackPrimary.canceled += _ => PrimaryPressed = false;

        Input.Player.Alternative.performed += _ => AlternativePressed = true;
        Input.Player.Alternative.canceled += _ => AlternativePressed = false;

        Input.Player.Ult.performed += _ => UltPressed = true;
        Input.Player.Ult.canceled += _ => UltPressed = false;

        Input.Player.Cheat.performed += _ => CheatedPressed = true;
        Input.Player.Cheat.canceled += _ => CheatedPressed = false;

        Input.Player.AttackSecondary.performed += OnSecondaryPerformed;
        Input.Player.AttackSecondary.performed += OnSecondaryCancelled;

        Input.Player.SecondarySkill.performed += OnSkillPerformed;
        Input.Player.SecondarySkill.canceled += OnSkillCanceled;

        Input.Player.Pause.started += _ => PauseGame.Instance.TogglePause();
    }
    private void OnSecondaryPerformed(InputAction.CallbackContext ctx)
    {
        SecondaryPressed = true;
        SecondaryPressedThisFrame = true;
        SecondaryToggledThisFrame = true;
    }
    private void OnSecondaryCancelled(InputAction.CallbackContext ctx)
    {
        SecondaryPressed = false;
        SecondaryReleasedThisFrame = true;
    }
    private void OnSkillPerformed(InputAction.CallbackContext ctx)
    {
        SkillPressed = true;
        SkillPressedThisFrame = true;
        SkillToggledThisFrame = true;
    }
    private void OnSkillCanceled(InputAction.CallbackContext ctx)
    {
        SkillPressed = false;
        SkillReleasedThisFrame = true;
    }
    private void LateUpdate()
    {
        SkillPressedThisFrame = false;
        SkillReleasedThisFrame = false;
        SkillToggledThisFrame = false;
        SecondaryPressedThisFrame = false;
        SecondaryReleasedThisFrame = false;
        SecondaryToggledThisFrame = false;
    }
    private void OnDisable()
    {
        Input.Player.Disable();
    }
}
