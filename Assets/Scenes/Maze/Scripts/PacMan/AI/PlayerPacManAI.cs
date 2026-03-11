using UnityEngine;
using System.Linq;
public class PlayerPacManAI : PacManAI
{
    protected override void InputUpdate()
    {
        // Set the new direction based on the current input
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            SetInputDirection(Vector2.up);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            SetInputDirection(Vector2.down);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            SetInputDirection(Vector2.left);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            SetInputDirection(Vector2.right);
        }

    }
}