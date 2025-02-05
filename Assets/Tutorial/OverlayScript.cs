using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class OverlayScript : MonoBehaviour
{
    public bool tutorial_part_1;
    public bool tutorial_part_2;
    public bool tutorial_part_3;
    public bool tutorial_part_4;
    public bool tutorial_part_5;
    public bool tutorial_part_6;
    public bool tutorial_part_7;

    private bool show = false;

    void Update()
    {
        
        Deinitialise();
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    void Deinitialise()
    {
        if (tutorial_part_1 && TutorialProgress.part == 1)  { show = true; }
        else if (tutorial_part_2 && TutorialProgress.part == 2) { show = true; }
        else if (tutorial_part_3 && TutorialProgress.part == 3) { show = true; }
        else if (tutorial_part_4 && TutorialProgress.part == 4) { show = true; }
        else if (tutorial_part_5 && TutorialProgress.part == 5) { show = true; }
        else if (tutorial_part_6 && TutorialProgress.part == 6) { show = true; }
        else if (tutorial_part_7 && TutorialProgress.part == 7) { show = true; }

        if (tutorial_part_1 && TutorialProgress.part != 1) { show = false; }
        else if (tutorial_part_2 && TutorialProgress.part != 2) { show = false; }
        else if (tutorial_part_3 && TutorialProgress.part != 3) { show = false; }
        else if (tutorial_part_4 && TutorialProgress.part != 4) { show = false; }
        else if (tutorial_part_5 && TutorialProgress.part != 5) { show = false; }
        else if (tutorial_part_6 && TutorialProgress.part != 6) { show = false; }
        else if (tutorial_part_7 && TutorialProgress.part == 7) { show = true; }


        if (show) { gameObject.SetActive(true); }
        else { gameObject.SetActive(false); }
    }


}
