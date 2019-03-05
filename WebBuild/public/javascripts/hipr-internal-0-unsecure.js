// API 1.0

HIPRInternal0 = {
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
						self.playerScore.getTopScore(i, function (error, result) {
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


}
