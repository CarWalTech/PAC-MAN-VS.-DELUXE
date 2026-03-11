using System.Collections;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public GameObject target;
    public Vector2 outDirection = Vector2.zero;

    private void OnTriggerEnter2D(Collider2D other)
    {
        StartCoroutine(Teleport(other));
    }

    private IPlayable GetEntity(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            return other.gameObject.GetComponent<PacMan>();
        else if (other.gameObject.layer == LayerMask.NameToLayer("Chaser"))
            return other.gameObject.GetComponent<Ghost>();
        else return null;
    }

    private IEnumerator Teleport(Collider2D collider)
    {   
        IPlayable other = GetEntity(collider);
        if (other != null && !other.IsLocked())
        {
            other.Lock();
            GameManager.Instance.Teleport_FadeOut(other);
            yield return new WaitForSeconds(0.5f);
            var warpTarget = target.GetComponent<Transform>().position;
            other.TeleportTo(warpTarget, outDirection);
            yield return new WaitForSeconds(0.5f);
            GameManager.Instance.Teleport_FadeIn(other);     
            other.Unlock();
        }

    }
}
