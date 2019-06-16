using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
  
    public void PlayGame(string scence)
    {
        SceneManager.LoadScene(scence);
    }


    public void QuitGame()
    {
        Debug.Log("Quit Game!");
        Application.Quit();
    }


}
