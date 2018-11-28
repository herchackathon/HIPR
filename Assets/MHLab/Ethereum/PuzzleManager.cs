using MHLab.Metamask;
using MHLab.Utilities;
using System;
using UnityEngine.SceneManagement;

namespace MHLab.Ethereum
{
	public class PuzzleManager
    {
        public static string CurrentHash = "TestIt";

        public static void GetPuzzleHash(Action<string> callback, Action<Exception> errorCallback)
        {
	        if (!JavascriptInteractor.Actions.ContainsKey("GetPuzzle"))
		        JavascriptInteractor.Actions.Add("GetPuzzle", callback);
	        else
		        JavascriptInteractor.Actions["GetPuzzle"] = callback;
			MetamaskManager.GetPuzzle();
        }

        public static void ValidatePuzzleResult(string hash, Action<bool> callback)
		{
			if (!JavascriptInteractor.Actions.ContainsKey("ValidatePuzzleResult"))
				JavascriptInteractor.Actions.Add("ValidatePuzzleResult", (result) =>
				{
					callback.Invoke(bool.Parse(result));
				});
			else
				JavascriptInteractor.Actions["ValidatePuzzleResult"] = (result) =>
				{
					callback.Invoke(bool.Parse(result));
				};
			MetamaskManager.ValidatePuzzleResult(hash);
        }
	}
}
