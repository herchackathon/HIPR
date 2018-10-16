using System.Runtime.InteropServices;

namespace MHLab.Metamask
{
    public class MetamaskManager
    {
        [DllImport("__Internal")]
        public static extern void SendTransaction(string to, string data);

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
        /// <returns>A string array where every entry has this format: "accountAddress|score"</returns>
        [DllImport("__Internal")]
        public static extern string[] GetTopScores(int count);

        /// <summary>
        /// Set the score for this player.
        /// </summary>
        /// <param name="score">The score to set.</param>
        [DllImport("__Internal")]
        public static extern bool SetScore(int score);

        /// <summary>
        /// Retrieves a puzzle hash from the blockchain.
        /// </summary>
        /// <returns>The metrics hash to encode.</returns>
        [DllImport("__Internal")]
        public static extern string GetPuzzle();

        /// <summary>
        /// Pushes the puzzle result to the smart contract for validation.
        /// </summary>
        /// <param name="resultHash">The resulting hash from the puzzle solving.</param>
        /// <returns>True if correctly validated, false if not.</returns>
        [DllImport("__Internal")]
        public static extern bool ValidatePuzzleResult(string resultHash);
    }
}