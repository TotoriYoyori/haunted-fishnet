using UnityEngine;

public class SpawnGhostAntagonists : MonoBehaviour
{
    //objects to spawn
    public GameObject ghostObject;


    //places to spawn robber
    public Transform firstInstance;



    // Update is called once per frame
    void Update()
    {
        if (TutorialProgress.part == 1)
        {
            //Instantiate(ghostObject, firstInstance.position, Quaternion.identity);
        }

    }
}
