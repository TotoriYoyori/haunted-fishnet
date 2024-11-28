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


}

