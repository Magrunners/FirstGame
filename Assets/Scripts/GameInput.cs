using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class GameInput : MonoBehaviour
{
    public static GameInput Instance {  get; private set; }
    private PlayerInputActions _playerInputActions;
    public event EventHandler OnHeroAttack;
    private void Awake()
    {
        Instance = this;
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Enable();
        _playerInputActions.Attack.Attack.started += Attack_started;

    }
    private void Attack_started(InputAction.CallbackContext obj)
    {
        if (OnHeroAttack != null)
            OnHeroAttack.Invoke(this, EventArgs.Empty);
    }
    public Vector2 GetMovementVector()
    {
        Vector2 inputVector = _playerInputActions.Hero.Move.ReadValue<Vector2>();

        return inputVector;
    }
    public Vector3 MousePosition()
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        return mousePosition;

    }
    public void DisableMovement()
    {
        _playerInputActions.Disable();
    }
}
