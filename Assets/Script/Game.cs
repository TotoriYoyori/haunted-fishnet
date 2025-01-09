using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using Newtonsoft.Json.Bson;
using TMPro;
using UnityEditor.PackageManager;
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

    public static GameOverUI game_over;
    public NetworkManager network_manager;

    void Start()
    {
        network_manager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
    }
}
