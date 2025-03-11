using UnityEngine;

public class ghostTutorial : MonoBehaviour
{
    //how many times the ghost has caugth the robber
    public float robberKills;

    //did the player test every direction of movement
    public bool goUp;
    public bool goDown;
    public bool goLeft;
    public bool goRight;

    //did the player use step vision
    public bool stepVision;

    //accessed the closed room
    public bool throughWall;
    public bool finishSteps;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        robberKills = 0;

        goUp = false;
        goDown = false;
        goLeft = false;
        goRight = false;

        stepVision = false;

        throughWall = false;
        finishSteps = false;
    }

    // Update is called once per frame
    void Update()
    {
        //tracking the progress of the part of the tutorial

        //part 1
        if (goUp && goDown && goLeft && goRight)
        {
            TutorialProgress.part = 2;
        }

        //part 2

        if (throughWall)
        {
            TutorialProgress.part = 3;
        }


        //part 3
        if (robberKills == 1)
        {
            TutorialProgress.part = 4;
        }

        //part 4
        if (finishSteps)
        {
            TutorialProgress.part = 5;
        }

        //part 5
        if (robberKills == 2)
        {
            TutorialProgress.part = 6;
        }

        //part 6
        if (robberKills == 3)
        {
            TutorialProgress.part = 7;
        }

    }
}
