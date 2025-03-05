using UnityEngine;

public class SpawnAntagonists : MonoBehaviour
{
    //objects to spawn
    public GameObject robberObject;
    public GameObject footsteps;

    //places to spawn robber
    public Transform firstInstance;
    public Transform secondInstance;  
    public Transform thirdInstance;

    //endzones spawn
    public GameObject endZones;


    // Update is called once per frame
    void Update()
    {
        if(TutorialProgress.part == 1)
        {
            //Instantiate(robberObject, firstInstance.position, Quaternion.identity);
        }


        if(TutorialProgress.part == 2)
        {
            //Instantiate(robberObject, secondInstance.position, Quaternion.identity);
            footsteps.SetActive(true);
        }

        if (TutorialProgress.part == 3)
        {
            //Instantiate(robberObject, thirdInstance.position, Quaternion.identity);
            footsteps.SetActive(false);
        }

        if(TutorialProgress.part == 6)
        {
            endZones.SetActive(true);
        }
    }
}
