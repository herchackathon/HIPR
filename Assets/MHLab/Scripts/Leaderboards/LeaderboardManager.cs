using MHLab.Ethereum;
using System.Collections.Generic;
using UnityEngine;

namespace MHLab.SlidingTilePuzzle.Leaderboards
{
    public class LeaderboardManager : MonoBehaviour
    {
        public static LeaderboardManager Instance;
        private LeaderboardEntry[] _entries = new LeaderboardEntry[5];

        protected void Awake()
        {
            Instance = this;

            _entries = GetComponentsInChildren<LeaderboardEntry>();
            
            ScoresManager.GetTopScores((scores) =>
            {
                int index = 0;
                foreach (var topScore in scores)
                {
                    LeaderboardManager.Instance.SetEntry(index, topScore.PlayerAddress, topScore.Score);
                    index++;
                }
            });
        }

        public void SetEntry(int index, string address, int score)
        {
            var entry = _entries[index];
            entry.Position.text = (index + 1).ToString();
            entry.Address.text = address;
            entry.Score.text = score.ToString();
        }
    }
}