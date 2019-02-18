using MHLab.Metamask;
using MHLab.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MHLab.Ethereum
{
    [Serializable]
    public struct SetScoreData
    {
        public bool result;
    }

    public class ScoresManager
    {
        public static void GetTopScores(Action<List<TopScore>> callback)
		{
			if (!JavascriptInteractor.Actions.ContainsKey("GetTopScores"))
				JavascriptInteractor.Actions.Add("GetTopScores", (result) => {ProcessGetTopScores(result, callback);});
			else
				JavascriptInteractor.Actions["GetTopScores"] = (result) => { ProcessGetTopScores(result, callback); };
			MetamaskManager.GetTopScores(50);
        }

		private static void ProcessGetTopScores(string result, Action<List<TopScore>> callback)
	    {
			try
			{
			    var entries = result.Split(new string[] {"], "}, StringSplitOptions.RemoveEmptyEntries);

			    var list = new List<TopScore>();

			    foreach (var entry in entries)
			    {
				    var entrySanitized = entry.Replace("[", "").Replace("]", "");
				    var tmp = entrySanitized.Split(',');
				    var address = tmp[0].Trim();
				    var score = int.Parse(tmp[1].Trim());

				    list.Add(new TopScore()
				    {
					    PlayerAddress = address,
					    Score = score
				    });
			    }

			    callback.Invoke(list.OrderByDescending(x => x.Score).ToList());
		    }
		    catch
		    {
			    var list = new List<TopScore>();
			    for (int i = 0; i< 5; i++)
			    {
				    list.Add(new TopScore()
				    {
					    PlayerAddress = "---",
					    Score = 0
				    });
			    }

			    callback.Invoke(list);
		    }
	    }

		public static void PushScore(int score, Action<bool> callback)
		{
			if (!JavascriptInteractor.Actions.ContainsKey("SetScore"))
				JavascriptInteractor.Actions.Add("SetScore", (result) =>
				{
				    var r = JsonUtility.FromJson<SetScoreData>(result);
				    callback.Invoke(r.result);
                });
			else
			{
				JavascriptInteractor.Actions["SetScore"] = (result) =>
				{
				    var r = JsonUtility.FromJson<SetScoreData>(result);
				    callback.Invoke(r.result);
                };
			}
			MetamaskManager.SetScore(score);
        }
    }
}
