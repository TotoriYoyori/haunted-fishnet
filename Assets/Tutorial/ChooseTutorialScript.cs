using UnityEngine;
using UnityEngine.SceneManagement;

public class ChooseTutorialScript : MonoBehaviour
{

    public void RobberTutorial()
    {
        //reset the tutorial stage in static script
        TutorialProgress.part = 1;

        SceneManager.LoadScene("Lvl_Tutorial_Robber", LoadSceneMode.Single);
    }

    public void GhostTutorial()
    {
        //reset the tutorial stage in static script
        TutorialProgress.part = 1;

        SceneManager.LoadScene("Lvl_Tutorial_Ghost", LoadSceneMode.Single);
    }

}
