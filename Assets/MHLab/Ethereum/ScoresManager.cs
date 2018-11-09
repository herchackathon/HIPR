using MHLab.Metamask;
using MHLab.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MHLab.Ethereum
{
	public class ScoresManager
    {
        public static void GetTopScores(Action<List<TopScore>> callback)
		{
			if (!JavascriptInteractor.Actions.ContainsKey("GetTopScores"))
				JavascriptInteractor.Actions.Add("GetTopScores", (result) => {ProcessGetTopScores(result, callback);});
			else
				JavascriptInteractor.Actions["GetTopScores"] = (result) => { ProcessGetTopScores(result, callback); };
			MetamaskManager.GetTopScores(5);
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
					bool r = false;
					try
					{
						r = bool.Parse(result);
					}
					catch
					{
						if (result == "true")
							r = true;
						else
							r = false;
					}
					callback.Invoke(r);
				});
			else
			{
				JavascriptInteractor.Actions["SetScore"] = (result) =>
				{
					bool r = false;
					try
					{
						r = bool.Parse(result);
					}
					catch
					{
						if (result == "true")
							r = true;
						else
							r = false;
					}
					callback.Invoke(r);
				};
			}
			MetamaskManager.SetScore(score);
        }
    }
}
