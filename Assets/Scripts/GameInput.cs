using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance {  get; private set; }

    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        Instance = this;
        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();
    }
    public Vector2 GetMovementVector()
    {
        Vector2 inputVector = playerInputActions.Hero.Move.ReadValue<Vector2>();

        return inputVector;
    }

    public Vector3 MousePosition()
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        return mousePosition;

    }
}
