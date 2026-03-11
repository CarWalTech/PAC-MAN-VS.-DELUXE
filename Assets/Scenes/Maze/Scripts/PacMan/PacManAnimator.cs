using UnityEngine;

public class PacManAnimator : MonoBehaviour
{
    public Animator animatonController;
    private PacMan _source = null;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!_source) return;

        animatonController.SetBool("DeathSequence", _source.isDead);
    }

    public void OnUpdate(PacMan player)
    {
        if (!_source) _source = player;
    }
}
