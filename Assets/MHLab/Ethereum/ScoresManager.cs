using MHLab.Metamask;
using MHLab.Utilities;
using System;
using System.Collections.Generic;

namespace MHLab.Ethereum
{
	public class ScoresManager
    {
        public static void GetTopScores(Action<List<TopScore>> callback)
		{
			if (!JavascriptInteractor.Actions.ContainsKey("GetTopScores"))
				JavascriptInteractor.Actions.Add("GetTopScores", (result) =>
				{
					try
					{
						var entries = result.Split(';');

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

						callback.Invoke(list);
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

						callback.Invoke(list);
					}
				});
			MetamaskManager.GetTopScores(5);
			//MainThreadDispatcher.EnqueueActionForNextFrame(() => GetTopScoresInternal(callback));
        }

		/*private static void GetTopScoresInternal(Action<List<TopScore>> callback)
		{
			var result = MetamaskManager.GetResults("GetTopScores");

			if (result.Trim() == string.Empty)
			{
				MainThreadDispatcher.EnqueueActionForNextFrame(() => GetTopScoresInternal(callback));
			}
			else
			{
				try
				{
					var entries = result.Split(';');

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

					MainThreadDispatcher.EnqueueAction(() => callback.Invoke(list));
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

					MainThreadDispatcher.EnqueueAction(() => callback.Invoke(list));
				}
			}
		}*/

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
			MetamaskManager.SetScore(score);

            /*MainThreadDispatcher.EnqueueActionForNextFrame(() =>
            {
	            PushScoreInternal(score, callback);
            });*/
        }

	    /*private static void PushScoreInternal(int score, Action<int, bool> callback)
	    {
		    var result = MetamaskManager.GetResults("SetScore");

		    if (result.Trim() == string.Empty)
		    {
			    MainThreadDispatcher.EnqueueActionForNextFrame(() => PushScoreInternal(score, callback));
		    }
		    else
		    {
			    MainThreadDispatcher.EnqueueAction(() => callback.Invoke(score, true));
		    }
	    }*/
    }
}
