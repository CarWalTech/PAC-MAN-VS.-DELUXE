using UnityEngine;

public class MenuManager : MonoBehaviour
{

    [Header("Cameras")]
    public CameraNavigation cameraNavigation;

    [Header("SFX")]
    public AudioSource hoverSound;
    public AudioSource sliderSound;
    public AudioSource swooshSound;

    [Header("Game Settings")]
    public MatchConfigManager gameConfig;

    void Start()
    {
        if (GameConfiguration.GameExitMode == GameConfiguration.ExitMode.Developer) Navigation_LocalVS();
        GameConfiguration.GameEnterMode = GameConfiguration.EnterMode.Disabled;
        GameConfiguration.GameExitMode = GameConfiguration.ExitMode.Disabled;
    }

    public void NavigateTo(int navigationPoint)
    {
        cameraNavigation.StartTransition = CameraNavigation.Transition.FadeOut;
        cameraNavigation.EndTransition = CameraNavigation.Transition.FadeIn;
        cameraNavigation.StartDuration = 0.5f;
        cameraNavigation.EndDuration = 0.5f;
        cameraNavigation.CurrentLocation = navigationPoint;
    }

    public void Navigation_Title()
    {
        cameraNavigation.StartTransition = CameraNavigation.Transition.FadeOut;
        cameraNavigation.EndTransition = CameraNavigation.Transition.FadeIn;
        cameraNavigation.StartDuration = 0.5f;
        cameraNavigation.EndDuration = 0.5f;
        cameraNavigation.CurrentLocation = 0;
    }
    public void Navigation_MainMenu()
    {
        cameraNavigation.StartTransition = CameraNavigation.Transition.FadeOut;
        cameraNavigation.EndTransition = CameraNavigation.Transition.FadeIn;
        cameraNavigation.StartDuration = 0.5f;
        cameraNavigation.EndDuration = 0.5f;
        cameraNavigation.CurrentLocation = 1;
    }
    public void Navigation_Options()
    {
        cameraNavigation.StartTransition = CameraNavigation.Transition.FadeOut;
        cameraNavigation.EndTransition = CameraNavigation.Transition.FadeIn;
        cameraNavigation.StartDuration = 0.5f;
        cameraNavigation.EndDuration = 0.5f;
        cameraNavigation.CurrentLocation = 2;
    }
        public void Navigation_Achivements()
    {
        cameraNavigation.StartTransition = CameraNavigation.Transition.FadeOut;
        cameraNavigation.EndTransition = CameraNavigation.Transition.FadeIn;
        cameraNavigation.StartDuration = 0.5f;
        cameraNavigation.EndDuration = 0.5f;
        cameraNavigation.CurrentLocation = 3;
    }
    public void Navigation_Extras()
    {
        cameraNavigation.StartTransition = CameraNavigation.Transition.FadeOut;
        cameraNavigation.EndTransition = CameraNavigation.Transition.FadeIn;
        cameraNavigation.StartDuration = 0.5f;
        cameraNavigation.EndDuration = 0.5f;
        cameraNavigation.CurrentLocation = 4;
    }

    public void Navigation_LocalVS()
    {
        cameraNavigation.StartTransition = CameraNavigation.Transition.FadeOut;
        cameraNavigation.EndTransition = CameraNavigation.Transition.FadeIn;
        cameraNavigation.StartDuration = 0.5f;
        cameraNavigation.EndDuration = 0.5f;
        cameraNavigation.CurrentLocation = 5;
    }

    public void Navigation_QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void Navigation_StartGame()
    {
        GameConfiguration.Event_StartGame(gameConfig);
    }
    public void SFX_ButtonHover()
    {
        hoverSound.Play();
    }
    public void SFX_SliderTick()
    {
        sliderSound.Play();
    }
    public void SFX_Swoosh()
    {
        swooshSound.Play();
    }


}
