var LibraryHIPR = {

	/// <summary>
    /// Retrieves the injected Metamask account.
    /// </summary>
    /// <returns>The account address.</returns>
	GetAccount: function() 
	{
		var account = window.web3.eth.accounts[0];

		var buffer = _malloc(lengthBytesUTF8(account) + 1);
		stringToUTF8(account, buffer, account.length + 1);
		return buffer;
	},


	/// <summary>
    /// Get top scores.
    /// </summary>
    /// <param name="count">The amount of top scores to retrieve.</param>
    /// <returns>A string array where every entry has this format: "accountAddress|score"</returns>
    GetTopScores: function(count)
    {
    	GetTopScoresInternal(count);
    },


    /// <summary>
    /// Set the score for this player.
    /// </summary>
    /// <param name="score">The score to set.</param>
    SetScore: function(score)
    {
    	SetScoreInternal(score);
    },


    /// <summary>
    /// Retrieves a puzzle hash from the blockchain.
    /// </summary>
    /// <returns>The metrics hash to encode.</returns>
    GetPuzzle: function()
    {	
    	GetPuzzleInternal();
    },


    /// <summary>
	/// Pushes the puzzle result to the smart contract for validation.
	/// </summary>
	/// <param name="resultHash">The resulting hash from the puzzle solving.</param>
	/// <returns>True if correctly validated, false if not.</returns>
	ValidatePuzzleResult: function(resultHash)
	{
		ValidatePuzzleResultInternal(resultHash);
	},
};

mergeInto(LibraryManager.library, LibraryHIPR);