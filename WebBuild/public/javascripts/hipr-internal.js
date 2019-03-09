// API 1.2

// hipr util [

function hiprUrl(urlType, options) {
	if (urlType == 'status') {
		var url = `${Web3Options[Web3Options.env].hipr_restful}/status`
		return url
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
	else if (urlType == 'puzzleCreateConfig') { // new api
		var modes = {
			'hipr-restful': '(default, secure)',
			'web3-browser': '(unsecure, not implemented)',
		}

		var mode = 'hipr-restful' // always hipr-restful mode in this update
		var puzzleType = '15'
		var plainTextMetrics = ''
		var address = options.address
		var password = options.password

		url = `${Web3Options[Web3Options.env].hipr_restful}/puzzleCreateConfig/${address}/${puzzleType}/undefined/${encodeURIComponent(JSON.stringify({password}))}`
		return url
	}
	else if (urlType == 'validatePuzzle') {
		var puzzleId = options.puzzleId
		var address = options.address
		var score = options.score
		var resultHash = options.resultHash
		var movesSet = options.movesSet
		
		url = `${Web3Options[Web3Options.env].hipr_restful}/validatePuzzleSecureSign/${puzzleId}/${address}/${score}/${resultHash}/${encodeURIComponent(movesSet)}`
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
		var id = (new Date()).getTime() //this.requestId++
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
		if (req.done)
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

	GetTopScoresForMenu: function(count, callback)
    {
		this.defaultWeb3();

		var self = this,
			requestId = this.getRequestId('GetTopScores')

		this.playerScore.GetTopScoresSecureCount(function (error, result) {
			if (!error) {
				count = result.c[0]
				var values = new Array(count),
					resultsCount = 0

				for (var i = 0; i < count; i++) {
					function f(i) {
/*						self.playerScore.getTopScore(i, function (error, result) {
							var value = 0
							if (!error)
								value = [result[0], result[1].c[0]];
							values[i] = {error, value}
							if (++resultsCount == count) {
								callback(values);
							}
						})*/
						self.playerScore.getTopPlayerAddress(i, function (error, result) {
							var address = 0
							if (!error)
								address = result
							self.playerScore.getTopPlayerScore(i, function (error, result) {
								var score = 0
								if (!error)
									score = result
								values[i] = {error, value: [address, score]}
								if (++resultsCount == count) {
									callback(values);
								}
							})
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
    /// Retrieves a puzzle hash from the blockchain.
    /// </summary>
    /// <param name="address">0xaddress</param>
    /// <param name="puzzleType">Puzzle type ("rubic-cube", "15")</param>
    /// <param name="mode">Mode creation hipr-restful (default) or web3-browser (not implemented)</param>
    /// <returns>Request id -> true or false</returns>
	GetPuzzle: function(address, puzzleType, mode)
	{
//		this.GetPuzzleSecureRest(address, puzzleType, mode)
		this.GetPuzzleB1(address, puzzleType, mode)
	},

	GetPuzzleB1 (address, puzzleType, mode)
	{
		var self = this,
			requestId = this.getRequestId('CreatePuzzle1')

		var url = hiprUrl('puzzleCreateConfig', {address})

//		axios({method: 'post', url, timeout: 60 * 15 * 1000})
		axios.post(url)
		.then(function (response) {
			if (!response.data || !response.data.metricsHash) {
				self.setRequestError(requestId, 'bad response')
				return
			}
//			self.CreatePuzzle(response.data.metricsHash)

			self.puzzleData = response.data

			self.puzzleManager.CreatePuzzle(response.data.metricsHash, `${requestId}`, (error, result) => {
				if (!error) {
					console.log(`Contract created for reqId=${requestId} tx=${result}, waitig for PuzzleCreated event...`)
				}
				else {
					self.setRequestError(requestId, error)
				}
			})

		})
		.catch(function (error) {
			self.setRequestError(requestId, error)
		})
/*

		this.puzzleManager.CreatePuzzle(metrics, `${requestId}`, (error, result) => {
			if (!error) {
				console.log(`Contract created for reqId=${requestId} tx=${result}, waitig for PuzzleCreated event...`)
			}
			else {
				this.setRequestError(requestId, error)
			}
		})

		return requestId*/
	},

    ValidatePuzzle: function(puzzleId, address, score, resultHash, movesSet)
    {
    	this.defaultWeb3();

		var self = this,
			requestId = this.getRequestId('ValidatePuzzle')

		// 1. ValidateMetrics(puzzleId, resultHash)
		// 		-> true

		this.puzzleManager.ValidateMetrics(puzzleId, resultHash, (error, result) => {
			var self = this

			if (!error) {

				// 2. validatePuzzle(puzzleId, address, score, resultHash, movesSet)
				// 		-> signature

				var url = hiprUrl('validatePuzzle', {puzzleId: self.puzzleData.id, address, score, resultHash, movesSet})

				axios.post(url)
				.then(function (response) {

					try {
						// 3. SetScoreSecureSign(puzzleId, resultHash, v, r, s)
						// 		-> true & SetScore

						var sig = response.data

						if (sig.err) {
							self.setRequestError(requestId, sig.err)
							return
						}

						var r = sig.r //`0x${sig.slice(0, 64)}`
						var s = sig.s //`0x${sig.slice(64, 128)}`
						var v = sig.v //web3.toDecimal(sig.slice(128, 130)) + 27

						self.playerScore.SetScoreSecureSign(address, score, resultHash, v, r, s, (error, result) => {
							if (!error) {
								self.setRequestValue(requestId, {result: true})
							}
							else {
								self.setRequestError(requestId, {result: false, error})
							}
						})
					}
					catch (e) {
						self.setRequestError(requestId, e)
					}
				})
				.catch(function (error) {
					self.setRequestError(requestId, error)
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

		this.playerScore.releaseDate((error, result) => {
			if (!error) {
				self.setRequestValue(requestId, {time: result.c[0]})
			}
			else {
				self.setRequestError(requestId, error)
			}
		})

		return requestId
	},

	PayoutInfoForMenu: function(callback) {
		this.defaultWeb3();

		this.playerScore.releaseDate((error, result) => {
			if (!error) {
				callback({time: result.c[0], err: null});
			}
			else {
				callback({time: 0, err: error});
			}
		})
	},

	wipeScores: function(callback) {
        try {
			let from = window.web3.eth.accounts[0]
			var m = this.playerScore
			let res = m.WipeScores((error, result) => {
				if (!error) {
					callback({result})
				}
				else {
					callback({err:error})
				}
			})
		}
		catch (e) {
			console.error('WipeScores error', e.message)
			return {
				err: e.message
				}
		}
	}
}

