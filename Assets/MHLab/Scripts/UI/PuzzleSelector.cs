using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PuzzleSelector : MonoBehaviour
{
    public Text LevelName;
    public Text LevelDescription;
    public Image LevelImage;

    private class PuzzleLevelData
    {
        public int SceneIndex;
        public string Name;
        public string Description;
        public Sprite Image;
    }

    private static int CurrentSelectedPuzzleIndex = 0;

    private static List<PuzzleLevelData> PuzzleLevels;

    protected void Awake()
    {
        PuzzleLevels = new List<PuzzleLevelData>()
        {
            new PuzzleLevelData()
            {
                SceneIndex = 2,
                Name = "Sliding Puzzle",
                Description = "Let's reorder puzzle pieces to restore the original image! But pay attention to moves count and to the time! Your score will be influenced by them!",
                Image = Resources.Load<Sprite>("Sprites/SlidingPuzzleMiniature")
            },
            /*new PuzzleLevelData()
            {
                SceneIndex = 1,
                Name = "Rubik's Cube",
                Description = "",
                Image = Resources.Load<Sprite>("Sprites/SlidingPuzzleMiniature")
            },*/
        };
    }

    protected void Start()
    {
        UpdateLevelData(CurrentSelectedPuzzleIndex);
    }

    private void UpdateLevelData(int index)
    {
        if (index >= PuzzleLevels.Count) return;

        var level = PuzzleLevels[index];

        if (level == null) return;

        LevelName.text = level.Name;
        LevelDescription.text = level.Description;
        LevelImage.sprite = level.Image;
    }

    public void SelectNext()
    {
        if (CurrentSelectedPuzzleIndex < PuzzleLevels.Count - 1)
        {
            CurrentSelectedPuzzleIndex++;
            UpdateLevelData(CurrentSelectedPuzzleIndex);
        }
    }

    public void SelectPrevious()
    {
        if (CurrentSelectedPuzzleIndex > 0)
        {
            CurrentSelectedPuzzleIndex--;
            UpdateLevelData(CurrentSelectedPuzzleIndex);
        }
    }

    public void SelectLevel()
    {
        SceneManager.LoadScene(PuzzleLevels[CurrentSelectedPuzzleIndex].SceneIndex);
    }
}
