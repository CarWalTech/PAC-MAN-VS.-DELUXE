using UnityEngine;


public class OrangeGhostAI : GhostAI
{
    protected override void Chase()
    {
        ChaseTarget(
            Vector2.Distance(transform.position, chaseModeTarget.transform.position) >= 8f
                ? chaseModeTarget
                : scatterModeTarget, normalSpeed);
    }
}

