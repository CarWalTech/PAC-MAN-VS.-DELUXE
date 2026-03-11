using UnityEngine;
using UnityEngine.Tilemaps;

public interface IPlayable
{
    public void Setup(GameManager.SpawnData spawnData, Maze2D mazeData);
    public void Freeze();
    public void Unfreeze();
    public void Lock();
    public bool IsLocked();
    public void Unlock();
    public float GetSightRange();

    public PlayerSlot GetPlayerID();
    public void SetPlayerID(PlayerSlot slot);
    public void ResetState();
    public GameObject GetMazeObject();
    public PlayerType GetPlayerType();
    public GameObject GetWorldObject();
    public void TeleportTo(Vector2 newWorldPos, Vector2 outDirection);
}