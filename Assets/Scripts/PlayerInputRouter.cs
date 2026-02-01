using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputRouter : MonoBehaviour
{
    public Vector2 Move { get; private set; }
    public Vector2 Look { get; private set; }
    public bool CheatedPressed { get; private set; }
    public bool PrimaryPressed { get; private set; }

    public PlayerInputActions Input { get; private set; }

    private void Awake()
    {
        Input = new PlayerInputActions();
    }

    private void OnEnable()
    {
        Input.Player.Enable();

        Input.Player.Move.performed += ctx => Move = ctx.ReadValue<Vector2>();

        Input.Player.Look.performed +=
            ctx => Look = ctx.ReadValue<Vector2>();

        Input.Player.AttackPrimary.performed += _ => PrimaryPressed = true;
        Input.Player.Cheat.performed += _ => CheatedPressed = true;
    }

    private void OnDisable()
    {
        Input.Player.Disable();
    }
}
