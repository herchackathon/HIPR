using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace MHLab.SlidingTilePuzzle
{
    public class GameTimerUpdater : MonoBehaviour
    {
        private Text m_text;

        // Use this for initialization
        protected void Start()
        {
            m_text = GetComponent<Text>();
        }

        // Update is called once per frame
        protected void Update()
        {
            m_text.text = ST_PuzzleDisplay.GameTimer.Elapsed.Seconds.ToString();
        }
    }
}
