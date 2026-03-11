using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;
public class PlayerPacManAI : PacManAI, IPlayerInput
{
    public void Input_OnMove(InputAction.CallbackContext context)
    {
        var direction = context.ReadValue<Vector2>();
        // Set the new direction based on the current input
        if (direction == Vector2.up) {
            SetInputDirection(Vector2.up);
        }
        else if (direction == Vector2.down) {
            SetInputDirection(Vector2.down);
        }
        else if (direction == Vector2.left) {
            SetInputDirection(Vector2.left);
        }
        else if (direction == Vector2.right) {
            SetInputDirection(Vector2.right);
        }
    }

    public void Input_OnPause(InputAction.CallbackContext context)
    {
        if (context.canceled) return;
        GameManager.Instance.Event_PauseGame();
    }

    public void Input_OnSwitchViews(InputAction.CallbackContext context)
    {
        if (context.canceled) return;
        GameManager.Instance.Event_SwitchCameras();
    }


    protected override void InputUpdate()
    {

    }
}