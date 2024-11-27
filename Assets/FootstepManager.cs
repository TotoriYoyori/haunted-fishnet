using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

public class FootstepManager : NetworkBehaviour
{
    [SerializeField] int footstep_interval;
    [SerializeField] int footstep_lifetime;
    int footstep_time;
    private readonly SyncVar<bool> is_walking = new SyncVar<bool>();

    [SerializeField] GameObject footstep;
    GameObject new_footstep;
    float adj = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        footstep_time = footstep_interval;
    }

    private void Update()
    {
        Debug.Log(is_walking.Value);
        if (is_walking.Value)
        {
            FootstepCreationCheck();
        }
    }
    public void FootstepCreationCheck()
    {
        footstep_time--;
        if (footstep_time < 1)
        {
            new_footstep = Instantiate(footstep, transform.position, Quaternion.identity);

            new_footstep.GetComponent<Footstep>().left_or_right = adj;
            new_footstep.GetComponent<Footstep>().lifetime = footstep_lifetime;

            footstep_time = footstep_interval;
            adj = adj * -1;
        }
    }

    [ServerRpc]
    public void SetIsWalkingServerRpc(bool walking) => is_walking.Value = walking;
   
}
