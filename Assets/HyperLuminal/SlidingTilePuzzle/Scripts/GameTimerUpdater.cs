using UnityEngine;
using UnityEngine.UI;

namespace MHLab.SlidingTilePuzzle
{
    public class GameTimerUpdater : MonoBehaviour
    {
        private Text m_text;
        public static float ElapsedSeconds;
        private static bool IsStarted = false;

        // Use this for initialization
        protected void Start()
        {
            ClearTimer();
            m_text = GetComponent<Text>();
        }

        // Update is called once per frame
        protected void Update()
        {
            if (IsStarted)
            {
                ElapsedSeconds += Time.deltaTime;
                m_text.text = ((int) ElapsedSeconds).ToString();
            }
        }

        public static void StartTimer()
        {
            IsStarted = true;
        }

        public static void StopTimer()
        {
            IsStarted = false;
        }

        public static void ClearTimer()
        {
            ElapsedSeconds = 0f;
        }
    }
}
