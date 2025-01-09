using FishNet.Managing.Client;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    public TextMeshProUGUI win_text;
    public TextMeshProUGUI lose_text;
    public TextMeshProUGUI client_wait_text;
    public Button restart_button;
    public Button disconnect_button;
    private void Start()
    {
        Game.game_over = this;

        restart_button.onClick.AddListener(() => Restart());
        disconnect_button.onClick.AddListener(() => Disconnect());
    }
    public void GameOverUIOn(bool won, bool is_host)
    {
        // disconnect button
        disconnect_button.gameObject.SetActive(true);

        // win lose text
        if (won) win_text.gameObject.SetActive(true);
        else lose_text.gameObject.SetActive(true);

        // wait for host / restart
        if (is_host) restart_button.gameObject.SetActive(true);
        else client_wait_text.gameObject.SetActive(true);
    }

    void Restart()
    {
        SceneManager.LoadScene("Lvl_Tilemap", LoadSceneMode.Single);
    }

    private void Disconnect()
    {
        if (Game.Instance.network_manager != null)
        {
            // both client and server can shut down the server upon pressing disconnect
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
            Game.Instance.network_manager.ServerManager.StopConnection(true);
            
        }
        else Debug.Log("Network manager was not found");
    }
}
