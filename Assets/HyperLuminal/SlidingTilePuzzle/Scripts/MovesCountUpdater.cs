﻿using UnityEngine;
using UnityEngine.UI;

namespace MHLab.SlidingTilePuzzle
{
    public class MovesCountUpdater : MonoBehaviour
    {
        private Text m_text;

        private static int Counter = 0;

        // Use this for initialization
        protected void Start()
        {
            Counter = 0;
            m_text = GetComponent<Text>();
        }

        // Update is called once per frame
        protected void Update()
        {
            m_text.text = Counter.ToString();
        }

        public static void AddMoves(int count = 1)
        {
            Counter += count;
        }
    }
}