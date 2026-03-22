using UnityEngine;

public class BlueGhostAI : GhostAI
{
    public GameObject redGhost;

    protected override void Chase()
    {
        var targetPos = chaseModeTarget.transform.position;
        Vector2 directionVector = targetPos - redGhost.transform.position;
        ChaseTarget((Vector2)targetPos + directionVector, normalSpeed);
    }
}
