using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Pellet : PelletThemeHolder
{
    public int points = 10;
    public GameObject worldPrefab;
    public GameObject worldObject { get; private set; }
    private static GameObject worldPelletCache = null;
    public SpriteRenderer spriteRenderer;
    public AnimatedSprite animatedSprite;

    void OnValidate()
    {
        RefreshSkin();        
    }

    IEnumerator Start()
    {
        yield return new WaitUntil(() => GameManager.IsReady);
        RefreshSkin();
        if (!worldPelletCache)
        {
            GameObject worldCache = GameManager.Instance.GetWorldViewCache();
            worldPelletCache = new GameObject("Pellets");
            worldPelletCache.transform.SetParent(worldCache.transform);
        }

        worldObject = GameManager.Instance.GetMazeManager().InstantiateInWorld(worldPrefab, transform.localPosition, worldPelletCache);


        GameManager.Instance.CollectPellet(this);
    }

    public override void RefreshSkin()
    {
        var skin = GetSkin();
        if (!skin) return;

        var result = skin.GetSpriteSet(PelletTheme.PelletType.Normal);
        if (result == null) return;
        
        spriteRenderer.sprite = result.sprites[0];
        animatedSprite.sprites = result.sprites.ToArray();
        animatedSprite.animationTime = result.animationTime;
        animatedSprite.loop = result.loop;

        if (!skin.models.ContainsKey(PelletTheme.PelletType.Normal)) return;
        worldPrefab = skin.models[PelletTheme.PelletType.Normal];
    }

    public void ResetState()
    {
        gameObject.SetActive(true);
        worldObject.SetActive(true);
    }

    protected virtual void Eat(IPlayable src)
    {
        GameManager.Instance.Event_EatPellet(src, this);
    }

    public void OnEat()
    {   
        gameObject.SetActive(false);
        worldObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            Eat(other.gameObject.GetComponent<PacMan>());
        }
    }

}
