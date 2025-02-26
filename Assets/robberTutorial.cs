using TMPro;
using UnityEngine;

public class robberTutorial : MonoBehaviour
{
    //how many items did the robber get
    public float itemsGathered; 

    //did the player test every direction of movement
    public bool goUp;
    public bool goDown;
    public bool goLeft;
    public bool goRight;

    //did the player use flashlight
    public bool flashlight;

    //did the robber loose a life
    public bool lossOfLife;





    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        itemsGathered = 0;

        goUp = false;
        goDown = false;
        goLeft = false;
        goRight = false;

        flashlight = false;

        lossOfLife = false;
    }

    // Update is called once per frame
    void Update()
    {
        //tracking the progress of the part of the tutorial




    }
}
