using System;
using System.Runtime.InteropServices;

namespace MHLab.Metamask
{
    public class MetamaskManager
    {
	    [DllImport("__Internal")]
	    public static extern string GetResults(string key);
		
        /// <summary>
        /// Retrieves the injected Metamask account.
        /// </summary>
        /// <returns>The account address.</returns>
        [DllImport("__Internal")]
        public static extern string GetAccount();

        /// <summary>
        /// Get top scores.
        /// </summary>
        /// <param name="count">The amount of top scores to retrieve.</param>
        [DllImport("__Internal")]
        public static extern void GetTopScores(int count);

        /// <summary>
        /// Set the score for this player.
        /// </summary>
        /// <param name="score">The score to set.</param>
        [DllImport("__Internal")]
        public static extern void SetScore(int score);

        /// <summary>
        /// Retrieves a puzzle hash from the blockchain.
        /// </summary>
        [DllImport("__Internal")]
        public static extern void GetPuzzle();

        /// <summary>
        /// Pushes the puzzle result to the smart contract for validation.
        /// </summary>
        /// <param name="resultHash">The resulting hash from the puzzle solving.</param>
        [DllImport("__Internal")]
        public static extern void ValidatePuzzleResult(string resultHash);
    }
}