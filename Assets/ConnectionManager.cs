using UnityEngine;
using FishNet.Object;
using UnityEngine.UIElements;
using FishNet.Connection;
using System.Collections;
using UnityEditor.MemoryProfiler;

public class ConnectionManager : NetworkBehaviour
{
    [SerializeField] GameObject character_select;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    override public void OnStartServer()
    {
        base.OnStartServer();
        StartCoroutine(EnableCharacterSelectScreen());


        // this whole thing only works on servers side !!! 
    }

    IEnumerator EnableCharacterSelectScreen()
    {
        yield return new WaitForSeconds(1.0f);

        Debug.Log("I- Trying to spawn a player");
        GameObject new_player = Instantiate(character_select);
        ServerManager.Spawn(new_player, LocalConnection);
        //EnableCharacterSelectScreenServerRpc(LocalConnection);
    }

    [ServerRpc(RequireOwnership = false)]
    void EnableCharacterSelectScreenServerRpc(NetworkConnection connection)
    {
        GameObject new_player = Instantiate(character_select);
        ServerManager.Spawn(new_player, connection);
    }
}
