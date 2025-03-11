using UnityEngine;

public class SpawnGhostAntagonists : MonoBehaviour
{
    //objects to spawn
    public GameObject ghostObject;


    //places to spawn robber
    public Transform firstInstance;

    //endzones spawn
    public GameObject endZones;


    // Update is called once per frame
    void Update()
    {
        if (TutorialProgress.part == 4)
        {
            Instantiate(ghostObject, firstInstance.position, Quaternion.identity);
        }

        if(TutorialProgress.part == 7)
        {
            endZones.SetActive(true);
        }

    }
}
