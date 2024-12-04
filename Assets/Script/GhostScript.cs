using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class GhostScript : NetworkBehaviour
{
    [HideInInspector] public Player player;

    [SerializeField] GameObject aiming_arrow;
    [SerializeField] GameObject ghost_hiding;
    [SerializeField] GameObject ghost_attacking;
    public Color stepvision_color;
    [SerializeField] float stepvision_speed;
    [HideInInspector] public float default_speed;
    

    // Dashing Variables
    bool is_aiming;
    Vector2 mouse_position;
    Vector2 charge_target_position = Vector2.zero;
    float charge_time;
    [SerializeField] float charge_duration;
    [SerializeField] float charge_length;
    Vector3 charge_starting_position;

    // Catching Variables
    [SerializeField] float laughing_duration;
    bool is_laughing = false;
    List<Vector3> teleportation_locations = new List<Vector3>();

    public override void OnStartClient()
    {
        base.OnStartClient();

        Debug.Log("Ghost OnStartClient");

        FindTeleportationPoint(GameObject.Find("teleportation_point_1"));
        FindTeleportationPoint(GameObject.Find("teleportation_point_2"));
        FindTeleportationPoint(GameObject.Find("teleportation_point_3"));
        FindTeleportationPoint(GameObject.Find("teleportation_point_4"));
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

            mouse_position = player.camera.ScreenToWorldPoint(screen_mouse_position);

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
        float progress = ((charge_time / charge_duration) > 1f) ? 1 : charge_time / charge_duration;
        Debug.Log(progress);

        float coolT = Mathf.Pow(progress, 2);

        player.transform.position = Vector3.Lerp(charge_starting_position, charge_target_position, coolT);

        if (progress == 1) EndCharge();
    }

    void StartCharge()
    {
        charge_target_position = transform.position + aiming_arrow.transform.up * charge_length;
        charge_starting_position = transform.position;
        charge_time = 0f;
        SyncHideServerRpc(false);
        //Hide(false);

        Debug.Log("Charge Started");
    }

    void EndCharge()
    {
        charge_target_position = Vector3.zero;
        SyncHideServerRpc(true);
        //Hide(true);
        Debug.Log("Charge Ended");
    }
    public void ChargeAttack(bool is_on)
    {
        is_aiming = is_on;
        aiming_arrow.SetActive(is_aiming);
        SyncHideServerRpc(!is_on);
        //Hide(!is_on);

        if (is_on == false && Vector2.Distance(mouse_position, transform.position) > 0f)
        {
            StartCharge();
        }

    }

    public void StepVision(bool is_on)
    {
        player.camera.GetComponent<CameraBehavior>().SpecialVision(is_on, false);

        player.speed = (is_on) ? stepvision_speed : default_speed;
    }

    // Changing states HIDING - ATTACKING ======================================

    [ServerRpc]
    public void SyncHideServerRpc(bool is_hiding)
    {
        SyncHideObserversRpc(is_hiding);
    }

    [ObserversRpc]
    public void SyncHideObserversRpc(bool is_hiding)
    {
        if (IsOwner) player.frozen = !is_hiding;
        ghost_hiding.SetActive(is_hiding);
        ghost_attacking.SetActive(!is_hiding);
        Game.Instance.robber.Value.GetComponent<Player>().Indication(!is_hiding);
    }

    // ==========================================================================

    [ServerRpc]
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
    void TeleportAway()
    {
        Debug.Log("TeleportsAway");
    }
    IEnumerator Catch()
    {
        Debug.Log("CATCHING: I caught the robber (Observer)");
        
        is_laughing = true;
        SpriteRenderer sprite = ghost_hiding.GetComponent<SpriteRenderer>();
        Color color = sprite.color;
        player.frozen = true;
        float currect_laughing_duration = laughing_duration;
        float current_alpha = color.a;

        while (currect_laughing_duration > 0)
        {
            Debug.Log(current_alpha);
            sprite.color = new Color(color.r, color.g, color.b, current_alpha);
            current_alpha = currect_laughing_duration / laughing_duration;

            currect_laughing_duration--;
            yield return new WaitForSeconds(0.1f);
        }

        sprite.color = color;
        player.frozen = false;
        is_laughing = false;

        TeleportAway();        
    }
}
