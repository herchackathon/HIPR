Web3Options = {
    env: 'production', 
    dev: {
        hipr_restful: 'http://localhost:8086/api/1.0',
        eth: 'ropsten'
    },
    production: {
        hipr_restful: 'http://server:8086/api/1.0',
        eth: 'mainnet'
    },
    contracts: {
        mainnet: {
            'PlayerScore': {
                abi: [{"constant":true,"inputs":[],"name":"startDate","outputs":[{"name":"","type":"uint256"}],"payable":false,"stateMutability":"view","type":"function"},{"constant":true,"inputs":[],"name":"hercContract","outputs":[{"name":"","type":"address"}],"payable":false,"stateMutability":"view","type":"function"},{"constant":true,"inputs":[],"name":"seasonInterval","outputs":[{"name":"","type":"uint256"}],"payable":false,"stateMutability":"view","type":"function"},{"constant":false,"inputs":[],"name":"renounceOwnership","outputs":[],"payable":false,"stateMutability":"nonpayable","type":"function"},{"constant":true,"inputs":[{"name":"","type":"uint256"}],"name":"TopScores","outputs":[{"name":"player","type":"address"},{"name":"score","type":"int256"}],"payable":false,"stateMutability":"view","type":"function"},{"constant":true,"inputs":[{"name":"","type":"address"}],"name":"Scores","outputs":[{"name":"","type":"int256"}],"payable":false,"stateMutability":"view","type":"function"},{"constant":true,"inputs":[],"name":"owner","outputs":[{"name":"","type":"address"}],"payable":false,"stateMutability":"view","type":"function"},{"constant":true,"inputs":[],"name":"isOwner","outputs":[{"name":"","type":"bool"}],"payable":false,"stateMutability":"view","type":"function"},{"constant":true,"inputs":[],"name":"lastWipeDate","outputs":[{"name":"","type":"uint256"}],"payable":false,"stateMutability":"view","type":"function"},{"constant":true,"inputs":[],"name":"payoutBoss","outputs":[{"name":"","type":"address"}],"payable":false,"stateMutability":"view","type":"function"},{"constant":true,"inputs":[{"name":"","type":"uint256"}],"name":"winnerReward","outputs":[{"name":"","type":"uint256"}],"payable":false,"stateMutability":"view","type":"function"},{"constant":true,"inputs":[],"name":"releaseDate","outputs":[{"name":"","type":"uint256"}],"payable":false,"stateMutability":"view","type":"function"},{"constant":false,"inputs":[{"name":"newOwner","type":"address"}],"name":"transferOwnership","outputs":[],"payable":false,"stateMutability":"nonpayable","type":"function"},{"anonymous":false,"inputs":[{"indexed":false,"name":"player","type":"address"},{"indexed":false,"name":"rank","type":"uint256"},{"indexed":false,"name":"reward","type":"uint256"}],"name":"WinnerPayout","type":"event"},{"anonymous":false,"inputs":[{"indexed":true,"name":"previousOwner","type":"address"},{"indexed":true,"name":"newOwner","type":"address"}],"name":"OwnershipTransferred","type":"event"},{"constant":false,"inputs":[{"name":"score","type":"int256"}],"name":"SetScore","outputs":[],"payable":false,"stateMutability":"nonpayable","type":"function"},{"constant":true,"inputs":[],"name":"GetTopScoresCount","outputs":[{"name":"","type":"uint256"}],"payable":false,"stateMutability":"view","type":"function"},{"constant":true,"inputs":[],"name":"GetTopScoresMax","outputs":[{"name":"","type":"uint256"}],"payable":false,"stateMutability":"view","type":"function"},{"constant":false,"inputs":[],"name":"SetScoreSecure","outputs":[],"payable":false,"stateMutability":"nonpayable","type":"function"},{"constant":false,"inputs":[{"name":"hercContract_","type":"address"}],"name":"SetHERCTokenAddress","outputs":[],"payable":false,"stateMutability":"nonpayable","type":"function"},{"constant":false,"inputs":[{"name":"boss","type":"address"}],"name":"SetPayoutAddress","outputs":[],"payable":false,"stateMutability":"nonpayable","type":"function"},{"constant":false,"inputs":[{"name":"rank","type":"uint256"},{"name":"reward","type":"uint256"}],"name":"SetWinnerReward","outputs":[],"payable":false,"stateMutability":"nonpayable","type":"function"},{"constant":false,"inputs":[{"name":"startDate_","type":"uint256"},{"name":"releaseDate_","type":"uint256"}],"name":"SetNextSeasonReleaseDate","outputs":[],"payable":false,"stateMutability":"nonpayable","type":"function"},{"constant":false,"inputs":[{"name":"interval","type":"uint256"}],"name":"SetSeasonInterval","outputs":[],"payable":false,"stateMutability":"nonpayable","type":"function"},{"constant":true,"inputs":[],"name":"IsSeasonOver","outputs":[{"name":"","type":"bool"}],"payable":false,"stateMutability":"view","type":"function"},{"constant":false,"inputs":[],"name":"PayoutToWinners","outputs":[],"payable":false,"stateMutability":"nonpayable","type":"function"},{"constant":false,"inputs":[],"name":"WipeScores","outputs":[],"payable":false,"stateMutability":"nonpayable","type":"function"}],
                address: '0xf1886887f07ef5392c41a976cec1eb8443dcad85',//'0xA07B1FE246D9020f6884eA9d432B551Ea534b13f',
                validation: {
                    hash: '',
                    deployDate: '',
                    deployBy: '',
                    abiHash: '',
                    sourceSize: 0,
                    sourceLines: 0,
                    sourceHash: ''
                }
            },
            'PuzzleManager': {
                abi: [{"constant":false,"inputs":[],"name":"acceptPuzzle","outputs":[],"payable":false,"stateMutability":"nonpayable","type":"function"},{"constant":true,"inputs":[{"name":"puzzleId","type":"uint256"}],"name":"GetPuzzleMetrics","outputs":[{"name":"","type":"bytes"}],"payable":false,"stateMutability":"view","type":"function"},{"constant":true,"inputs":[{"name":"puzzleId","type":"uint256"}],"name":"CompareMetrics","outputs":[{"name":"","type":"bool"}],"payable":false,"stateMutability":"view","type":"function"},{"constant":true,"inputs":[],"name":"owner","outputs":[{"name":"","type":"address"}],"payable":false,"stateMutability":"view","type":"function"},{"constant":true,"inputs":[{"name":"puzzleId","type":"uint256"}],"name":"GetPuzzleOriginalMetrics","outputs":[{"name":"","type":"string"}],"payable":false,"stateMutability":"view","type":"function"},{"constant":false,"inputs":[{"name":"metrics","type":"string"},{"name":"uniqueId","type":"string"}],"name":"CreatePuzzle","outputs":[{"name":"","type":"uint256"}],"payable":false,"stateMutability":"nonpayable","type":"function"},{"constant":false,"inputs":[{"name":"puzzleId","type":"uint256"},{"name":"metrics","type":"string"}],"name":"PushMetrics","outputs":[{"name":"","type":"bool"}],"payable":false,"stateMutability":"nonpayable","type":"function"},{"constant":false,"inputs":[{"name":"newOwner","type":"address"}],"name":"transferOwnership","outputs":[],"payable":false,"stateMutability":"nonpayable","type":"function"},{"inputs":[],"payable":false,"stateMutability":"nonpayable","type":"constructor"},{"anonymous":false,"inputs":[{"indexed":false,"name":"puzzleId","type":"uint256"},{"indexed":false,"name":"uniqueId","type":"string"}],"name":"PuzzleCreated","type":"event"},{"anonymous":false,"inputs":[{"indexed":true,"name":"previousOwner","type":"address"},{"indexed":true,"name":"newOwner","type":"address"}],"name":"OwnershipTransferred","type":"event"}],
                address: '0xf792c43f23c39f7de185cfdc6ce96aa69e9f00c1',
                validation: {
                    hash: '',
                    deployDate: '',
                    deployBy: '',
                    abiHash: '',
                    sourceSize: 0,
                    sourceLines: 0,
                    sourceHash: ''
                }
            }
        },
        ropsten: {
            'PlayerScore': {
                abi: [{"anonymous": false,"inputs": [{"indexed": true,"name": "previousOwner","type": "address"},{"indexed": true,"name": "newOwner","type": "address"}],"name": "OwnershipTransferred","type": "event"},{"constant": false,"inputs": [],"name": "renounceOwnership","outputs": [],"payable": false,"stateMutability": "nonpayable","type": "function"},{"constant": false,"inputs": [{"name": "score","type": "int256"}],"name": "SetScore","outputs": [],"payable": false,"stateMutability": "nonpayable","type": "function"},{"constant": false,"inputs": [{"name": "newOwner","type": "address"}],"name": "transferOwnership","outputs": [],"payable": false,"stateMutability": "nonpayable","type": "function"},{"constant": true,"inputs": [],"name": "GetTopScoresCount","outputs": [{"name": "","type": "uint256"}],"payable": false,"stateMutability": "view","type": "function"},{"constant": true,"inputs": [],"name": "isOwner","outputs": [{"name": "","type": "bool"}],"payable": false,"stateMutability": "view","type": "function"},{"constant": true,"inputs": [],"name": "owner","outputs": [{"name": "","type": "address"}],"payable": false,"stateMutability": "view","type": "function"},{"constant": true,"inputs": [{"name": "","type": "address"}],"name": "Scores","outputs": [{"name": "","type": "int256"}],"payable": false,"stateMutability": "view","type": "function"},{"constant": true,"inputs": [{"name": "","type": "uint256"}],"name": "TopScores","outputs": [{"name": "player","type": "address"},{"name": "score","type": "int256"}],"payable": false,"stateMutability": "view","type": "function"}],//[{"constant":false,"inputs":[{"name":"score","type":"int256"}],"name":"SetScore","outputs":[],"payable":false,"stateMutability":"nonpayable","type":"function"},{"constant":true,"inputs":[{"name":"","type":"uint256"}],"name":"TopScores","outputs":[{"name":"player","type":"address"},{"name":"score","type":"int256"}],"payable":false,"stateMutability":"view","type":"function"},{"constant":true,"inputs":[{"name":"","type":"address"}],"name":"Scores","outputs":[{"name":"","type":"int256"}],"payable":false,"stateMutability":"view","type":"function"},{"constant":true,"inputs":[],"name":"GetTopScoresCount","outputs":[{"name":"","type":"uint256"}],"payable":false,"stateMutability":"view","type":"function"}],
                address: '0xeed0eb7a4251ce217b7d37d370267735626ad2c6',//'0xA07B1FE246D9020f6884eA9d432B551Ea534b13f',
                validation: {
                    hash: '',
                    deployDate: '',
                    deployBy: '',
                    abiHash: '',
                    sourceSize: 0,
                    sourceLines: 0,
                    sourceHash: ''
                }
            },
            'PuzzleManager': {
                abi: [{"constant":false,"inputs":[],"name":"acceptPuzzle","outputs":[],"payable":false,"stateMutability":"nonpayable","type":"function"},{"constant":true,"inputs":[{"name":"puzzleId","type":"uint256"}],"name":"GetPuzzleMetrics","outputs":[{"name":"","type":"bytes"}],"payable":false,"stateMutability":"view","type":"function"},{"constant":true,"inputs":[{"name":"puzzleId","type":"uint256"}],"name":"CompareMetrics","outputs":[{"name":"","type":"bool"}],"payable":false,"stateMutability":"view","type":"function"},{"constant":true,"inputs":[],"name":"owner","outputs":[{"name":"","type":"address"}],"payable":false,"stateMutability":"view","type":"function"},{"constant":true,"inputs":[{"name":"puzzleId","type":"uint256"}],"name":"GetPuzzleOriginalMetrics","outputs":[{"name":"","type":"string"}],"payable":false,"stateMutability":"view","type":"function"},{"constant":false,"inputs":[{"name":"metrics","type":"string"},{"name":"uniqueId","type":"string"}],"name":"CreatePuzzle","outputs":[{"name":"","type":"uint256"}],"payable":false,"stateMutability":"nonpayable","type":"function"},{"constant":false,"inputs":[{"name":"puzzleId","type":"uint256"},{"name":"metrics","type":"string"}],"name":"PushMetrics","outputs":[{"name":"","type":"bool"}],"payable":false,"stateMutability":"nonpayable","type":"function"},{"constant":false,"inputs":[{"name":"newOwner","type":"address"}],"name":"transferOwnership","outputs":[],"payable":false,"stateMutability":"nonpayable","type":"function"},{"inputs":[],"payable":false,"stateMutability":"nonpayable","type":"constructor"},{"anonymous":false,"inputs":[{"indexed":false,"name":"puzzleId","type":"uint256"},{"indexed":false,"name":"uniqueId","type":"string"}],"name":"PuzzleCreated","type":"event"},{"anonymous":false,"inputs":[{"indexed":true,"name":"previousOwner","type":"address"},{"indexed":true,"name":"newOwner","type":"address"}],"name":"OwnershipTransferred","type":"event"}],
                address: '0xf792c43f23c39f7de185cfdc6ce96aa69e9f00c1',
                validation: {
                    hash: '',
                    deployDate: '',
                    deployBy: '',
                    abiHash: '',
                    sourceSize: 0,
                    sourceLines: 0,
                    sourceHash: ''
                }
            }
        }
    }
}
