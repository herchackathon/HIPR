using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace MHLab.Metamask
{
    public class MetamaskManager
    {
#if UNITY_EDITOR
		private static readonly Dictionary<string, string> _results = new Dictionary<string, string>()
		{
			{ "GetTopScores", "" },
			{ "GetPuzzle", "" },
			{ "SetScore", "" },
			{ "ValidatePuzzleResult", "" }
		};
#endif

/*#if UNITY_EDITOR
		public static string GetResults(string key)
		{
			return _results[key];
		}
#else
		[DllImport("__Internal")]
	    public static extern string GetResults(string key);
#endif*/
	    /// <summary>
	    /// Retrieves the injected Metamask account.
	    /// </summary>
	    /// <returns>The account address.</returns>
#if UNITY_EDITOR
	    public static string GetAccount()
	    {
		    return "0xQWERTYUIOPASDFGHJKLZXCVBNM";
	    }
#else
		[DllImport("__Internal")]
        public static extern string GetAccount();
#endif

	    /// <summary>
	    /// Get top scores.
	    /// </summary>
	    /// <param name="count">The amount of top scores to retrieve.</param>
#if UNITY_EDITOR
	    public static void GetTopScores(int count)
	    {
			JavascriptInteractor.ProcessResultGlobal("GetTopScores#0x1111111111111111111111111111111111111111|15;0x2222222222222222222222222222222222222222|12;0x3333333333333333333333333333333333333333|11;0x4444444444444444444444444444444444444444|9;0x5555555555555555555555555555555555555555|3");
		}
#else
		[DllImport("__Internal")]
        public static extern void GetTopScores(int count);
#endif

	    /// <summary>
	    /// Set the score for this player.
	    /// </summary>
	    /// <param name="score">The score to set.</param>
#if UNITY_EDITOR
	    public static void SetScore(int score)
	    {
		    JavascriptInteractor.ProcessResultGlobal("SetScore#78231");
	    }
#else
		[DllImport("__Internal")]
        public static extern void SetScore(int score);
#endif

	    /// <summary>
	    /// Retrieves a puzzle hash from the blockchain.
	    /// </summary>
#if UNITY_EDITOR
	    public static void GetPuzzle()
	    {
		    JavascriptInteractor.ProcessResultGlobal("GetPuzzle#sandshdiuashfuiashfuidahfuihfuia");
	    }
#else
		[DllImport("__Internal")]
        public static extern void GetPuzzle();
#endif

	    /// <summary>
	    /// Pushes the puzzle result to the smart contract for validation.
	    /// </summary>
	    /// <param name="resultHash">The resulting hash from the puzzle solving.</param>
#if UNITY_EDITOR
	    public static void ValidatePuzzleResult(string resultHash)
	    {
		    JavascriptInteractor.ProcessResultGlobal("ValidatePuzzleResult#true");
	    }
#else
		[DllImport("__Internal")]
        public static extern void ValidatePuzzleResult(string resultHash);
#endif
    }
}