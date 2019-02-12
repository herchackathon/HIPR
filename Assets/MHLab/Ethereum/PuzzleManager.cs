using MHLab.Metamask;
using MHLab.Utilities;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MHLab.Ethereum
{
    public struct GetPuzzleData
    {
        public int puzzleId;
        public List<int> field;
    }

	public static class PuzzleManager
    {
        public static string CurrentHash = "TestIt";
        public static GetPuzzleData PuzzleData;

        public static void GetPuzzleHash(Action<string> callback, Action<Exception> errorCallback)
        {
	        if (!JavascriptInteractor.Actions.ContainsKey("GetPuzzle"))
		        JavascriptInteractor.Actions.Add("GetPuzzle", callback);
	        else
		        JavascriptInteractor.Actions["GetPuzzle"] = callback;
			MetamaskManager.GetPuzzle();
        }

        public static void ValidatePuzzleResult(int puzzleId, int score, string hash, List<STPuzzleMove> moveset, Action<bool> callback)
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

			MetamaskManager.ValidatePuzzleResult(puzzleId, score, hash, JsonConvert.SerializeObject(moveset));
        }
	}
}
