using FishNet.Object;
using UnityEngine;

public class InputController : NetworkBehaviour
{
    Player player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        player.transform.position += MovementInput();
    }
    Vector3 MovementInput()
    {
        if (player.frozen) return Vector3.zero;

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");


        Vector3 Movement = new Vector3(x, y, 0) * player.speed;
        if (Movement.magnitude > 1) Movement.Normalize();

        return Movement;
    }
    private void Update()
    {
        AbilitiesInput();
    }
    void AbilitiesInput()
    {
        if (TryGetComponent(out RobberScript robber))
        {
            // Flashlight on left mouse
            if (Input.GetButtonDown("Fire1")) robber.NightVision(true);
            if (Input.GetButtonUp("Fire1")) robber.NightVision(false);
            
            // Nightvision on right mouse
            if (Input.GetButtonDown("Fire2")) robber.Flashlight(true);
            if (Input.GetButtonUp("Fire2")) robber.Flashlight(false);

            // to do:add picking up items on SPACE_BAR
            if (Input.GetButtonDown("Jump")) robber.item_pick_up_aura.GetComponent<ItemPickUp>().StartPicking(true);
            if (Input.GetButtonUp("Jump")) robber.item_pick_up_aura.GetComponent<ItemPickUp>().StartPicking(false);
        }
        else if (TryGetComponent(out GhostScript ghost))
        {
            // Dash on left mouse
            if (Input.GetButtonDown("Fire1")) ghost.ChargeAttack(true);
            if (Input.GetButtonUp("Fire1")) ghost.ChargeAttack(false);

            // Stepvision on right mouse
            if (Input.GetButtonDown("Fire2")) ghost.StepVision(true);
            if (Input.GetButtonUp("Fire2")) ghost.StepVision(false);

        }
    }
}
