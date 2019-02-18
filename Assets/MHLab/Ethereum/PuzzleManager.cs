using MHLab.Metamask;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MHLab.Ethereum
{
    [Serializable]
    public struct GetPuzzleData
    {
        public int puzzleId;
        public List<int> field;
        public string hash;
    }

    [Serializable]
    public struct ValidatePuzzleData
    {
        public bool result;
        public string tx;
        public string err;
    }

    [Serializable]
    public struct MovesSetData
    {
        public List<STPuzzleMove> moveset;
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
			if (!JavascriptInteractor.Actions.ContainsKey("ValidatePuzzle"))
				JavascriptInteractor.Actions.Add("ValidatePuzzle", (result) =>
				{
				    var r = JsonUtility.FromJson<ValidatePuzzleData>(result);
					callback.Invoke(r.result);
				});
			else
				JavascriptInteractor.Actions["ValidatePuzzle"] = (result) =>
				{
				    var r = JsonUtility.FromJson<ValidatePuzzleData>(result);
				    callback.Invoke(r.result);
                };
            var movesSetWrapper = new MovesSetData();
		    movesSetWrapper.moveset = moveset;
		    var temp = JsonUtility.ToJson(movesSetWrapper);

            MetamaskManager.ValidatePuzzleResult(puzzleId, score, hash, temp);
        }
	}
}
