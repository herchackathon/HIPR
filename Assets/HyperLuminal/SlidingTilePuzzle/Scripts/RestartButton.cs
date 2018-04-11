using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    public int SceneToStart;

    public void Restart()
    {
        SceneManager.LoadScene(SceneToStart);
        ST_PuzzleDisplay.PuzzleMoves = 0;
        ST_PuzzleDisplay.CanMove = false;
        ST_PuzzleDisplay.CanCount = false;
    }
}
