using FishNet.Object;
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

}
