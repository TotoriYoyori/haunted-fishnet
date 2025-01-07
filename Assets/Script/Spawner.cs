using FishNet.Object;
using FishNet.Connection;
using UnityEngine;
using FishNet.Managing;
using UnityEngine.UI;
using Unity.VisualScripting;

public class Spawner : NetworkBehaviour
{
    [SerializeField] GameObject robber_prefab;
    [SerializeField] GameObject ghost_prefab;
    [SerializeField] character starting_character;
    GameObject new_player;

    // character select
    [SerializeField] Button ghost_button;
    [SerializeField] Button robber_button;
    [SerializeField] GameObject ghost_taken_text;
    [SerializeField] GameObject robber_taken_text;


    public override void OnStartClient()
    {
        base.OnStartClient();

        // Only allow the owner to see and interact with the character selection screen
        
        if (!base.IsOwner)
        {
            ghost_button.gameObject.SetActive(false);
            robber_button.gameObject.SetActive(false);
            //SelfDestruct();
            return;
        }

        Debug.Log("I- Spawner client star");

        ghost_button.onClick.AddListener(() => RequestSpawnGhost());
        robber_button.onClick.AddListener(() => RequestSpawnRobber());
    }

    [ServerRpc(RequireOwnership = false)]
    private void RequestSpawnGhost(NetworkConnection sender = null)
    {
        if (Game.Instance.ghost.Value != null)
        {
            DenyChoiceObserverRpc(false);
            return;
        }
        SpawnPlayer(ghost_prefab, FindSpawnPoint("GhostSpawnpoint"), sender, false);
    }

    [ServerRpc(RequireOwnership = false)]
    private void RequestSpawnRobber(NetworkConnection sender = null)
    {
        if (Game.Instance.robber.Value != null)
        {
            DenyChoiceObserverRpc(true);
            return;
        }
        SpawnPlayer(robber_prefab, FindSpawnPoint("RobberSpawnpoint"), sender, true);
    }

    [ObserversRpc]
    void DenyChoiceObserverRpc(bool is_robber)
    {
        if (!IsOwner) return;

        if (is_robber)
        {
            robber_button.gameObject.SetActive(false);
            robber_taken_text.SetActive(true);
        }
        else
        {
            ghost_button.gameObject.SetActive(false);
            ghost_taken_text.SetActive(true);
        }
    }
    private Vector3 FindSpawnPoint(string spawnpointName)
    {
        GameObject spawnPoint = GameObject.Find(spawnpointName);
        return (spawnPoint != null) ? spawnPoint.transform.position : Vector3.zero;
    }

    void SpawnPlayer(GameObject player_to_spawn, Vector3 position, NetworkConnection owner, bool is_robber)
    {
        new_player = Instantiate(player_to_spawn, position, Quaternion.identity);
        ServerManager.Spawn(new_player, owner);

        UpdatePlayerObserversRpc(is_robber);

        // Setting up player variable on the client
        // Game.Instance.player = new_player.GetComponent<Player>();

        Debug.Log($"Spawned {(is_robber ? "Robber" : "Ghost")} for connection: {owner.ClientId}"); // line by GPT to verify connection

        SelfDestruct();
    }
    void SelfDestruct()
    {
        NetworkObject networkObject = GetComponent<NetworkObject>();
        networkObject.Despawn();
    }
    // theres a polish cafe 
    [ObserversRpc]
    void UpdatePlayerObserversRpc(bool is_robber)
    {
        if (is_robber)
            Game.Instance.robber.Value = new_player;
        else
            Game.Instance.ghost.Value = new_player;

        //if (IsOwner) Game.Instance.player = new_player.GetComponent<Player>();
    }
}
