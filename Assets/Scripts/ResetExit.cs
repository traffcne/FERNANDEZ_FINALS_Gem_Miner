using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetExit : MonoBehaviour
{
    public GameManager gameManager;
    public void startEasy()
    {
        SceneManager.LoadScene(1); 
    }

    //public void startMedium()         //Still attempting to figure out how to instantiate different difficulty settings
    //{                                 //Besides from manually commenting and uncommenting function calls.
    //    SceneManager.LoadScene(1); 
    //}

    //public void startHard()
    //{
    //    SceneManager.LoadScene(1);      
    //}

    //public void startUV()
    //{
    //    SceneManager.LoadScene(1);
    //}

    public void mainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        print("Game Reset");
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
