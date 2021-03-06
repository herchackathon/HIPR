﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace MHLab.Metamask
{
    public class MetamaskManager
    {
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
        /// Retrieves the injected Metamask account.
        /// </summary>
        /// <returns>The account address.</returns>
#if UNITY_EDITOR
        public static void GetEndOfSeason()
        {
            // PARAMS THAT WEB3 JAVASCRIPT HAS TO PASS BACK TO HIPR
            // Key      = GetEndOfSeason: string
            // Value    = {"time":1547510399000}: Unix Timestamp (long)
            JavascriptInteractor.ProcessResultGlobal("GetEndOfSeason#{\"time\":1547510399000}");
        }
#else
		[DllImport("__Internal")]
        public static extern long GetEndOfSeason();
#endif

        /// <summary>
        /// Get top scores.
        /// </summary>
        /// <param name="count">The amount of top scores to retrieve.</param>
#if UNITY_EDITOR
        public static void GetTopScores(int count)
        {
            // PARAMS THAT WEB3 JAVASCRIPT HAS TO PASS BACK TO HIPR
            // Key      = GetTopScores: string
            // Value    =   [0xf55f45267258efbfcefb795a688630a26576635e, 9727], 
            //              [0xf55f45267258efbfcefb795a688630a26576635e, 4574], 
            //              [0xf55f45267258efbfcefb795a688630a26576635e, 15948], 
            //              [0xf55f45267258efbfcefb795a688630a26576635e, 9425], 
            //              [0x8f96f32db25b2d4fa0787d6e045630caaf2d09f7, 789]
            JavascriptInteractor.ProcessResultGlobal("GetTopScores#[0xf55f45267258efbfcefb795a688630a26576635e, 9727], [0xf55f45267258efbfcefb795a688630a26576635e, 4574], [0xf55f45267258efbfcefb795a688630a26576635e, 15948], [0xf55f45267258efbfcefb795a688630a26576635e, 9425], [0x8f96f32db25b2d4fa0787d6e045630caaf2d09f7, 789]");
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
            // PARAMS THAT WEB3 JAVASCRIPT HAS TO PASS BACK TO HIPR
            // Key      = SetScore: string
            // Value    = {"result": true}
            JavascriptInteractor.ProcessResultGlobal("SetScore#{\"result\": true}");
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
            // PARAMS THAT WEB3 JAVASCRIPT HAS TO PASS BACK TO HIPR
            // Key      = GetPuzzle: string
            // Value    = {
            //              puzzleId: 1,                    => the puzzleId
            //              field: [0,7,3,4,1,2,6,8,5],     => the initial state of the puzzle
            //              hash: "HashMetricsHere"         => the hash to encode in the puzzle
	        //            }: stringified JSON object
            JavascriptInteractor.ProcessResultGlobal("GetPuzzle#{\"plainTextMetrics\":\"\",\"metricsHash\":\"0xcc0babd1bdc475da63b327ea6e0e2fc5aaa844a2aa6190c8c1263cf6e8e8baa6\",\"params\":{},\"puzzleField\":[1,0,2,4,3,5,6,8,7],\"checkOwner\":true,\"puzzleId\":6}");
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
	    public static void ValidatePuzzleResult(int puzzleId, int score, string resultHash, string movesSet)
	    {
            // PARAMS HIPR GAME PASSES TO JAVASCRIPT
            // puzzleId: int        => the ID of the puzzle
            // score: int           => the score 
            // resultHash: string   => the decoded
            // movesSet: string     => the set of moves performed by the player to solve the puzzle => [[1, 0], [2, 0], [2, 1], [2, 2]]

            // PARAMS THAT WEB3 JAVASCRIPT HAS TO PASS BACK TO HIPR
            // Key      = ValidatePuzzleResult: string
            // Value    = {"result": true}
            JavascriptInteractor.ProcessResultGlobal("ValidatePuzzle#{\"result\": true}");
	    }
#else
		[DllImport("__Internal")]
        public static extern void ValidatePuzzleResult(int puzzleId, int score, string resultHash, string movesSet);
#endif

        /// <summary>
        /// Shows a message as debug.
        /// </summary>
#if UNITY_EDITOR
        public static void DebugLog(string message)
        {
            Debug.Log(message);
        }
#else
		[DllImport("__Internal")]
        public static extern void DebugLog(string message);
#endif
    }
}