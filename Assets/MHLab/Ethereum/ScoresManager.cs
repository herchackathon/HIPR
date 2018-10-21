using MHLab.Metamask;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MHLab.Utilities;
using Org.BouncyCastle.Crypto.Prng;

namespace MHLab.Ethereum
{
    public class ScoresManager
    {
        public static void GetTopScores(Action<List<TopScore>> callback)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    var entries = MetamaskManager.GetTopScores(5);
                    var list = new List<TopScore>();

                    foreach (var entry in entries)
                    {
                        var tmp = entry.Split('|');
                        var address = tmp[0];
                        var score = int.Parse(tmp[1]);

                        list.Add(new TopScore()
                        {
                            PlayerAddress = address,
                            Score = score
                        });
                    }

                    MainThreadDispatcher.EnqueueAction(() =>
                    {
                        callback.Invoke(list);
                    });
                }
                catch
                {
                    var list = new List<TopScore>();
                    for (int i = 0; i < 5; i++)
                    {
                        list.Add(new TopScore()
                        {
                            PlayerAddress = "---",
                            Score = 0
                        });
                    }

                    MainThreadDispatcher.EnqueueAction(() =>
                    {
                        callback.Invoke(list);
                    });
                }
            });
        }

        public static void PushScore(int score, Action<int, bool> callback)
        {
            Task.Factory.StartNew(() => 
            {
                var response = MetamaskManager.SetScore(score);

                MainThreadDispatcher.EnqueueAction(() =>
                {
                    callback.Invoke(score, response);
                });
            });
        }
    }
}
