using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GhostsView : MonoBehaviour
{
    public GhostViewport viewport1;
    public GhostViewport viewport2;
    public GhostViewport viewport3;
    public GhostViewport viewport4;
    private Camera _cam;

    void Start()
    {
        _cam = GetComponentInChildren<Camera>();
    }

    public void SetActive(bool val)
    {
        _cam.gameObject.SetActive(val);
    }

    public Color GetFrameColor(int playerId)
    {
        switch (playerId)
        {
            case 1:
                return PlayerPicker.GetGhostColor(GameConfiguration.GetCharacter(PlayerSlot.P1));
            case 2:
                return PlayerPicker.GetGhostColor(GameConfiguration.GetCharacter(PlayerSlot.P2));
            case 3:
                return PlayerPicker.GetGhostColor(GameConfiguration.GetCharacter(PlayerSlot.P3));
            case 4:
                return PlayerPicker.GetGhostColor(GameConfiguration.GetCharacter(PlayerSlot.P4));
            case 5:
                return PlayerPicker.GetGhostColor(GameConfiguration.GetCharacter(PlayerSlot.P5));
            default:
                return PlayerPicker.GetGhostColor(PlayerCharacter.COM);
        }
    }

    public PlayerNumber GetFrameLabel(int playerId)
    {
        switch (playerId)
        {
            case 1:
                return PlayerNumber.P1;
            case 2:
                return PlayerNumber.P2;
            case 3:
                return PlayerNumber.P3;
            case 4:
                return PlayerNumber.P4;
            case 5:
                return PlayerNumber.P5;
            default:
                return PlayerNumber.Computer;
        }
    }

    public void UpdateCameras(List<KeyValuePair<int, IGameCamera>> cameras)
    {
        void UpdateViewport(int index, ref GhostViewport source)
        {
            if (cameras.Count >= index + 1)
            {
                source.SetViewport(cameras[index].Value.GetViewport());
                source.player = GetFrameLabel(cameras[index].Key);
                source.cardColor = GetFrameColor(cameras[index].Key);

            }
            else
            {
                source.SetViewport(null);
                source.player = GetFrameLabel(0);
                source.cardColor = GetFrameColor(0);
            }
        }


        if (viewport1 != null) UpdateViewport(0, ref viewport1);
        if (viewport2 != null) UpdateViewport(1, ref viewport2);
        if (viewport3 != null) UpdateViewport(2, ref viewport3);
        if (viewport4 != null) UpdateViewport(3, ref viewport4);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
