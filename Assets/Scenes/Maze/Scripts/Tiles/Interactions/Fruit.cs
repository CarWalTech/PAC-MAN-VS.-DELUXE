using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Fruit : MonoBehaviour
{
    public int points = 10;
    public GameObject worldPrefab;
    public GameObject worldObject { get; private set; }
    private static GameObject worldFruitCache = null;
    private static GameObject mazeFruitCache = null;

    public static void SetupFruit(FruitSpawn spawn, Vector3 mazeOrigin)
    {
        if (!mazeFruitCache)
        {
            GameObject mazeCache = GameManager.Instance.GetMazeViewCache();
            mazeFruitCache = new GameObject("Fruits");
            mazeFruitCache.transform.SetParent(mazeCache.transform);
        }
        Instantiate(spawn.prefab, mazeOrigin, Quaternion.Euler(Vector3.zero), Fruit.mazeFruitCache.transform);
    }

    IEnumerator Start()
    {
        yield return new WaitUntil(() => GameManager.IsReady);
        if (!worldFruitCache)
        {
            GameObject worldCache = GameManager.Instance.GetWorldViewCache();
            worldFruitCache = new GameObject("Fruits");
            worldFruitCache.transform.SetParent(worldCache.transform);
        }

        worldObject = GameManager.Instance.GetMazeManager().InstantiateInWorld(worldPrefab, transform.localPosition, worldFruitCache);
        GameManager.Instance.CollectFruit(this);
    }

    public void ResetState()
    {
        gameObject.SetActive(true);
        worldObject.SetActive(true);
    }

    public void OnEat()
    {   
        gameObject.SetActive(false);
        worldObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            GameManager.Instance.Event_EatFruit_PacMan(this, other.gameObject.GetComponent<PacMan>());
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Chaser")) {
            GameManager.Instance.Event_EatFruit_Ghost(this, other.gameObject.GetComponent<Ghost>());
        }
    }

}
