using FishNet.Object;
using FishNet.Connection;
using UnityEngine;
using FishNet.Managing;

public class Spawner : NetworkBehaviour
{
    [SerializeField] GameObject robber_prefab;
    [SerializeField] GameObject ghost_prefab;
    [SerializeField] character starting_character;
    GameObject new_player;
    
    public override void OnStartServer()
    {
        base.OnStartServer();

        
        if (starting_character == character.ROBBER) // Order is based on the starting character specified in game.
        {
            if (Game.Instance.robber.Value == null) SpawnRobber();
            else if (Game.Instance.ghost.Value == null) SpawnGhost();
        }
        else if (starting_character == character.GHOST)
        {
            if (Game.Instance.ghost.Value == null) SpawnGhost();
            else if (Game.Instance.robber.Value == null) SpawnRobber();
        }
        
        else
        {
            Debug.Log("No more players needed");
        }

        SelfDestruct();
    }

    void SpawnRobber()
    {
        NetworkConnection ownerConnection = GetComponent<NetworkObject>().Owner;

        Debug.Log("spawning a robber");
        Vector3 spawn_position = (GameObject.Find("RobberSpawnpoint") == null) ? Vector3.zero : GameObject.Find("RobberSpawnpoint").transform.position;
        SpawnPlayer(robber_prefab, spawn_position, ownerConnection, true);
    }
    void SpawnGhost()
    {
        NetworkConnection ownerConnection = GetComponent<NetworkObject>().Owner;

        Debug.Log("spawning a ghost");
        Vector3 spawn_position = (GameObject.Find("GhostSpawnpoint") == null) ? Vector3.zero : GameObject.Find("GhostSpawnpoint").transform.position;
        SpawnPlayer(ghost_prefab, spawn_position, ownerConnection, false);
    }
    void SpawnPlayer(GameObject player_to_spawn, Vector3 position, NetworkConnection owner, bool is_robber)
    {
        new_player = Instantiate(player_to_spawn, position, Quaternion.identity);
        ServerManager.Spawn(new_player, owner);

        UpdatePlayerObserversRpc(is_robber);

        Debug.Log($"Spawned {(is_robber ? "Robber" : "Ghost")} for connection: {owner.ClientId}"); // line by GPT to verify connection

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
    }
}
