using UnityEngine;
using UnityEngine.SceneManagement;

namespace MHLab.UI
{
    public class RestartButton : MonoBehaviour
    {
        public int SceneToStart;

        public void Restart()
        {
            SceneManager.LoadScene(SceneToStart);
        }
    }
}