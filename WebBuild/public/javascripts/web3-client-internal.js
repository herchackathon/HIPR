Web3Internal = {
    // web3 [
    
        options: {
            contracts: {
                'PlayerScore': {
                    abi: [{"anonymous": false,"inputs": [{"indexed": true,"name": "previousOwner","type": "address"},{"indexed": true,"name": "newOwner","type": "address"}],"name": "OwnershipTransferred","type": "event"},{"constant": false,"inputs": [],"name": "renounceOwnership","outputs": [],"payable": false,"stateMutability": "nonpayable","type": "function"},{"constant": false,"inputs": [{"name": "score","type": "int256"}],"name": "SetScore","outputs": [],"payable": false,"stateMutability": "nonpayable","type": "function"},{"constant": false,"inputs": [{"name": "newOwner","type": "address"}],"name": "transferOwnership","outputs": [],"payable": false,"stateMutability": "nonpayable","type": "function"},{"constant": true,"inputs": [],"name": "GetTopScoresCount","outputs": [{"name": "","type": "uint256"}],"payable": false,"stateMutability": "view","type": "function"},{"constant": true,"inputs": [],"name": "isOwner","outputs": [{"name": "","type": "bool"}],"payable": false,"stateMutability": "view","type": "function"},{"constant": true,"inputs": [],"name": "owner","outputs": [{"name": "","type": "address"}],"payable": false,"stateMutability": "view","type": "function"},{"constant": true,"inputs": [{"name": "","type": "address"}],"name": "Scores","outputs": [{"name": "","type": "int256"}],"payable": false,"stateMutability": "view","type": "function"},{"constant": true,"inputs": [{"name": "","type": "uint256"}],"name": "TopScores","outputs": [{"name": "player","type": "address"},{"name": "score","type": "int256"}],"payable": false,"stateMutability": "view","type": "function"}],//[{"constant":false,"inputs":[{"name":"score","type":"int256"}],"name":"SetScore","outputs":[],"payable":false,"stateMutability":"nonpayable","type":"function"},{"constant":true,"inputs":[{"name":"","type":"uint256"}],"name":"TopScores","outputs":[{"name":"player","type":"address"},{"name":"score","type":"int256"}],"payable":false,"stateMutability":"view","type":"function"},{"constant":true,"inputs":[{"name":"","type":"address"}],"name":"Scores","outputs":[{"name":"","type":"int256"}],"payable":false,"stateMutability":"view","type":"function"},{"constant":true,"inputs":[],"name":"GetTopScoresCount","outputs":[{"name":"","type":"uint256"}],"payable":false,"stateMutability":"view","type":"function"}],
                    net: 'mainnet',
                    address: '0xeed0eb7a4251ce217b7d37d370267735626ad2c6',//'0xA07B1FE246D9020f6884eA9d432B551Ea534b13f',
                },
                'PuzzleManager': {
                    abi: [{"constant":false,"inputs":[],"name":"acceptPuzzle","outputs":[],"payable":false,"stateMutability":"nonpayable","type":"function"},{"constant":true,"inputs":[{"name":"puzzleId","type":"uint256"}],"name":"GetPuzzleMetrics","outputs":[{"name":"","type":"bytes"}],"payable":false,"stateMutability":"view","type":"function"},{"constant":true,"inputs":[{"name":"puzzleId","type":"uint256"}],"name":"CompareMetrics","outputs":[{"name":"","type":"bool"}],"payable":false,"stateMutability":"view","type":"function"},{"constant":true,"inputs":[],"name":"owner","outputs":[{"name":"","type":"address"}],"payable":false,"stateMutability":"view","type":"function"},{"constant":true,"inputs":[{"name":"puzzleId","type":"uint256"}],"name":"GetPuzzleOriginalMetrics","outputs":[{"name":"","type":"string"}],"payable":false,"stateMutability":"view","type":"function"},{"constant":false,"inputs":[{"name":"metrics","type":"string"},{"name":"uniqueId","type":"string"}],"name":"CreatePuzzle","outputs":[{"name":"","type":"uint256"}],"payable":false,"stateMutability":"nonpayable","type":"function"},{"constant":false,"inputs":[{"name":"puzzleId","type":"uint256"},{"name":"metrics","type":"string"}],"name":"PushMetrics","outputs":[{"name":"","type":"bool"}],"payable":false,"stateMutability":"nonpayable","type":"function"},{"constant":false,"inputs":[{"name":"newOwner","type":"address"}],"name":"transferOwnership","outputs":[],"payable":false,"stateMutability":"nonpayable","type":"function"},{"inputs":[],"payable":false,"stateMutability":"nonpayable","type":"constructor"},{"anonymous":false,"inputs":[{"indexed":false,"name":"puzzleId","type":"uint256"},{"indexed":false,"name":"uniqueId","type":"string"}],"name":"PuzzleCreated","type":"event"},{"anonymous":false,"inputs":[{"indexed":true,"name":"previousOwner","type":"address"},{"indexed":true,"name":"newOwner","type":"address"}],"name":"OwnershipTransferred","type":"event"}],
                    net: 'ropsten',
                    address: '0xf792c43f23c39f7de185cfdc6ce96aa69e9f00c1',
                }
            }
        },
        
        
        _web3: {},
    
        getWeb3 (url) {
            let web3 = this._web3[url]
            if (web3 == undefined) {
                if (url == 'metamask')
                    web3 = window.web3
    //			else
    //				web3 = new Web3(new Web3.providers.HttpProvider(url))
                this._web3[url] = web3
                this.initWeb3(web3, this.options)
            }
            return web3
        },
    
        defaultWeb3 () {
            return this.getWeb3('metamask')
        },
    
        initWeb3 (web3, options) {
            //    web3.eth.subscribe('an_event', (error, event) => {})
            this.playerScore = this.contractInit(web3, options.contracts['PlayerScore'])
            this.puzzleManager = this.contractInit(web3, options.contracts['PuzzleManager'])
    
            // start events timer
            
            var self = this
    
            var fromBlock = 0
    
            setInterval(()=>{
                self.puzzleManager.PuzzleCreated({}, (error, event)=>{
                    fromBlock = event.blockNumber
                    var puzzleId = event.args.puzzleId.c[0]
                    var requestId = parseInt(event.args.uniqueId)
                    
//                    console.log(`Received PuzzleCreated from block ${fromBlock} puzzleId=${puzzleId} requestId=${requestId}`)

                    // LINK to HIPRInternal

                    HIPRInternal.setRequestValue(requestId, puzzleId)
                })
            }, 1000)
        },
    
        contractInit (web3, options) {
            let contract = web3.eth.contract(options.abi).at(options.address)
            
            if (contract)
                console.log('new contract success at address:', options.address)
    
            return contract
        },
    
        contractGetPastEvents (contract, eventName, options, cb) {
            console.log(contract)
            contract.getPastEvents(eventName, options, (error, results) => {
                if (error) {
                    console.log(error)
                    return
                }
                results.forEach(event => {
                    cb(event)
                })
            })
        },
    
        // web3 ]
    }
    