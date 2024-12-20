using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Vent : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] Vent[] connected_vents;
    Button[] vent_buttons = new Button[3];
    [SerializeField] Color closed_color;
    [SerializeField] Color blocked_color;
    Color open_color;
    SpriteRenderer sprite;
    [HideInInspector] public bool blocked;
    
    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        open_color = sprite.color;

        // getting all the vent ui buttons and disabling them
        GameObject[] vent_button_gameObjects = GameObject.FindGameObjectsWithTag("VentButton");
        for (int i = 0; i < vent_button_gameObjects.Length; i++)
        {
            vent_buttons[i] = vent_button_gameObjects[i].GetComponent<Button>();
        }
    }

    private void Start()
    {
        // deactivating buttons by default
        if (vent_buttons[0].gameObject.activeSelf == false)
        {
            for (int i = 0; i < vent_buttons.Length; i++)
            {
                vent_buttons[i].gameObject.SetActive(false);
            }
        }
    }

    public void OpenVent(bool is_open)
    {
        if (blocked) return;
        if (CheckToBlock()) BlockVent();

        // changing visuals
        sprite.color = (is_open) ? open_color : closed_color;

        for (int a = 0; a < connected_vents.Length; a++)
        {
            if (a >= vent_buttons.Length) continue;

            vent_buttons[a].gameObject.SetActive(is_open);

            if (!is_open) continue; // we dont need to go further if we simply close vents

            // if the target vent is blocked there's no reason to make an arrow for it
            else if (connected_vents[a].blocked)
            {
                vent_buttons[a].gameObject.SetActive(false);
                continue;
            }

            // setting positions and rotations of the buttons
            vent_buttons[a].transform.position = transform.position;

            Vector3 vent_direction = (connected_vents[a].transform.position - transform.position).normalized;
            vent_buttons[a].transform.rotation = Quaternion.FromToRotation(Vector3.up, vent_direction);

            vent_buttons[a].transform.position += vent_buttons[a].transform.up * 1.5f;

            // Adding functionality to buttons
            vent_buttons[a].onClick.RemoveAllListeners();
            Vent target_vent = connected_vents[a];
            vent_buttons[a].onClick.AddListener(() => MoveVent(target_vent)); 
        }
    }

    public void BlockVent()
    {
        blocked = true;
        this.gameObject.tag = "Untagged";
        sprite.color = blocked_color;
    }

    bool CheckToBlock()
    {
        for (int a = 0; a < connected_vents.Length; a++)
        {
            if (connected_vents[a].blocked == false) return false;
        }

        return true;
    }
    void MoveVent(Vent vent_to_move_to)
    {
        OpenVent(false);

        //SFX
        AudioManager.instance.PlaySFX("Vent");

        // Moving the character, so far its literally just teleporting
        Game.Instance.robber.Value.transform.position = vent_to_move_to.transform.position;

        // block the vent you just moved to
        vent_to_move_to.BlockVent();

        // we need to block this vent if both vents it goes to were blocked
        if (CheckToBlock()) BlockVent();

        // to do: the above can happen when you interact with another vent so make sure to account for that
    }
}
