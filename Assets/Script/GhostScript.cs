using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

public class GhostScript : NetworkBehaviour
{
    [HideInInspector] public Player player;

    [SerializeField] GameObject aiming_arrow;
    [SerializeField] GameObject ghost_hiding;
    [SerializeField] GameObject ghost_attacking;
    public Color stepvision_color;
    [SerializeField] float stepvision_speed;
    [HideInInspector] public float default_speed;

    // GhostUI
    GhostUI ghostUI;
    [SerializeField] float dash_cooldown;
    [SerializeField] float stepvision_cooldown;

    // Dashing Variables
    bool is_aiming;
    bool is_dashing;
    Vector2 mouse_position;
    Vector2 charge_target_position = Vector2.zero;
    float charge_time;
    [SerializeField] float dash_duration;
    [SerializeField] float dash_length;
    [SerializeField] float dash_delay_time;
    Vector3 charge_starting_position;

    // Catching Variables
    [SerializeField] float laughing_duration;
    bool is_laughing = false;
    List<Vector3> teleportation_locations = new List<Vector3>();
    SpriteRenderer hiding_sprite;
    Color hiding_color;

    public override void OnStartClient()
    {
        base.OnStartClient();
        Debug.Log("Ghost OnStartClient");

        // Temporarily turning off music for the ghost so that theres only one music that plays
        if (IsOwner) AudioManager.instance.musicSource.gameObject.SetActive(false);
        

        FindTeleportationPoint(GameObject.Find("teleportation_point_1"));
        FindTeleportationPoint(GameObject.Find("teleportation_point_2"));
        FindTeleportationPoint(GameObject.Find("teleportation_point_3"));
        FindTeleportationPoint(GameObject.Find("teleportation_point_4"));

        hiding_sprite = ghost_hiding.GetComponent<SpriteRenderer>();
        hiding_color = hiding_sprite.color;

        if (IsOwner)
        {
            ghostUI = GameObject.Find("GhostUI").GetComponent<GhostUI>();
            if (ghostUI == null) Debug.Log("Couldnt find ghost UI");
            else ghostUI.EnableUI();
        }
    }

    void FindTeleportationPoint(GameObject teleport_point)
    {
        if (teleport_point == null) return;

        teleportation_locations.Add(teleport_point.transform.position);
        Debug.Log("Teleportation pont added at " + teleport_point.transform.position);
    }
    private void FixedUpdate()
    {
        if (IsOwner)
        {
            Vector3 screen_mouse_position = Input.mousePosition;

            mouse_position = player.main_camera.ScreenToWorldPoint(screen_mouse_position);

            if (is_aiming) AimForCharge(mouse_position);
            if (charge_target_position != Vector2.zero) Charging();
        }


    }
    void AimForCharge(Vector2 target_position)
    {
        Vector2 direction = new Vector3(target_position.x, target_position.y, 0) - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        aiming_arrow.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    void Charging()
    {
        charge_time += Time.deltaTime;
        float progress = ((charge_time / dash_duration) > 1f) ? 1 : charge_time / dash_duration;
        //Debug.Log(progress);

        float coolT = Mathf.Pow(progress, 2);

        player.transform.position = Vector3.Lerp(charge_starting_position, charge_target_position, coolT);

        if (progress == 1) EndCharge();
    }

    IEnumerator StartCharge()
    {
        //SFX
        AudioManager.instance.PlaySFXGlobal("GhostWarp");

        SyncHideServerRpc(false);
        is_aiming = false;

        float dash_delay_timer = dash_delay_time;

        while (dash_delay_timer > 0)
        {
            dash_delay_timer -= Time.deltaTime;

            yield return new WaitForSeconds(Time.deltaTime);
        }

        // Dash start soundeffect
        AudioManager.instance.PlaySFXGlobal("Dash");

        charge_target_position = transform.position + aiming_arrow.transform.up * dash_length;
        charge_starting_position = transform.position;
        charge_time = 0f;

        // Cooldown
        StartCoroutine(ghostUI.Cooldown(dash_cooldown, true));

        Debug.Log("Charge Started");
    } 

    void EndCharge()
    {
        aiming_arrow.SetActive(false);
        charge_target_position = Vector3.zero;
        SyncHideServerRpc(true);

        //Hide(true);
        Debug.Log("Charge Ended");
    }
    public void ChargeAttack(bool is_on)
    {
        if (ghostUI.dash_fill.fillAmount < 1 || is_dashing) return;

        is_aiming = is_on;
        aiming_arrow.SetActive(is_aiming);

        if (is_on == false && Vector2.Distance(mouse_position, transform.position) > 0f)
        {
            StartCoroutine(StartCharge());
        }
    }

    public void StepVision(bool is_on)
    {
        if (ghostUI.stepvision_fill.fillAmount < 1) return;

        // Cooldown
        if (!is_on) StartCoroutine(ghostUI.Cooldown(stepvision_cooldown, false));

        player.main_camera.GetComponent<CameraBehavior>().SpecialVision(is_on, false);

        player.speed = (is_on) ? stepvision_speed : default_speed;
    }

    // Changing states HIDING - ATTACKING ======================================

    [ServerRpc(RequireOwnership = false)]
    public void SyncHideServerRpc(bool is_hiding)
    {
        SyncHideObserversRpc(is_hiding);
    }

    [ObserversRpc]
    public void SyncHideObserversRpc(bool is_hiding)
    {
        if (IsOwner) player.frozen = !is_hiding; // To fix, make this not unfreeze the ghost when it dashes and catches the robber and when robber gets
        ghost_hiding.SetActive(is_hiding);
        ghost_attacking.SetActive(!is_hiding);
        is_dashing = !is_hiding;
        Game.Instance.robber.Value.GetComponent<Player>().Indication(!is_hiding);
    }

    // ==========================================================================

    [ServerRpc(RequireOwnership = false)]
    public void SyncCatchServerRpc()
    {
        SyncCatchObserverRpc();
    }

    [ObserversRpc]
    public void SyncCatchObserverRpc()
    {
        if (is_laughing) return;
        StartCoroutine(Catch());
    }
    Vector2 TeleportAway()
    {
        Debug.Log("TeleportsAway");

        int chosen_point = 0;

        for (int i = 0; i < teleportation_locations.Count; i++)
        {
            float old_distance = Vector2.Distance(transform.position, teleportation_locations[chosen_point]);
            float new_distance = Vector2.Distance(transform.position, teleportation_locations[i]);
            if (new_distance > old_distance) chosen_point = i;
        }

        return teleportation_locations[chosen_point];
    }
    IEnumerator Catch()
    {
        Debug.Log("CATCHING: I caught the robber (Observer)");

        //SFX
        AudioManager.instance.PlaySFX("GhostLaugh");

        is_laughing = true;
        ghost_hiding.layer = LayerMask.NameToLayer("Default");
        if (player != null) player.frozen = true;
        float current_laughing_duration = laughing_duration;
        float current_alpha = hiding_color.a;

        // Bliknking collected souls
        if (IsOwner) StartCoroutine(player.BlinkingLives());

        while (current_laughing_duration > 0)
        {
            hiding_sprite.color = new Color(hiding_color.r, hiding_color.g, hiding_color.b, current_alpha);
            current_alpha = current_laughing_duration / laughing_duration;

            current_laughing_duration--;
            yield return new WaitForSeconds(0.1f);
        }

        hiding_sprite.color = hiding_color;
        if (player != null) player.frozen = false;
        ghost_hiding.layer = LayerMask.NameToLayer("Ghost");
        is_laughing = false;

        transform.position = TeleportAway();        
    }
}
