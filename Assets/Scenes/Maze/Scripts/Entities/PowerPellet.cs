using UnityEngine;

public class PowerPellet : Pellet
{
    public float duration = 8f;

    void OnValidate()
    {
        RefreshSkin();        
    }

    protected override void Eat(IPlayable src)
    {
        GameManager.Instance.Event_EatPowerPellet(src, this);
    }

    public override void RefreshSkin()
    {
        var skin = GetSkin();
        if (!skin) return;

        var result = skin.GetSpriteSet(PelletTheme.PelletType.Powered);
        if (result == null) return;
        
        spriteRenderer.sprite = result.sprites[0];
        animatedSprite.sprites = result.sprites.ToArray();
        animatedSprite.animationTime = result.animationTime;
        animatedSprite.loop = result.loop;

        if (!skin.models.ContainsKey(PelletTheme.PelletType.Powered)) return;
        worldPrefab = skin.models[PelletTheme.PelletType.Powered];
    }

}
