using FishNet.Object;
using UnityEngine;

public class RobberScript : NetworkBehaviour
{
    [HideInInspector]
    public Player player;
    [SerializeField] GameObject flashlight;
    
    void Update()
    {
        
    }
    public void Flashlight(bool is_on)
    {
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

        if (IsOwner) player.narrow_dark_filter.SetActive(!is_on);
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
    }
}
