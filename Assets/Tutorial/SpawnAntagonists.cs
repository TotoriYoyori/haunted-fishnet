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




    // Update is called once per frame
    void Update()
    {


        if (TutorialProgress.part == 3)
        {
            Instantiate(robberObject, firstInstance.position, Quaternion.identity);
        }


        if(TutorialProgress.part == 4)
        {
            Instantiate(robberObject, secondInstance.position, Quaternion.identity);
            footsteps.SetActive(true);
        }

        if (TutorialProgress.part == 5)
        {
            Instantiate(robberObject, thirdInstance.position, Quaternion.identity);
            footsteps.SetActive(false);
        }

        if(TutorialProgress.part == 6)
        {
            //ghost wins
        }
    }
}
