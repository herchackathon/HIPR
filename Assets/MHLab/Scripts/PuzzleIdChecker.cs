using MHLab.SlidingTilePuzzle.Data;
using MHLab.Web.Storage;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MHLab.Ethereum
{
    public class PuzzleIdChecker : MonoBehaviour
    {
        public const string IdPrefix = "HERCID-";
        // The scene index to load after a successful login.
        public int SceneToLoad = 2;

        // The input field that contains the address.
        public InputField PuzzleIdText;

        public Text VerifyingText;
        public Text ErrorText;

        public void OnStartPressed()
        {
            var puzzleId = uint.Parse(PuzzleIdText.text.Trim().Replace(IdPrefix, ""));
            /*StartCoroutine(AccountManager.GetPuzzleData(puzzleId, (originalMetrics, currentMetrics) =>
            {
                ST_PuzzleDisplay.OriginalHash = originalMetrics;
                ST_PuzzleDisplay.CurrentHash = currentMetrics;
                SceneManager.LoadScene(SceneToLoad);
            }));*/
        }
    }
}