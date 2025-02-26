using UnityEngine;

public class RoomCheckScript : MonoBehaviour
{
    public bool tutorial_part_1;
    public bool tutorial_part_2;
    public bool tutorial_part_3;
    public bool tutorial_part_4;
    public bool tutorial_part_5;
    public bool tutorial_part_6;
    public bool tutorial_part_7;

    bool kill = false;

    // Update is called once per frame
    void Update()
    {
        Deinitialise();
    }

    void Deinitialise()
    {
        if (tutorial_part_1 && TutorialProgress.part == 2) { kill = true; }
        else if (tutorial_part_2 && TutorialProgress.part == 3) { kill = true; }
        else if (tutorial_part_3 && TutorialProgress.part == 4) { kill = true; }
        else if (tutorial_part_4 && TutorialProgress.part == 5) { kill = true; }
        else if (tutorial_part_5 && TutorialProgress.part == 6) { kill = true; }
        else if (tutorial_part_6 && TutorialProgress.part == 7) { kill = true; }


        if (kill) { gameObject.SetActive(false); }
    }


    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.CompareTag("Player"))
        {
            //checking the position of the ghost in the tutorial
            if (collider.gameObject.TryGetComponent(out ghostTutorial component))
            {
                component.throughWall = true;
            }

            if (tutorial_part_1 && TutorialProgress.part == 1) { TutorialProgress.part = 2; }
            else if (tutorial_part_2 && TutorialProgress.part == 2) { TutorialProgress.part = 3; }
            else if (tutorial_part_3 && TutorialProgress.part == 3) { TutorialProgress.part = 4; }
            else if (tutorial_part_4 && TutorialProgress.part == 4) { TutorialProgress.part = 5; }
            else if (tutorial_part_5 && TutorialProgress.part == 5) { TutorialProgress.part = 6; }
            else if (tutorial_part_6 && TutorialProgress.part == 6) { TutorialProgress.part = 7; }
        }
    }

}
