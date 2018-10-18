using MHLab.Metamask;
using System;
using System.Collections.Generic;

namespace MHLab.Ethereum
{
    public class ScoresManager
    {
        public static void GetTopScores(Action<List<TopScore>> callback)
        {
            MetamaskManager.GetTopScores(5);
        }

        public static void PushScore(int score, Action<int> callback)
        {
            var response = MetamaskManager.SetScore(score);
        }
    }
}
