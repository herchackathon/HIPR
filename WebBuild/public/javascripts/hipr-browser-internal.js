// hipr util [

function hiprUrl(urlType, options) {
	if (urlType == status) {

	}
	else if (urlType == 'registerPuzzle') {
		var address = options.address
		var params = options.params // ipfsHash
		var url = `${Web3Options[Web3Options.env].hipr_restful}/registerPuzzleAddress/${address}/${encodeURIComponent(params)}`
		return url
	}
	else if (urlType == 'createPuzzle') {
		var modes = {
			'hipr-restful': '(default, secure)',
			'web3-browser': '(unsecure, not implemented)',
		}

		var mode = 'hipr-restful' // always hipr-restful mode in this update
		var puzzleType = '15'
		var plainTextMetrics = ''
		var address = options.address
		var password = options.password

		url = `${Web3Options[Web3Options.env].hipr_restful}/createPuzzleSecure/${address}/${puzzleType}/undefined/${encodeURIComponent(JSON.stringify({password}))}`
		return url
	}
	else if (urlType == 'validatePuzzle') {
		var puzzleId = options.puzzleId
		var address = options.address
		var score = options.score
		var resultHash = options.resultHash
		var movesSet = options.movesSet
		
		url = `${Web3Options[Web3Options.env].hipr_restful}/validatePuzzleSecure/${puzzleId}/${address}/${score}/${resultHash}/${encodeURIComponent(movesSet)}`
		return url
	}

	return null
}

// hipr util ]

