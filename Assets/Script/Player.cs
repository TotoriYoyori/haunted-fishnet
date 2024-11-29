using FishNet.Example.ColliderRollbacks;
using FishNet.Object;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : NetworkBehaviour
{
    [HideInInspector]
    public bool frozen;
    [HideInInspector]
    public bool is_special_vision_on;
    public float speed;
    [HideInInspector]
    public Camera camera;
    public GameObject narrow_dark_filter;
    public GameObject wide_dark_filter;

    // indication varialbes
    [SerializeField] GameObject indication;
    [SerializeField] SpriteRenderer indication_sprite;
    [SerializeField] float indication_hide_distance;
    public override void OnStartClient()
    {
        base.OnStartClient();
        if (base.IsOwner)
        {
            // Set up camera
            camera = Camera.main;
            Debug.Log("Camera_supposed_to_be_assigned");
            if (camera == null) Debug.Log("Camera_was_not_assigned");
        
            if (TryGetComponent(out RobberScript robber))
            {
                robber.player = GetComponent<Player>();
                narrow_dark_filter.SetActive(true);
                wide_dark_filter.SetActive(true);
                camera.GetComponent<CameraBehavior>().CameraMode(camera_mode.ROBBER);
                if (IsOwner) GetComponent<FootstepManager>().enabled = false;
            }
            else if (TryGetComponent(out GhostScript ghost))
            {
                ghost.player = GetComponent<Player>(); 
                ghost.default_speed = speed;
                wide_dark_filter.SetActive(true);
                camera.GetComponent<CameraBehavior>().filter.GetComponent<Image>().color = ghost.stepvision_color;
                camera.GetComponent<CameraBehavior>().CameraMode(camera_mode.GHOST);
            }
        }
        else
        {
            GetComponent<InputController>().enabled = false;
        }
    }

    private void Update()
    {
        if (camera != null) camera.GetComponent<CameraBehavior>().to_follow = transform.position;
    }

    private void FixedUpdate()
    {
        IndicationRotation();
    }

    void IndicationRotation()
    {
        // indication
        if (indication.activeSelf && TryGetComponent(out GhostScript ghost))
        {
            Vector3 robber_direction = (Game.Instance.robber.Value.transform.position - transform.position).normalized;
            indication.transform.rotation = Quaternion.FromToRotation(Vector3.up, robber_direction);

            if (Vector2.Distance(Game.Instance.robber.Value.transform.position, transform.position) < indication_hide_distance)
            {
                indication_sprite.enabled = false;
            }
            else indication_sprite.enabled = true;
        }
        else
        {
            if (indication.activeSelf && TryGetComponent(out RobberScript robber))
            {
                Vector3 robber_direction = (Game.Instance.ghost.Value.transform.position - transform.position).normalized;
                indication.transform.rotation = Quaternion.FromToRotation(Vector3.up, robber_direction);

                if (Vector2.Distance(Game.Instance.ghost.Value.transform.position, transform.position) < indication_hide_distance)
                {
                    indication_sprite.enabled = false;
                }
                else indication_sprite.enabled = true;
            }
        }
    } // Optimize this
    public void Indication(bool is_on)
    {
        if (IsOwner) indication.SetActive(is_on);
    }


}

