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
    }

    // Update is called once per frame
    void Update()
    {
        //tutorial part checker


    }
}