HIPRInternal = {

    isInit: false,

    init () {
		this.defaultWeb3()
		
		//this.TestApi()
    },

    // restructured code class
    defaultWeb3 () {
        if (!this.isInit) {
			this.isInit = true

			// LINK to Web3Internal

            Web3Internal.defaultWeb3()
            this.playerScore = Web3Internal.playerScore
            this.puzzleManager = Web3Internal.puzzleManager
        }
    },

    requests: {
		0: {
			method: 'method name',
			value: 'string|number'
		}
	},
	requestId: 1,

	getRequestId (name) {
		var id = this.requestId++
		this.requests[id] = {
			name,
			value: null,
			error: null,
			done: false
		}
		return id
	},

	setRequestValue (id, value) {
		var req = this.requests[id]
		if (!req) {
			console.log(`request ${id} not found`)
			return
		}
		if (req.value)
			return
		console.log('result', id, req.name, value)
		req.value = value
        req.done = true
        
        // LINK to HIPR Unity
        SendResultBack(req.name, req.value)
    },

	setRequestError (id, error) {
		var req = this.requests[id]
		if (!req) {
			console.log(`request ${id} not found`)
			return
		}
		console.log('error', id, req.name, error)
		req.error = error
		req.done = true
    },
    
    TestApi0 () {
		var self = this
		var rid = self.SetScore(789)
		var puzzleId
		setInterval(()=>{

			var req = self.GetRequest(rid)
			if (req) {
				console.log(req)
				if (rid == 1) {
					rid = self.GetTopScores(10)
				}
				else if (rid == 2) {
					rid = self.CreatePuzzle('puzzle 1a')
				}
				else if (rid == 3) {
					rid = self.CreatePuzzle('puzzle 2b')
				}
				else if (rid == 4) {
					puzzleId = req
					rid = self.PushMetrics(puzzleId, 'puzzle 1')
				}
				else if (rid == 5) {
					rid = self.CompareMetrics(puzzleId)
				}
				else if (rid == 6) {
					debugger
					rid = self.ValidatePuzzleResult(puzzleId, 'puzzle 2b')
				}
			}

		}, 100)
	},

    TestApi () {
		var self = this
		var address = web3.eth.defaultAccount
		var rid = self.RegisterPuzzle(address, 'username/assetid/factomchainhash/ipfsHash/0xaddress')
		var puzzleId
		setInterval(()=>{

			var req = self.GetRequest(rid)
			if (req) {
				console.log(req)
				if (rid == 1) {
					rid = self.GetPuzzle(address, '15-test')
				}
				else if (rid == 2) {
					debugger
					puzzleId = req.puzzleId
					metrics
					rid = self.ValidatePuzzleResult(puzzleId, 'puzzle 2b')
				}
			}

		}, 100)
	},
	

    /// <summary>
	/// Get request for id
    /// <param name="requestId">Request id returned by previous function call</param>
    /// <returns>A string array with value or null if error</returns>
	/// <summary>
	GetRequest (requestId) {
		var req = this.requests[requestId]
		if (req && req.done) {
			if (req.error) {
				return null
			}

			var s = typeof req.value === 'object' ? req.value : `${req.value}`

            return s
		}
		else
			return null
	},

	/// <summary>
    /// Get top scores.
    /// </summary>
    /// <param name="count">The amount of top scores to retrieve.</param>
    /// <returns>Request id -> string with results</returns>
    GetTopScores: function(count)
    {
		this.defaultWeb3();

		var self = this,
			requestId = this.getRequestId('GetTopScores')

		this.playerScore.GetTopScoresSecureCount(function (error, result) {
			if (!error) {
				var values = new Array(count),
					resultsCount = 0

				for (var i = 0; i < count; i++) {
					function f(i) {
						self.playerScore.TopScoresSecure(i, function (error, result) {
							var value = 0
							if (!error)
								value = `[${result[0]}, ${result[1].c[0]}]`
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
					f(i)
				}
			}
			else {
				this.setRequestError(requestId, error)
			}
		})

		return requestId
	},

	GetTopScoresForMenu: function(count, callback)
    {
		this.defaultWeb3();

		var self = this,
			requestId = this.getRequestId('GetTopScores')

		this.playerScore.GetTopScoresSecureCount(function (error, result) {
			if (!error) {
				var values = new Array(count),
					resultsCount = 0

				for (var i = 0; i < count; i++) {
					function f(i) {
						self.playerScore.TopScoresSecure(i, function (error, result) {
							var value = 0
							if (!error)
								value = [result[0], result[1].c[0]];
							values[i] = {error, value}
							if (++resultsCount == count) {
								callback(values);
							}
						})
					}
					f(i);
				}
			}
			else {
				this.setRequestError(requestId, error)
			}
		})

		return requestId
	},

    /// <summary>
    /// Set the score for this player.
    /// </summary>
    /// <param name="score">The score to set.</param>
    /// <returns>Request id -> true if success</returns>
    SetScore: function(score)
    {
		this.defaultWeb3();

		var self = this,
			requestId = this.getRequestId('SetScore')

		this.playerScore.SetScore(score, (error, result) => {
			if (!error) {
				self.setRequestValue(requestId, result)
			}
			else {
				this.setRequestError(requestId, error)
			}
		})

		return requestId
    },

	/// <summary>
    /// Creates a new puzzle with given metrics.
    /// </summary>
    /// <param name="metrics">Metrics for new puzzle</param>
    /// <returns>Request id -> puzzleId</returns>
    CreatePuzzle (metrics)
    {
		this.defaultWeb3();

		var self = this,
			requestId = this.getRequestId('CreatePuzzle')

		this.puzzleManager.CreatePuzzle(metrics, `${requestId}`, (error, result) => {
			if (!error) {
				console.log(`Contract created for reqId=${requestId} tx=${result}, waitig for PuzzleCreated event...`)
			}
			else {
				this.setRequestError(requestId, error)
			}
		})

		return requestId
	},

    /// <summary>
    /// Pushes metrics for the given puzzle.
    /// </summary>
    /// <param name="puzzleId">Puzzle id</param>
    /// <param name="metrics">Metrics for push</param>
    /// <returns>Request id -> true if success</returns>
	PushMetrics (puzzleId, metrics) 
	{
		this.defaultWeb3();

		var self = this,
			requestId = this.getRequestId('PushMetrics')

		this.puzzleManager.PushMetrics(puzzleId, metrics, (error, result) => {
			if (!error) {
				self.setRequestValue(requestId, true)
			}
			else {
				this.setRequestError(requestId, error)
			}
		})

		return requestId
	},

	/// <summary>
    /// Pushes metrics for the given puzzle.
    /// </summary>
    /// <param name="puzzleId">Puzzle id</param>
    /// <returns>Request id -> true or false</returns>
	CompareMetrics (puzzleId) 
	{
		this.defaultWeb3();

		var self = this,
			requestId = this.getRequestId('CompareMetrics')

		this.puzzleManager.CompareMetrics(puzzleId, (error, result) => {
			if (!error) {
				self.setRequestValue(requestId, result)
			}
			else {
				this.setRequestError(requestId, error)
			}
		})

		return requestId
	},

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
    GetPuzzle: function(address, puzzleType, mode)
    {
    	this.defaultWeb3();

		var self = this,
			requestId = this.getRequestId('GetPuzzle')

		var url = hiprUrl('createPuzzle', {address})

		axios.post(url)
			.then(function (response) {
				// handle success
//				console.log(requestId, response);
				self.setRequestValue(requestId, response)

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
	ValidatePuzzleResult: function(puzzleId, resultHash)
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
						this.setRequestError(requestId, error)
					}
				})
			}
			else {
				this.setRequestError(requestId, error)
			}
		})

		return requestId
	},

	PayoutInfo: function() {
		this.defaultWeb3();

		var self = this,
			requestId = this.getRequestId('GetEndOfSeason')
/*
			let lastWipeDate = await m.lastWipeDate().call()
            let startDate = await m.startDate().call()
            let releaseDate = await m.releaseDate().call()
            let seasonInterval = await m.seasonInterval().call()

            let hercContract = await m.hercContract().call()
            let payoutAddress = await m.payoutBoss().call()
*/

		this.puzzleManager.releaseDate((error, result) => {
			if (!error) {
				self.setRequestValue(requestId, result)
			}
			else {
				this.setRequestError(requestId, error)
			}
		})

		return requestId
	},

    ValidatePuzzle: function(puzzleId, address, score, resultHash, movesSet)
    {
    	this.defaultWeb3();

		var self = this,
			requestId = this.getRequestId('GetPuzzle')

		var url = hiprUrl('validatePuzzle', {puzzleId, address, score, resultHash, movesSet})

		axios.post(url)
			.then(function (response) {
				// handle success
//				console.log(requestId, response);
				self.setRequestValue(requestId, response)

				HIPRInternal.setRequestValue(requestId, response)
			})
			.catch(function (error) {
				// handle error
//				console.log(error);
				this.setRequestError(requestId, error)
			})

		return requestId
    },
}

