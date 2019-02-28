// see ui.css

HiprUI = {
    init: function() {
        if (this.isDebug()) {
            
            var self = this

            // create menu button

            //&equiv;
            $('<div id="menu_web3_info">web3</div>').insertBefore("#menu_toggle")
            $('#menu_web3_info').on("click", function(event) {
                console.log('web3 info')
                self.toggleWeb3Info()
            })

//            self.toggleWeb3Info()
        }
    },
    isDebug: function() {
        if (window.location.hostname == 'hipr.one')
            return false

        return true
    },

    toggleWeb3Info: function() {
        var web3View = $('#web3_view')
        if (web3View.length == 0 || web3View.hasClass("hide")) {
            this.showWeb3View()
        }
        else {
            $('#web3_view').remove()
        }
/*
        if (web3View.hasClass("hide")) {
            // show
            web3View.removeClass("hide");
        }
        else {
            // hide
            web3View.addClass("hide");
        }*/
    },

    showWeb3View: function() {
        // create web3_view

        $('<div id="web3_view" class="">').insertAfter('#menu')
        $('#web3_view').html(`
            <div class="web3_dialog">
                <div class="title">
                    HIPR Web3 Inspector
                </div>
                <div class="web3-table-info">
                    <table class="web3">
                        <tbody>
                        </tbody>
                    </table>
                </div>
            </div>
        `)

        try {
//            a.b.c
            this.tableWeb3addRow('env', Web3Options.env)
            var envopt = Web3Options[Web3Options.env]

            var eth = envopt['eth']
            var ethColor = 'black'
            if (eth == 'ganache') ethColor = 'purple'
            if (eth == 'ropsten') ethColor = 'blue'
            if (eth == 'main') ethColor = 'red'
            this.tableWeb3addRow('eth', `<div style="color: ${ethColor}; font-weight: bold">${envopt['eth']}</div>`)

            // update eth & hipr status

            var ethConnected = !!window.web3
            this.tableWeb3addRow('eth status', `<div style="color: ${ethConnected ? "green" : "red"}; font-weight: bold">${ethConnected ? 'connected' : 'not connected'}</div>`)

            this.tableWeb3addRow('hipr_restful', envopt['hipr_restful'])

            this.restStatus = 'pending'
            this.restPendingTime = 0

            var text = this.getRestStatusHTML(this.restStatus, this.restPendingTime)
            this.tableWeb3addRow('hipr_restful status', text, {tags: 'id="rest-status-row"'})

            this.updateHiprRestfultStats()

            // update hipr contracts

            var playerScore = Web3Options['contracts'][eth]['PlayerScore']
            var assetValidator = Web3Options['contracts'][eth]['PuzzleManager']
            
            this.tableWeb3addRow('HIPR Contracts', '',  {style: 'color: #ecd23a; background-color: #08112b;'})

            this.tableWeb3AddContract('PlayerScore', playerScore)
            this.tableWeb3AddContract('assetValidator', assetValidator)

            this.tableWeb3addRow('HIPR State', '',  {style: 'color: #ecd23a; background-color: #08112b;'})

            this.tableWeb3addRow('Season', '', {style: 'font-weight: bold'})
            this.tableWeb3addRow('startDate', '...', {tags: 'id="season-start-date"'})
            this.tableWeb3addRow('releaseDate', '...', {tags: 'id="season-release-date"'})
            this.tableWeb3addRow('seasonInterval', '...', {tags: 'id="season-interval"'})
            this.tableWeb3addRow('lastWipeDate', '...', {tags: 'id="season-last-wipe-date"'})
            
            this.tableWeb3addRow('Top scoresS max', '...', {tags: 'id="top-scores-max"', style: 'font-weight: bold'})
            this.tableWeb3addRow('Top scoresS', '...', {tags: 'id="top-scores-count"', style: 'font-weight: bold'})
            

            this.tableWeb3addRow('Rewards', '<table id="table-rewards"></table>')
            this.tableWeb3addRow('Payout Approves', '')

            // hipr-restufl state

//            this.tableWeb3addRow('HIPR-RESTFUL State', '',  {style: 'color: #ecd23a; background-color: #08112b;'})
//            this.tableWeb3addRow('Contracts', '')

            // tests

            var self = this

            this.tableWeb3addRow('HIPR Tests', '',  {style: 'color: #ecd23a; background-color: #08112b;'})

            // Test #1

            try {
                var address = window.web3.eth.accounts[0]
                var ipfsParams = `${address}/QmQhM65XyqJ52QXWPz2opaGkALgH8XXhPn8n8nff4LDE6C/QmQhM65XyqJ52QXWPz2opaGkALgH8XXhPn8n8nff4LDE6C`

                this.tableWeb3addRow('Test #1', '<div class="test-name">Register puzzle</div><button id="test1-bn-run" class="test-bn">Run</button>')
                this.tableWeb3addRow('- address', `<input id="test1-input-address" class="test-input" value="${address}"></input>`)
                this.tableWeb3addRow('- ipfs params', `<input id="test1-input-params" class="test-input" value="${ipfsParams}"></input>`)
                $('#test1-bn-run').click(()=>{
                    self.runTest('test1')
                })
            }
            catch (e)
            {
                this.tableWeb3addRow('#ERROR Test #1', e, {style: 'color: red'})
            }

            // Test #2
/*
            try {
                var address = window.web3.eth.accounts[0]
                var password

                this.tableWeb3addRow('restPendingTimeTest #2', '<div class="test-name">Create puzzle</div><button id="test2-bn-run" class="test-bn">Run</button>')
                this.tableWeb3addRow('- address', `<input id="test2-input-address" class="test-input" value="${address}"></input>`)
                this.tableWeb3addRow('- password', `<input id="test2-input-password" class="test-input" value="${password}"></input>`)
                $('#test2-bn-run').click(()=>{
                    self.runTest('test2')
                })
            }
            catch (e)
            {
                this.tableWeb3addRow('#ERROR Test #1', e, {style: 'color: red'})
            }
*/
            // Test #3
/*
            try {
                var puzzleId = 42
                var address = window.web3.eth.accounts[0]
                var score = 1000000
                var resultHash = '0x'//options.resultHash
                var movesSet = '7,8,6'//options.movesSet

                this.tableWeb3addRow('Test #3', '<div class="test-name">Validate puzzle</div><button id="test3-bn-run" class="test-bn">Run</button>')
                this.tableWeb3addRow('- puzzleId', `<input id="test3-input-puzzle-id" class="test-input" value="${puzzleId}"></input>`)
                this.tableWeb3addRow('- address', `<input id="test3-input-address" class="test-input" value="${address}"></input>`)
                this.tableWeb3addRow('- score', `<input id="test3-input-score" class="test-input" value="${score}"></input>`)
                this.tableWeb3addRow('- resultHash', `<input id="test3-input-result-hash" class="test-input" value="${resultHash}"></input>`)
                this.tableWeb3addRow('- movesSet', `<input id="test3-input-moves-set" class="test-input" value="${movesSet}"></input>`)
                $('#test3-bn-run').click(()=>{
                    self.runTest('test3')
                })
            }
            catch (e)
            {
                this.tableWeb3addRow('#ERROR Test #1', e, {style: 'color: red'})
            }
*/
            this.updateSeasonStats()
        }
        catch (e) {
            this.tableWeb3addRow('#ERROR', e, {style: 'color: red'})
        }
        var web3View = $('#web3_view')
    },
    
    runTest(testId) {
        var url
        var self = this

        if (testId == 'test1') {
            var address = $('#test1-input-address').val()
            var ipfsParams = $('#test1-input-params').val()
        
            self.testLog('Test #1: Result', `run ${address} ${ipfsParams}`, {style: 'color: #224488; font-weight: bold'})

            url = hiprUrl('registerPuzzle', {address, params: ipfsParams})
        }
        else if (testId == 'test2') {
            var address = $('#test2-input-address').val()
            var password = $('#test2-input-password').val()

            self.testLog('Test #2: Result', `run ${address} ${password}`, {style: 'color: #224488; font-weight: bold'})

            url = hiprUrl('createPuzzle', {address, password})
        }
        else if (testId == 'test3') {
            var puzzleId = $('#test3-input-puzzle-id').val()
            var address = $('#test3-input-address').val()
            var score = $('#test3-input-score').val()
            var resultHash = $('#test3-input-result-hash').val()
            var movesSet = $('#test3-input-moves-set').val()
    
            self.testLog('Test #3: Result', `run ${address} ${score} '${resultHash}' ${movesSet}`, {style: 'color: #224488; font-weight: bold'})

            var url = hiprUrl('validatePuzzle', {puzzleId, address, score, resultHash, movesSet})
/*
            axios.get(url)
            .then(function (response) {
                var data = response.data
                self.testLog('HIPR-RESTFUL Response', `<div style="max-width: 600px; overflow: auto;">${JSON.stringify(data, null, 2)}<div>`, {style: 'color: #224488; font-weight: bold'})
            })
            .catch(function (error) {
                self.testLog(`#ERROR ${testId}`, JSON.stringify(error), {style: 'color: red'})
            })
    
            return*/
        }

        axios.post(url)
        .then(function (response) {
            var data = response.data
            self.testLog('HIPR-RESTFUL Response', `<div style="max-width: 600px; overflow: auto;">${JSON.stringify(data, null, 2)}<div>`, {style: 'color: #224488; font-weight: bold'})
        })
        .catch(function (error) {
            self.testLog(`#ERROR ${testId}`, JSON.stringify(error), {style: 'color: red'})
        })

    },

    testLog(name, value, opts) {
        this.tableWeb3addRow(name, `<div style="max-width: 580px; overflow: auto;">${value}</div>`, opts)
    },

    tableWeb3addRow: function(name, value, opts) {
        var trTags = opts && opts.tags ? opts.tags : ''
        var trStyle = opts && opts.style ? 'style="'+opts.style+'"' : ''
        var row
        if (opts && opts.rows == 1) {
            row = `
                <tr ${trStyle} ${trTags} >
                    <td>
                        ${name}
                    </td>
                </tr>
            `
        }
        else {
            row = `
                <tr ${trStyle} ${trTags} >
                    <td>
                        ${name}
                    </td>
                    <td>
                        ${value}
                    </td>
                </tr>
            `
        }
        $('table.web3 > tbody').append(row)
    },

    tableWeb3AddContract(name, contract) {
        //            this.tableWeb3addRow('PlayerScore', '') //0, {rows: 1})
        this.tableWeb3addRow(name, contract.address, {style: 'background-color: #e8e12b;'}) //0, {rows: 1})

//            this.tableWeb3addRow('address', playerScore.address)
        if (contract.validation) {
            var validation = contract.validation.validation
            var o = {style: 'color: #050'}
            this.tableWeb3addRow('deployDate', validation.deployDate, o)
            this.tableWeb3addRow('deployBy', validation.deployBy, o)
            this.tableWeb3addRow('sourceSize', validation.sourceSize, o)
            this.tableWeb3addRow('sourceLines', validation.sourceLines, o)
            this.tableWeb3addRow('sourceHash', validation.sourceHash, o)
            this.tableWeb3addRow('abiHash', validation.abiHash, o)
        }
        else {
            this.tableWeb3addRow('validation', 'contract is not validated', {style: 'color: red'})
        }
    },

    getRestStatusHTML: function (restStatus, i) {
        var restStatusColor = restStatus == 'pending' ? "orange" : (restStatus == 'connected' ? "green" : "red")
        var s = restStatus == 'pending' ? `pending (${i})...` : restStatus
        return `<div style="color: ${restStatusColor}; font-weight: bold">${s}</div>`
    },

    restStatusRequest: function () {
        var self = this

        var requestInterval = 10 // seconds

        if (self.restPendingTime % requestInterval == 0) {
            var url = hiprUrl('status', {})

            axios.get(url)
            .then(function (response) {
                if (!response.data || !response.data.status) {
                    self.testLog(`#ERROR status`, JSON.stringify(response), {style: 'color: red'})
                    return
                }

//                clearInterval(self.restStatusInterval)
                if (self.restStatus == 'connected')
                    return

                var status = response.data.status
//                self.testLog(`#STATUS`, JSON.stringify(status), {style: 'color: purple'})
                self.restStatus = 'connected'
                self.tableWeb3addRow('HIPR-RESTFUL State', '',  {style: 'color: #ecd23a; background-color: #08112b;'})
                self.tableWeb3addRow('started', status.startDate)
                self.tableWeb3addRow('last update', status.nowDate)
                self.tableWeb3addRow('chain', JSON.stringify(status.chain))
                self.tableWeb3AddContract('PlayerScore', status.eth.playerScore)
                self.tableWeb3AddContract('assetValidator', status.eth.assetValidator)
            })
            .catch(function (error) {
                self.testLog(`#ERROR status`, JSON.stringify(error), {style: 'color: red'})
                self.restStatus = 'pending'
            })
        }
    },

    updateHiprRestfultStats: function () {
        // hipr connection refresh [

        var self = this

        function restStatusUpdate() {
            self.restStatusRequest()
            self.restStatusInterval = setInterval(()=>{
                var text = self.getRestStatusHTML(self.restStatus, ++self.restPendingTime)
                $('#rest-status-row > td:eq(1)').html(text)
                self.restStatusRequest()
            }, 1000)
        }

        restStatusUpdate()

        // hipr connection refresh ]
    },

    updateSeasonStats: async function() {
        var ps = HIPRInternal.playerScore

        var self = this

        // season

        function dateValue(v) {
//            return `${v} ${new Date(v) .toISOString()}`
            return v > 10 ? `${v} (${new Date(v).toUTCString()})` : v
        }
        
        ps.startDate((error, result)=>{
            $('#season-start-date > td:eq(1)').html(error ? JSON.stringify(error) : dateValue(result.c[0]))
        })
        ps.releaseDate((error, result)=>{
            $('#season-release-date > td:eq(1)').html(error ? JSON.stringify(error) : dateValue(result.c[0]))
        })
        ps.seasonInterval((error, result)=>{
            $('#season-interval > td:eq(1)').html(error ? JSON.stringify(error) : result.c[0])
        })
        ps.lastWipeDate((error, result)=>{
            $('#season-last-wipe-date > td:eq(1)').html(error ? JSON.stringify(error) : dateValue(result.c[0] * 1000))
        })

        // top scores

        ps.GetTopScoresSecureCount((error, result)=>{
            var count = error ? 0 : result.c[0]
            $('#top-scores-count > td:eq(1)').html(error ? JSON.stringify(error) : count)
            $('#top-scores-count > td:eq(1)').append('<button id="top-scores-bn-populate" style="margin: 0 10px;">populate</button>')
            $('#top-scores-count > td:eq(1)').append('<table id="top-scores-list"></table>')
            $('#top-scores-bn-populate').click(()=>{
                $('#top-scores-bn-populate').hide()
                for (var i = 0; i < count; i++) {
                    ps.TopScoresSecure(i, (error, result)=>{
                        if (!error) {
                            var address = result[0]
                            var score = result[1].c[0]
                            $('#top-scores-list').append(`<tr><td>${address}</td><td>${score}</td></tr>`)
                        }
                        else {
                            $('#top-scores-list').append(`<tr><td>${JSON.stringify(error)}</td></tr>`)
                        }
                    })
                }

            })
        })
        
        // rewards
        
        ps.GetTopScoresMax((error, result)=>{
            if (!error) {
                let rewardsCount = result.c[0]
                
                $('#top-scores-max > td:eq(1)').html(rewardsCount)

                var rewards = []
                for (var i = 0; i < rewardsCount; i++) {
                    ps.winnerReward(i, (error, result)=>{
                        rewards.push(!error ? result : JSON.stringify(error))
                        if (rewards.length == rewardsCount) {
                            var m = 10
                            var n = rewardsCount / m
                            for (var j = 0; j < n; j++) {
                                var s = ''
                                for (var i = 0; i < m; i++) {
                                    var k = j * m + i
                                    var reward = rewards[k]
                                    s += `<td>${k}-${reward}</td>`
                                }
                                $('#table-rewards').append(`<tr>${s}</tr>`)
                            }
                        }
                    })
                }
            }
        })
            
        
//        $('#top-scores-count > td:eq(1)').html('<table><tr><td>123</td><tr><tr><td>123</td><tr></table>')
/*
        m.releaseDate((error, result) => {
            if (error)
        }
        */
    }


}

window.addEventListener('load', async () => {
    HIPRInternal.init()
    HiprUI.init()
})
