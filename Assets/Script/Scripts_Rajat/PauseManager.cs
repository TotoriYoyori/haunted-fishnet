using UnityEngine;

public class PauseManager : MonoBehaviour
{

    private bool isPaused = false;
    public GameObject pausePanel;

    void Start()
    {

    }

    void Update()
    {
        // The pause menu code, currently it disables the script for the robber, but it should disable the script of the current player.
        // TODO: Fix the player instance from the Game class
        // TODO: Change the way input is handled later

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pausePanel.SetActive(!pausePanel.activeSelf);
            Game.Instance.robber.Value.GetComponent<InputController>().enabled = false;
            isPaused = !isPaused;
        }

        // This means that the updates will stop if game is paused
        //if (isPaused) return;

    }

    public void Resume()
    {
        isPaused = false;
        pausePanel.SetActive(false);
    }
}