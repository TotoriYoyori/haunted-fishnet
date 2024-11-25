using FishNet.Example.ColliderRollbacks;
using FishNet.Object;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [HideInInspector]
    public bool frozen;
    public float speed;
    Camera camera;
    public override void OnStartClient()
    {
        base.OnStartClient();
        if (base.IsOwner)
        {
            // Set up camera
            camera = Camera.main;
        }
        else
        {
            gameObject.GetComponent<InputController>().enabled = false;
        }
    }

    private void Update()
    {
        camera.GetComponent<CameraBehavior>().to_follow = transform.position;
    }


}

