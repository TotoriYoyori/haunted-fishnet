using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;
using FishNet.Connection;
using FishNet.Object;
using System.Collections;

public class PlayerSpawnTutorial : MonoBehaviour
{
    public GameObject character_select_prefab;


    private void Start()
    {
        
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(2f);
        //SpawnCharacterSelect();
    }
    void SpawnCharacterSelect(NetworkConnection conn)
    {
        GameObject character_select = Instantiate(character_select_prefab);
        Game.Instance.network_manager.ServerManager.Spawn(character_select, conn);

        NetworkObject network_obj = character_select.GetComponent<NetworkObject>();
        Game.Instance.network_manager.SceneManager.AddOwnerToDefaultScene(network_obj);

        //OnSpawned?.Invoke(network_obj);
    }
}
