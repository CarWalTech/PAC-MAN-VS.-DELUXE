using UnityEngine.InputSystem;

public interface IPlayerInput
{
    public void Input_OnMove(InputAction.CallbackContext context);
    public void Input_OnPause(InputAction.CallbackContext context);
    public void Input_OnSwitchViews(InputAction.CallbackContext context);
}