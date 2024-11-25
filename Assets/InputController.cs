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
        }



        /*
        // Left Mouse Abilities (NightVision and ChargeAttack)
        if (Input.GetButtonDown("Fire1"))
        {
            if (is_robber) this_robber.GetComponent<RobberScript>().NightVision(true);
            else this_ghost.GetComponent<GhostScript>().ChargeAttack(true);
        }
        if (Input.GetButtonUp("Fire1"))
        {
            if (is_robber) this_robber.GetComponent<RobberScript>().NightVision(false);
            else this_ghost.GetComponent<GhostScript>().ChargeAttack(false);
        }

        // Right Mouse Abilities (Flashlight and StepVision)
        if (Input.GetButtonDown("Fire2"))
        {
            if (is_robber) this_robber.GetComponent<RobberScript>().Flashlight(true);
            else this_ghost.GetComponent<GhostScript>().StepVision(true);
        }
        if (Input.GetButtonUp("Fire2"))
        {
            if (is_robber) this_robber.GetComponent<RobberScript>().Flashlight(false);
            else this_ghost.GetComponent<GhostScript>().StepVision(false);
        }

        // C button abilities (Item pickUp and walking through walls)
        if (Input.GetButtonDown("Jump"))
        {
            if (is_robber) this_robber.GetComponent<RobberScript>().ItemPickUpAura.GetComponent<ItemPickUp>().StartPicking(true);
            //else GetComponent<GhostScript>().StepVision(false);
        }
        if (Input.GetButtonUp("Jump"))
        {
            if (is_robber) this_robber.GetComponent<RobberScript>().ItemPickUpAura.GetComponent<ItemPickUp>().StartPicking(false);
            //else GetComponent<GhostScript>().StepVision(false);
        }*/
    }
}
