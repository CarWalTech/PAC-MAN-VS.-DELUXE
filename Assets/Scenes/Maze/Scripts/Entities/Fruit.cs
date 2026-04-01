using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Fruit : FruitThemeHolder
{
    public enum FruitBehaviour
    {
        Classic = 1,
        Wandering = 2,
    }


    [Header("General")]
    public FruitType fruitType = FruitType.Cherry;
    public FruitBehaviour fruitBehavior;
    public int initalSpawnsRequirement = 70;
    public int subsequentSpawnsRequirement = 100;
    public int points = 400;

    [Header("Internals")]
    public GameObject worldPrefab;
    public GameObject worldObject { get; private set; }
    private static GameObject worldFruitCache = null;
    private static GameObject mazeFruitCache = null;
    public SpriteRenderer spriteRenderer;

    private bool _initalSpawnMade = false;
    private int _lastSpawnPelletsEaten = 0;
    private bool _fruitActive = false;

    public static void SetupFruit(Spawner_Fruit spawn, Vector3 mazeOrigin)
    {
        if (!mazeFruitCache)
        {
            GameObject mazeCache = GameManager.Instance.GetMazeViewCache();
            mazeFruitCache = new GameObject("Fruits");
            mazeFruitCache.transform.SetParent(mazeCache.transform);
        }
        Instantiate(spawn.prefab, mazeOrigin, Quaternion.Euler(Vector3.zero), Fruit.mazeFruitCache.transform);
    }

    void OnValidate()
    {
        RefreshSkin();        
    }

    IEnumerator Start()
    {
        yield return new WaitUntil(() => GameManager.IsReady);
        RefreshSkin();
        if (!worldFruitCache)
        {
            GameObject worldCache = GameManager.Instance.GetWorldViewCache();
            worldFruitCache = new GameObject("Fruits");
            worldFruitCache.transform.SetParent(worldCache.transform);
        }

        worldObject = GameManager.Instance.GetMazeManager().InstantiateInWorld(worldPrefab, transform.localPosition, worldFruitCache);
        GameManager.Instance.CollectFruit(this);
        SetupFruitLogic();
    }

    private void SetupFruitLogic()
    {
        spriteRenderer.enabled = false;
        worldObject.SetActive(false);
        _lastSpawnPelletsEaten = 0;
        _initalSpawnMade = false;
        _fruitActive = false;
    }

    public void ResetState()
    {
        SetupFruitLogic();
    }

    public void OnEat()
    {   
        spriteRenderer.enabled = false;
        worldObject.SetActive(false);
        _fruitActive = false;
    }

    public void SummonFruit()
    {
        print("fruit appered");
        spriteRenderer.enabled = true;
        worldObject.SetActive(true);
        _fruitActive = true;
    }
    public void Update()
    {
        if (_initalSpawnMade == false && GameManager.Instance.GetPelletsEaten() >= initalSpawnsRequirement)
        {
            SummonFruit();
            _lastSpawnPelletsEaten = initalSpawnsRequirement;
            _initalSpawnMade = true;
        }
        else if (_initalSpawnMade == true && GameManager.Instance.GetPelletsEaten() >= _lastSpawnPelletsEaten + subsequentSpawnsRequirement)
        {
            SummonFruit();
            _lastSpawnPelletsEaten += subsequentSpawnsRequirement;
        }
    }

    public override void RefreshSkin()
    {
        var skin = GetSkin();
        if (!skin) return;

        var result = skin.GetSpriteSet(fruitType);
        if (result == null) return;
        
        spriteRenderer.sprite = result.sprites[0];
        spriteRenderer.GetComponent<AnimatedSprite>().sprites = result.sprites.ToArray();
        spriteRenderer.GetComponent<AnimatedSprite>().animationTime = result.animationTime;
        spriteRenderer.GetComponent<AnimatedSprite>().loop = result.loop;

        if (!skin.models.ContainsKey(fruitType)) return;
        worldPrefab = skin.models[fruitType];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_fruitActive) return;

        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            GameManager.Instance.Event_EatFruit_PacMan(this, other.gameObject.GetComponent<PacMan>());
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Chaser")) {
            var targetGhost = other.gameObject.GetComponent<Ghost>();
            if (!targetGhost.isEaten && !targetGhost.isTagGhost) GameManager.Instance.Event_EatFruit_Ghost(this, targetGhost);
            
        }
    }

}
