using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    [HideInInspector]
    public Vector2 to_follow;
    [SerializeField] float speed;
    
    void FixedUpdate()
    {
        Follow();
    }
    void Follow()
    {
        if (to_follow == null) return;
        Vector2 new_position = new Vector2(to_follow.x, to_follow.y);
        transform.position = Vector2.Lerp(transform.position, to_follow, speed);
        transform.position += new Vector3(0, 0, -10f); // Fix this!!!
    }
}
