// API 1.1

HIPRInternal1 = {
    /// <summary>
    /// Register puzzle for testing purposes
    /// </summary>
    /// <param name="address">0xaddress</param>
    /// <param name="params">Params into hiprs like 'username/assetid/factomchainhash/ipfsHash/0xaddress'</param>
    /// <returns>Request id -> true or false</returns>
	RegisterPuzzle: function(address, params)
	{
    	this.defaultWeb3();

		var self = this,
			requestId = this.getRequestId('RegisterPuzzle')

		var url = hiprUrl('registerPuzzle', {address, params})

		axios.post(url)
			.then(function (response) {
				// handle success
//				console.log(requestId, response);
				self.setRequestValue(requestId, response.data)
			})
			.catch(function (error) {
				// handle error
//				console.log(error);
				this.setRequestError(requestId, error)
			})

		return requestId
	},

    /// <summary>
    /// Retrieves a puzzle hash from the blockchain.
    /// </summary>
    /// <param name="address">0xaddress</param>
    /// <param name="puzzleType">Puzzle type ("rubic-cube", "15")</param>
    /// <param name="mode">Mode creation hipr-restful (default) or web3-browser (not implemented)</param>
    /// <returns>Request id -> true or false</returns>
    GetPuzzleSecureRest: function(address, puzzleType, mode)
    {
    	this.defaultWeb3();

		var self = this,
			requestId = this.getRequestId('GetPuzzle')

		var url = hiprUrl('createPuzzle', {address})

		axios({method: 'post', url, timeout: 60 * 15 * 1000})
//		axios.post(url)
		.then(function (response) {
				// handle success
//				console.log(requestId, response);
				self.setRequestValue(requestId, response.data)

				//HIPRInternal.setRequestValue(requestId, response)
			})
			.catch(function (error) {
				// handle error
//				console.log(error);
				self.setRequestError(requestId, error)
			})

		return requestId
    },

	
    /// <summary>
	/// Pushes the puzzle result to the smart contract for validation.
	/// </summary>
    /// <param name="puzzleId">Puzzle id</param>
	/// <param name="resultHash">The resulting hash from the puzzle solving.</param>
	/// <returns>Result id -> true if correctly validated, false if not.</returns>
	ValidatePuzzleResultSecure: function(puzzleId, resultHash)
	{
		this.defaultWeb3();

		var self = this,
			requestId = this.getRequestId('ValidatePuzzleResult')

		this.puzzleManager.PushSecureMetrics(puzzleId, resultHash, (error, result) => {
			if (!error) {
				this.puzzleManager.compareSecureMetrics(puzzleId, (error, result) => {
					if (!error) {
						self.setRequestValue(requestId, result)

						HIPRInternal.setRequestValue(requestId, result)
					}
					else {
						self.setRequestError(requestId, error)
					}
				})
			}
			else {
				this.setRequestError(requestId, error)
			}
		})

		return requestId
	},

}
