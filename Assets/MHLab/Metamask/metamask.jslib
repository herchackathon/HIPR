var LibraryHIPR = {
	$HIPR: {},

	/*options: {
		contracts: {
			'PlayerScore': {
				abi: [{"constant":false,"inputs":[{"name":"score","type":"int256"}],"name":"SetScore","outputs":[],"payable":false,"stateMutability":"nonpayable","type":"function"},{"constant":true,"inputs":[{"name":"","type":"uint256"}],"name":"TopScores","outputs":[{"name":"player","type":"address"},{"name":"score","type":"int256"}],"payable":false,"stateMutability":"view","type":"function"},{"constant":true,"inputs":[{"name":"","type":"address"}],"name":"Scores","outputs":[{"name":"","type":"int256"}],"payable":false,"stateMutability":"view","type":"function"},{"constant":true,"inputs":[],"name":"GetTopScoresCount","outputs":[{"name":"","type":"uint256"}],"payable":false,"stateMutability":"view","type":"function"}],
				net: 'ropsten',
				address: '0xA07B1FE246D9020f6884eA9d432B551Ea534b13f',
			},
			'PuzzleManager': {
				abi: [{"constant":false,"inputs":[],"name":"acceptPuzzle","outputs":[],"payable":false,"stateMutability":"nonpayable","type":"function"},{"constant":true,"inputs":[{"name":"puzzleId","type":"uint256"}],"name":"GetPuzzleMetrics","outputs":[{"name":"","type":"bytes"}],"payable":false,"stateMutability":"view","type":"function"},{"constant":true,"inputs":[{"name":"puzzleId","type":"uint256"}],"name":"CompareMetrics","outputs":[{"name":"","type":"bool"}],"payable":false,"stateMutability":"view","type":"function"},{"constant":true,"inputs":[],"name":"owner","outputs":[{"name":"","type":"address"}],"payable":false,"stateMutability":"view","type":"function"},{"constant":true,"inputs":[{"name":"puzzleId","type":"uint256"}],"name":"GetPuzzleOriginalMetrics","outputs":[{"name":"","type":"string"}],"payable":false,"stateMutability":"view","type":"function"},{"constant":false,"inputs":[{"name":"metrics","type":"string"},{"name":"uniqueId","type":"string"}],"name":"CreatePuzzle","outputs":[{"name":"","type":"uint256"}],"payable":false,"stateMutability":"nonpayable","type":"function"},{"constant":false,"inputs":[{"name":"puzzleId","type":"uint256"},{"name":"metrics","type":"string"}],"name":"PushMetrics","outputs":[{"name":"","type":"bool"}],"payable":false,"stateMutability":"nonpayable","type":"function"},{"constant":false,"inputs":[{"name":"newOwner","type":"address"}],"name":"transferOwnership","outputs":[],"payable":false,"stateMutability":"nonpayable","type":"function"},{"inputs":[],"payable":false,"stateMutability":"nonpayable","type":"constructor"},{"anonymous":false,"inputs":[{"indexed":false,"name":"puzzleId","type":"uint256"},{"indexed":false,"name":"uniqueId","type":"string"}],"name":"PuzzleCreated","type":"event"},{"anonymous":false,"inputs":[{"indexed":true,"name":"previousOwner","type":"address"},{"indexed":true,"name":"newOwner","type":"address"}],"name":"OwnershipTransferred","type":"event"}],
				net: 'ropsten',
				address: '0xA07B1FE246D9020f6884eA9d432B551Ea534b13f',
			}
		}
	},*/

	isInit: false,

	defaultWeb3: function() {
		if(!this.isInit)
		{
			this.initWeb3(window.web3);
		}
		return window.web3//this.getWeb3('metamask')
	},

	initWeb3: function(web3) {
		/*this.playerScore = this.contractInit(web3, this.options.contracts['PlayerScore'])
		this.puzzleManager = this.contractInit(web3, this.options.contracts['PuzzleManager'])*/
		this.isInit = true;
	},

	/*contractInit: function(web3, options) {
        let contract = web3.eth.contract(this.options.abi).at(this.options.address)
        
        if (contract)
            console.log('new contract success at address:', this.options.address)

        return contract
    },*/

	results: {
		"GetTopScores": "",
		"GetPuzzle": "",
		"ValidatePuzzleResult": ""
	},

	errors: {
		"GetTopScores": "",
		"GetPuzzle": "",
		"ValidatePuzzleResult": ""
	},

	setRequestValue: function(requestId, result)
	{
		this.results[requestId] = result;
	},

	setRequestError: function(requestId, error)
	{
		this.errors[requestId] = error;
	},

	clearRequestResults: function(requestId)
	{
    	this.setRequestValue(requestId, "")
		this.setRequestError(requestId, "")
	},

	/// <summary>
	/// Get request for id
    /// <param name="requestId">Request id returned by previous function call</param>
    /// <returns>A string array with value or null if error</returns>
	/// <summary>
	GetResults: function(requestId) 
	{
		var s = this.results[requestId];
		var buffer = _malloc(lengthBytesUTF8(s) + 1);
		stringToUTF8(s, buffer, s.length + 1);
		return buffer;
	},

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
    	this.defaultWeb3();
    	this.clearRequestResults("GetTopScores")
    	var self = this

    	self.setRequestValue("GetTopScores", "0x111111111|15;0x22222222|12;0x33333333|11;0x444444444|9;0x555555555|3;")

		/*this.playerScore.GetTopScoresCount(function (error, result) {
			if (!error) {
				var values = new Array(count),
					resultsCount = 0

				for (var i = 0; i < count; i++) {
					self.playerScore.GetScore(i, function (error, result) {
						var value = 0
						if (!error)
							value =result.c[0]
						values[i] = {error, value}
						if (++resultsCount == count) {
							var s = ''
							for (var j = 0; j < count; j++) {
								if (values[j].error) {
									self.setRequestError(requestId, error)
									return
								}
								if (j != 0)
									s += ', '
								s += `${values[j].value}`
							}
							self.setRequestValue(requestId, s)
						}
					})
				}
			}
			else {
				this.setRequestError(requestId, error)
			}
		})*/
    },


    /// <summary>
    /// Set the score for this player.
    /// </summary>
    /// <param name="score">The score to set.</param>
    SetScore: function(score)
    {
    	this.defaultWeb3();
    	this.clearRequestResults("SetScore")

		var self = this
		self.setRequestValue("SetScore", score)
		/*this.playerScore.SetScore(score, (error, result) => {
			if (!error) {
				self.setRequestValue("SetScore", result)
			}
			else {
				this.setRequestError("SetScore", error)
			}
		})*/
    },


    /// <summary>
    /// Retrieves a puzzle hash from the blockchain.
    /// </summary>
    /// <returns>The metrics hash to encode.</returns>
    GetPuzzle: function()
    {	
    	this.defaultWeb3();
    	this.clearRequestResults("GetPuzzle")

		var self = this
    	this.setRequestValue("GetPuzzle", "ajdhajsndjsadjsah");
    },


    /// <summary>
	/// Pushes the puzzle result to the smart contract for validation.
	/// </summary>
	/// <param name="resultHash">The resulting hash from the puzzle solving.</param>
	/// <returns>True if correctly validated, false if not.</returns>
	ValidatePuzzleResult: function(resultHash)
	{
		this.defaultWeb3();
    	this.clearRequestResults("ValidatePuzzleResult")

		var self = this
    	this.setRequestValue("ValidatePuzzleResult", "true");
	},
};

autoAddDeps(LibraryHIPR, '$HIPR');
mergeInto(LibraryManager.library, LibraryHIPR);