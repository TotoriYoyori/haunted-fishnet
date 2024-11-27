using UnityEngine;

public class Footstep : MonoBehaviour
{
    [SerializeField] SpriteRenderer sprite;
    public float left_or_right;
    public float lifetime;
    float current_lifetime;
    float transparency_decrease;
    float transparency = 1f;

    // Start is called before the first frame update
    void Start()
    {
        current_lifetime = 0;
        transparency_decrease = 0.2f * (300f / lifetime);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        current_lifetime++;
        //Debug.Log(lifetime);
        if (current_lifetime % 50 == 0)
        {
            transparency = transparency - transparency_decrease;
            if (current_lifetime == lifetime) Destroy(this.gameObject);
            sprite.color = new Color(1f, 1f, 1f, transparency);
        }

        if (current_lifetime == 1f) Initialize(); // There needs to be a small delay before Initializing, so that rotation vector could be something other than 0 (Otherwise robber and the footstep for a moment are located on the very same point, so it can't rotate towards the robber)

    }

    public void Initialize()
    {

        Vector2 dir = Game.Instance.robber.Value.transform.position - transform.position;
        transform.rotation = Quaternion.FromToRotation(Vector3.up, dir);

        transform.Translate(left_or_right, 0, 0);
    }
}
