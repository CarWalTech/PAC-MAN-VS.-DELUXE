using UnityEngine;

public class GrayGhostAI : GhostAI
{
    protected override void Chase()
    {
        var ghost = GetComponent<Ghost>();
        if (ghost)
        {
            if (ghost.isTagged)
            {
                ChaseTarget(scatterModeTarget, normalSpeed);
            }
            else
            {
                ChaseTarget(scatterModeTarget, normalSpeed);
            }
        }
    }

    protected override void Scatter()
    {
        var ghost = GetComponent<Ghost>();
        if (ghost)
        {
            if (ghost.isTagged)
            {
                ChaseTarget(scatterModeTarget, normalSpeed);
            }
            else
            {
                ChaseTarget(scatterModeTarget, normalSpeed);
            }
        }
    }
}
