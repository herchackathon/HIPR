Web3Internal = {
    // web3 [

        options: null,
    
        _web3: {},
    
        getWeb3 (url) {
            let web3 = this._web3[url]
            if (web3 == undefined) {
                if (url == 'metamask')
                    web3 = window.web3
    //			else
    //				web3 = new Web3(new Web3.providers.HttpProvider(url))
                this._web3[url] = web3
                this.initWeb3(web3, Web3Options)
            }
            return web3
        },
    
        defaultWeb3 () {
            return this.getWeb3('metamask')
        },
    
        initWeb3 (web3, options) {
            this.options = options

            var contracts = options.contracts[options[options.env].eth]
            
            //    web3.eth.subscribe('an_event', (error, event) => {})
            this.playerScore = this.contractInit(web3, contracts['PlayerScore'])
            this.puzzleManager = this.contractInit(web3, contracts['PuzzleManager'])
    
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
    