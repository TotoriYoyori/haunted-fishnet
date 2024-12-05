using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.Rendering;

public class RobberScript : NetworkBehaviour
{
    [HideInInspector]
    public Player player;
    public GameObject flashlight;
    [HideInInspector] public bool items_collected;
    public GameObject item_pick_up_aura;

    // Shake variables
    [SerializeField] float radar_range;
    [SerializeField] float white_noise_range;
    [SerializeField] float shake_intensity;
    [SerializeField] float white_noise_volume;

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

    private void Update()
    {
        GhostRadar();
    }

    void GhostRadar()
    {
        if (Game.Instance.ghost.Value == null && !IsOwner) return;
        float distance_to_ghost = Vector2.Distance(Game.Instance.ghost.Value.transform.position, transform.position);

        // Shaking darkness effect
        player.narrow_dark_filter.transform.localPosition = ShakeEffect(distance_to_ghost);
        player.wide_dark_filter.transform.localPosition = ShakeEffect(distance_to_ghost);

        // Growing Audio effect
        AudioSource white_noise = GetComponent<AudioSource>();
        if (distance_to_ghost > white_noise_range) white_noise.volume = 0;
        else white_noise.volume = (white_noise_range - distance_to_ghost) * white_noise_volume / white_noise_range;
    }

    Vector2 ShakeEffect(float distance_to_ghost)
    {
        // No shake at all if ghost is too far away
        if (distance_to_ghost > radar_range) return Vector2.zero;

        float distance_modifier = (radar_range - distance_to_ghost);
        float magnitude = (distance_modifier * shake_intensity / 50f);

        float x = Random.Range(-1f, 1f) * magnitude;
        float y = Random.Range(-1f, 1f) * magnitude;

        return new Vector2(x, y);

    }

    [Server]
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ghost")
        {
            SyncCatchRobberServerRpc();
        }
    }

    // Synchronizing using the flashlight ===========================================

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

    // ===============================================================================

    [ServerRpc]
    void SyncCatchRobberServerRpc()
    {
        SyncCatchRobberObserverRpc();
    }

    [ObserversRpc]
    void SyncCatchRobberObserverRpc()
    {
        CaughthRobber();
        //Game.Instance.ghost.Value.GetComponent<GhostScript>().Catch();
    }
    void CaughthRobber()
    {
        Debug.Log("CATCHING: I was caught (Observer)");
        GameObject.Find("Ghost(Clone)").GetComponent<GhostScript>().SyncCatchServerRpc();
    }
}
