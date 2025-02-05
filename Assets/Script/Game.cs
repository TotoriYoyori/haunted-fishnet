using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using Newtonsoft.Json.Bson;
using TMPro;
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

        // activating loading screen
        if (loading_screen != null) loading_screen.SetActive(true);
    }
    //public static GameObject robber;
    public readonly SyncVar<GameObject> robber = new SyncVar<GameObject>();
    public readonly SyncVar<GameObject> ghost = new SyncVar<GameObject>();
    //public static GameObject ghost;
    public Player player;
    public static Level level;

    public static GameOverUI game_over;
    public NetworkManager network_manager;
    public GameObject loading_screen;
    public ItemLottery item_lottery;

    void Start()
    {
        network_manager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
    }
}
