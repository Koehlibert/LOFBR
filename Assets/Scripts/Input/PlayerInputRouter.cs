using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputRouter : MonoBehaviour
{
    public Vector2 Move { get; private set; }
    public Vector2 Look { get; private set; }
    public bool CheatedPressed { get; private set; }
    public bool CheatedToggledThisFrame { get; private set; }
    public bool CheatedPressedThisFrame { get; private set; }
    public bool CheatedReleasedThisFrame { get; private set; }
    public bool PrimaryPressed { get; private set; }
    public bool PrimaryToggledThisFrame { get; private set; }
    public bool PrimaryPressedThisFrame { get; private set; }
    public bool PrimaryReleasedThisFrame { get; private set; }
    public bool SecondaryPressed { get; private set; }
    public bool SecondaryToggledThisFrame { get; private set; }
    public bool SecondaryPressedThisFrame { get; private set; }
    public bool SecondaryReleasedThisFrame { get; private set; }
    public bool AlternativePressed { get; private set; }
    public bool AlternativeToggledThisFrame { get; private set; }
    public bool AlternativePressedThisFrame { get; private set; }
    public bool AlternativeReleasedThisFrame { get; private set; }
    public bool SkillPressed { get; private set; }
    public bool SkillToggledThisFrame { get; private set; }
    public bool SkillPressedThisFrame { get; private set; }
    public bool SkillReleasedThisFrame { get; private set; }
    public bool ThirdSkillPressed { get; private set; }
    public bool ThirdSkillToggledThisFrame { get; private set; }
    public bool ThirdSkillPressedThisFrame { get; private set; }
    public bool ThirdSkillReleasedThisFrame { get; private set; }
    public bool FourthSkillPressed { get; private set; }
    public bool FourthSkillToggledThisFrame { get; private set; }
    public bool FourthSkillPressedThisFrame { get; private set; }
    public bool FourthSkillReleasedThisFrame { get; private set; }
    public bool UltPressed { get; private set; }
    public bool UltToggledThisFrame { get; private set; }
    public bool UltPressedThisFrame { get; private set; }
    public bool UltReleasedThisFrame { get; private set; }
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

        Input.Player.AttackPrimary.performed += OnPrimaryPerformed;
        Input.Player.AttackPrimary.canceled += OnPrimaryCancelled;

        Input.Player.Alternative.performed += OnAlternativePerformed;
        Input.Player.Alternative.canceled += OnAlternativeCanceled;

        Input.Player.Ult.performed += OnUltPerformed;
        Input.Player.Ult.canceled += OnUltCancelled;

        Input.Player.Cheat.performed += OnCheatPerformed;
        Input.Player.Cheat.performed += OnCheatCancelled;

        Input.Player.AttackSecondary.performed += OnSecondaryPerformed;
        Input.Player.AttackSecondary.performed += OnSecondaryCancelled;

        Input.Player.SecondarySkill.performed += OnSkillPerformed;
        Input.Player.SecondarySkill.canceled += OnSkillCanceled;

        Input.Player.ThirdSkill.performed += OnThirdSkillPerformed;
        Input.Player.ThirdSkill.canceled += OnThirdSkillCanceled;

        Input.Player.FourthSkill.performed += OnFourthSkillPerformed;
        Input.Player.FourthSkill.canceled += OnFourthSkillCanceled;

        Input.Player.Pause.started += _ => PauseGame.Instance.TogglePause();
    }
    private void OnPrimaryPerformed(InputAction.CallbackContext ctx)
    {
        PrimaryPressed = true;
        PrimaryPressedThisFrame = true;
        PrimaryToggledThisFrame = true;
    }
    private void OnPrimaryCancelled(InputAction.CallbackContext ctx)
    {
        PrimaryPressed = false;
        PrimaryReleasedThisFrame = true;
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
    private void OnAlternativePerformed(InputAction.CallbackContext ctx)
    {
        AlternativePressed = true;
        AlternativePressedThisFrame = true;
        AlternativeToggledThisFrame = true;
    }
    private void OnAlternativeCanceled(InputAction.CallbackContext ctx)
    {
        AlternativePressed = false;
        AlternativeReleasedThisFrame = true;
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
    private void OnThirdSkillPerformed(InputAction.CallbackContext ctx)
    {
        ThirdSkillPressed = true;
        ThirdSkillPressedThisFrame = true;
        ThirdSkillToggledThisFrame = true;
    }
    private void OnThirdSkillCanceled(InputAction.CallbackContext ctx)
    {
        ThirdSkillPressed = false;
        ThirdSkillReleasedThisFrame = true;
    }
    private void OnFourthSkillPerformed(InputAction.CallbackContext ctx)
    {
        FourthSkillPressed = true;
        FourthSkillPressedThisFrame = true;
        FourthSkillToggledThisFrame = true;
    }
    private void OnFourthSkillCanceled(InputAction.CallbackContext ctx)
    {
        FourthSkillPressed = false;
        FourthSkillReleasedThisFrame = true;
    }
    private void OnUltPerformed(InputAction.CallbackContext ctx)
    {
        UltPressed = true;
        UltPressedThisFrame = true;
        UltToggledThisFrame = true;
    }
    private void OnUltCancelled(InputAction.CallbackContext ctx)
    {
        UltPressed = false;
        UltReleasedThisFrame = true;
    }
    private void OnCheatPerformed(InputAction.CallbackContext ctx)
    {
        CheatedPressed = true;
        CheatedPressedThisFrame = true;
        CheatedToggledThisFrame = true;
    }
    private void OnCheatCancelled(InputAction.CallbackContext ctx)
    {
        CheatedPressed = false;
        CheatedReleasedThisFrame = true;
    }
    private void LateUpdate()
    {
        PrimaryPressedThisFrame = false;
        PrimaryReleasedThisFrame = false;
        PrimaryToggledThisFrame = false;
        AlternativePressedThisFrame = false;
        AlternativeReleasedThisFrame = false;
        AlternativeToggledThisFrame = false;
        SkillPressedThisFrame = false;
        SkillReleasedThisFrame = false;
        SkillToggledThisFrame = false;
        SecondaryPressedThisFrame = false;
        SecondaryReleasedThisFrame = false;
        SecondaryToggledThisFrame = false;
        ThirdSkillPressedThisFrame = false;
        ThirdSkillReleasedThisFrame = false;
        ThirdSkillToggledThisFrame = false;
        FourthSkillPressedThisFrame = false;
        FourthSkillReleasedThisFrame = false;
        FourthSkillToggledThisFrame = false;
        UltPressedThisFrame = false;
        UltReleasedThisFrame = false;
        UltToggledThisFrame = false;
        CheatedPressedThisFrame = false;
        CheatedReleasedThisFrame = false;
        CheatedToggledThisFrame = false;
    }
    private void OnDisable()
    {
        Input.Player.Disable();
    }
}
