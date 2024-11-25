using UnityEngine;

public class InputController : MonoBehaviour
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
}
