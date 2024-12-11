using FishNet.Object;
using FishNet.Object.Synchronizing;
using Newtonsoft.Json.Bson;
using UnityEngine;
public enum character
{
    ROBBER,
    GHOST,
}
public class Game : NetworkBehaviour
{
    public static Game Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    //public static GameObject robber;
    public readonly SyncVar<GameObject> robber = new SyncVar<GameObject>();
    public readonly SyncVar<GameObject> ghost = new SyncVar<GameObject>();
    //public static GameObject ghost;
    public Player player;
    public static Level level;

    [SerializeField] GameObject RobberWinText;
    [SerializeField] GameObject GhostWinText;

    [ServerRpc(RequireOwnership = false)]
    public void RobberWinsServerRpc()
    {
        RobberWinsObserverRpc();
    }

    [ObserversRpc]
    void RobberWinsObserverRpc()
    {
        GameOverObserverRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void GhostWinsServerRpc()
    {
        GhostWinsObserverRpc();
    }

    [ObserversRpc]
    void GhostWinsObserverRpc()
    {
        GameOverObserverRpc();
    }

    [ObserversRpc]
    void GameOverObserverRpc()
    {
        player.frozen = true;
    }
}
