public class PinkGhostAI : GhostAI
{
    protected override void Chase()
    {
        ChaseTarget(chaseModeTarget, normalSpeed);
    }
}