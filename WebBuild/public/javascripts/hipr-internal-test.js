// API 1.0 TEST

var HIPRInternalTest = {
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
}