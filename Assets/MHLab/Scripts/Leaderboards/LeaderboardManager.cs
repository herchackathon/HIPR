using System;
using System.Collections.Generic;
using MHLab.Nethereum;
using UnityEngine;

namespace MHLab.SlidingTilePuzzle.Leaderboards
{
    public class LeaderboardManager : MonoBehaviour
    {
        public static LeaderboardManager Instance;
        private readonly List<LeaderboardEntry> _entries = new List<LeaderboardEntry>();

        protected void Awake()
        {
            Instance = this;

            var entries = GetComponentsInChildren<LeaderboardEntry>();

            for (int i = 0; i < entries.Length; i++)
            {
                var entry = entries[i];
                entry.Position.text = (i + 1).ToString();
                entry.Address.text = "---";
                entry.Score.text = "---";
                _entries.Add(entry);
            }
        }

        public void SetEntry(int index, string address, int score)
        {
            var entry = _entries[index];
            entry.Address.text = address;
            entry.Score.text = score.ToString();
        }
    }
}