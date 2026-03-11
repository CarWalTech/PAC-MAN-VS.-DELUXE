using System.Collections;
using UnityEngine;

public class CameraNavigation : MonoBehaviour
{
    public enum Transition
    {
        None = 0,
        FadeIn = 1,
        FadeOut = 2
    }

    #region Properties: Locations
    [Header("Locations")]
    public int CurrentLocation = 0;
    public Transform[] Locations;
    #endregion

    #region Properties: Transitions
    [Header("Transitions")]
    public Transition StartTransition = Transition.None;
    public float StartDuration = 1.0f;
    public Transition EndTransition = Transition.None;
    public float EndDuration = 1.0f;
    #endregion

    #region Properties: State Accessors

    public bool IsSwitching { get; private set; } = false;

    #endregion

    #region Properties: Private Fields
    private int _LastLocation = -1;
    private Transform _NextLocation { get => Locations[CurrentLocation]; }
    private ScreenFader _fader;
    #endregion


    void Start()
    {
        _fader = GetComponent<ScreenFader>();

    }

    void Update()
    {
        if (CurrentLocation != _LastLocation && !IsSwitching)
        {
            IsSwitching = true;
            StartCoroutine(SwitchScreens());
        }
            
    }

    IEnumerator TransitionRoutine(Transition transition, float duration)
    {
        switch (transition)
        {
            case Transition.None:
                yield return null;
                break;
            case Transition.FadeOut:
                _fader.fadeDuration = duration;
                yield return _fader.FadeToBlackRoutine();
                break;
            case Transition.FadeIn:
                _fader.fadeDuration = duration;
                yield return _fader.FadeFromBlackRoutine();
                break;
            default:
                yield return null;
                break;
        }
    }

    IEnumerator SwitchScreens()
    {
        yield return TransitionRoutine(StartTransition, StartDuration);

        var pos1 = gameObject.transform.position;
        var pos2 = _NextLocation.position;
        gameObject.transform.position = new Vector3(pos2.x, pos2.y, pos1.z);

        yield return TransitionRoutine(EndTransition, EndDuration);

        _LastLocation = CurrentLocation;
        IsSwitching = false;
    }
}
