using MHLab.Metamask;
using MHLab.Utilities;
using System;
using UnityEngine.SceneManagement;

namespace MHLab.Ethereum
{
	public class PuzzleManager
    {
        public static string CurrentHash;

        public static void GetPuzzleHash(Action<string> callback, Action<Exception> errorCallback)
		{
			if (!JavascriptInteractor.Actions.ContainsKey("GetPuzzle"))
				JavascriptInteractor.Actions.Add("GetPuzzle", callback);
			MetamaskManager.GetPuzzle();
			//MainThreadDispatcher.EnqueueActionForNextFrame(() => { GetPuzzleHashInternal(callback, errorCallback);});
        }

	    /*private static void GetPuzzleHashInternal(Action<string> callback, Action<Exception> errorCallback)
	    {
		    var result = MetamaskManager.GetResults("GetPuzzle");

		    if (result.Trim() == string.Empty)
		    {
				MainThreadDispatcher.EnqueueActionForNextFrame(() => GetPuzzleHashInternal(callback, errorCallback));
		    }
		    else
		    {
			    CurrentHash = result;
				MainThreadDispatcher.EnqueueAction(() => { callback.Invoke(result); });
			}
	    }*/

        public static void ValidatePuzzleResult(string hash, Action<bool> callback)
		{
			if (!JavascriptInteractor.Actions.ContainsKey("ValidatePuzzleResult"))
				JavascriptInteractor.Actions.Add("ValidatePuzzleResult", (result) =>
				{
					callback.Invoke(bool.Parse(result));
				});
			MetamaskManager.ValidatePuzzleResult(hash);
	        //MainThreadDispatcher.EnqueueActionForNextFrame(() => { ValidatePuzzleResultInternal(hash, callback); });
        }

	    /*public static void ValidatePuzzleResultInternal(string hash, Action<bool> callback)
	    {
		    var result = MetamaskManager.GetResults("ValidatePuzzleResult");

		    if (result.Trim() == string.Empty)
		    {
			    MainThreadDispatcher.EnqueueActionForNextFrame(() => ValidatePuzzleResultInternal(hash, callback));
		    }
		    else
		    {
			    CurrentHash = null;
			    MainThreadDispatcher.EnqueueAction(() => { callback.Invoke(bool.Parse(result)); });
		    }
		}*/
	}
}
