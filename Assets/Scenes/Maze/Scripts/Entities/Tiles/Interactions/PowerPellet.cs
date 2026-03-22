using UnityEngine;

public class PowerPellet : Pellet
{
    public float duration = 8f;

    protected override void Eat(IPlayable src)
    {
        GameManager.Instance.Event_EatPowerPellet(src, this);
    }

}
