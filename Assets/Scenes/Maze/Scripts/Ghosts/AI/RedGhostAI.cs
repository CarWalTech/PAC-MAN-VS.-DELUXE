public class RedGhostAI : GhostAI
{
    protected override void Chase()
    {
        ChaseTarget(chaseModeTarget, normalSpeed);
    }
}