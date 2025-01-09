using FishNet.Managing;
using FishNet.Managing.Server;
using FishNet.Object;
using FishNet.Transporting.Tugboat;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    NetworkManager network;
    Tugboat tugboat;
    [SerializeField] TMP_InputField input_field;
    [SerializeField] TMP_Text server_button_text;
    [SerializeField] Color server_text_color;
    bool server_created = false;

    private void Start()
    {
        network = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        tugboat = network.gameObject.GetComponent<Tugboat>();
    }
    public void ServerCreate()
    {
        if (server_created) return;
        Debug.Log("Server button works");
        network.ServerManager.StartConnection();

        server_created = true;

        if (server_created)
        {
            server_button_text.color = server_text_color;
            server_button_text.text = "Server created";
        }
    }

    public void ClientJoin()
    {
        ChooseIP(); // get the ip from the Input Field

        Debug.Log("Client Button works");
        network.ClientManager.StartConnection();

        SceneManager.LoadScene("Lvl_Tilemap", LoadSceneMode.Single);
    }

    public void ChooseIP()
    {
        if (input_field.text == "")
        {
            tugboat.SetClientAddress("localhost");
        }
        else
        {
            tugboat.SetClientAddress(input_field.text);
        }
       
        Debug.Log(tugboat.GetClientAddress());
    }

    void Update()
    {
        // "Fast dial" ip adresses 
        if (Input.GetKeyDown(KeyCode.I)) // Igor's
        {
            input_field.text = "192.168.195.190";
        }
        else if (Input.GetKeyDown(KeyCode.J)) // Joel's
        {
            input_field.text = "192.168.195.36";
        }
    }
}
