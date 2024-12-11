using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;
using System.Collections;
using UnityEngine.Rendering;
using System.Runtime.CompilerServices;

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

    // Beign caught variables 
    [SerializeField] float jumpscare_duration;
    bool is_caught = false;
    [SerializeField] GameObject jumpscare;

    public void Flashlight(bool is_on)
    {
        if (player.is_special_vision_on) return;
        SyncFlashlightServerRpc(is_on);
        if (IsOwner) player.narrow_dark_filter.SetActive(!is_on);

        if (is_on)
        {
            //SFX
            AudioManager.instance.PlaySFX("FlashlightOn");

            Debug.Log("FlashlightOn");
        }
        else
        {
            //SFX
            AudioManager.instance.PlaySFX("FlashlightOff");

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
        if (Game.Instance.ghost.Value == null || !IsOwner || player == null) return;
        float distance_to_ghost = Vector2.Distance(Game.Instance.ghost.Value.transform.position, transform.position);

        // Shaking darkness effect
        if (player.narrow_dark_filter != null) player.narrow_dark_filter.transform.localPosition = ShakeEffect(distance_to_ghost);
        if (player.wide_dark_filter != null) player.wide_dark_filter.transform.localPosition = ShakeEffect(distance_to_ghost);

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsServer && collision.gameObject.tag == "Ghost")
        {
            SyncCatchRobberServerRpc();
        }

        if (collision.gameObject.tag == "EscapeZone" && items_collected)
        {
            player.won = true;
            player.GameOverServerRpc(true);
            if (Game.Instance.ghost.Value != null) Game.Instance.ghost.Value.GetComponent<Player>().GameOverServerRpc(false);
        }
    }

    // Synchronizing using the flashlight ===========================================

    [ServerRpc(RequireOwnership = false)]
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

    [ServerRpc(RequireOwnership = false)]
    void SyncCatchRobberServerRpc()
    {
        SyncCatchRobberObserverRpc();
    }

    [ObserversRpc]
    void SyncCatchRobberObserverRpc()
    {
        CaughthRobber();
        
    }
    void CaughthRobber()
    {
        Debug.Log("CATCHING: I was caught (Observer)");
        //if (IsOwner) GameObject.Find("Ghost(Clone)").GetComponent<GhostScript>().SyncCatchServerRpc();
        Game.Instance.ghost.Value.GetComponent<GhostScript>().SyncCatchServerRpc();

        if (IsOwner) StartCoroutine(GetSpooked());
    }

    IEnumerator GetSpooked() 
    {
        is_caught = true;
        float alpha = 1f;
        float current_jumpscare_duration = jumpscare_duration;
        SpriteRenderer sprite = jumpscare.GetComponent<SpriteRenderer>();
        jumpscare.SetActive(true);

        //HP blinking
        if (IsOwner && player.is_blinking == false) StartCoroutine(player.BlinkingLives());

        while (current_jumpscare_duration > 0)
        {
            alpha = current_jumpscare_duration / jumpscare_duration;
            sprite.color = new Color(1, 1, 1, alpha);

            current_jumpscare_duration--;
            yield return new WaitForSeconds(0.1f);
        }

        jumpscare.SetActive(false);
        is_caught = false;
    }
}
