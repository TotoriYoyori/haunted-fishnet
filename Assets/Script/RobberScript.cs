using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

public class RobberScript : NetworkBehaviour
{
    [HideInInspector]
    public Player player;
    public GameObject flashlight;
    [HideInInspector] public bool items_collected;
    public GameObject item_pick_up_aura;
    
    public void Flashlight(bool is_on)
    {
        if (player.is_special_vision_on) return;
        SyncFlashlightServerRpc(is_on);
        if (IsOwner) player.narrow_dark_filter.SetActive(!is_on);

        if (is_on)
        {
            Debug.Log("FlashlightOn");
        }
        else
        {
            Debug.Log("FlashlightOff");
        }
    }

    public void NightVision(bool is_on)
    {
        player.camera.GetComponent<CameraBehavior>().SpecialVision(is_on, true);
        player.is_special_vision_on = is_on;

        if (IsOwner) player.narrow_dark_filter.SetActive(!is_on);

        if (is_on) SyncFlashlightObserversRpc(false);
    }

    [ServerRpc]
    void SyncFlashlightServerRpc(bool is_on)
    {
        //flashlight.SetActive(is_on); this is unnecessary
        SyncFlashlightObserversRpc(is_on);
    }

    [ObserversRpc]
    void SyncFlashlightObserversRpc(bool is_on)
    {
        flashlight.SetActive(is_on);
        Game.Instance.ghost.Value.GetComponent<Player>().Indication(is_on);
    }
}
