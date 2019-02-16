// API

// CALLBACK

function SendResultBack(key, result)
{
    var message = key + "#" + JSON.stringify(result.data);
    console.log('SendResultBack', message)
    gameInstance.SendMessage('JavascriptInteractor', 'ProcessResult', message);
}

// METHODS

function GetEndOfSeasonInternal()
{
    //SendResultBack("GetEndOfSeason", "1548979199000");
    HIPRInternal.PayoutInfo();
}

function GetPuzzleInternal()
{
    var address = window.web3.eth.accounts[0]
    HIPRInternal.GetPuzzle(address)

    //HIPRInternal.GetPuzzle()
    //SendResultBack("GetPuzzle", "ajdhajsndjsadjsah");
}

function GetTopScoresInternal(count)
{
    HIPRInternal.GetTopScores(count)
    // -> "0x111111111|15;0x22222222|12;0x33333333|11;0x444444444|9;0x555555555|3;"
}

var lastScore

function SetScoreInternal(score)
{
    lastScore = score

//    HIPRInternal.SetScore(score);
    // -> "true"
}

function ValidatePuzzleResultInternal(puzzleId, score, resultHash, movesSet)
{
    var puzzleId // lastest PuzzleId returned by createPuzzle
    var address = window.web3.eth.accounts[0]
    HIPRInternal.ValidatePuzzle(puzzleId, address, score, resultHash, movesSet)

    //    SendResultBack('ValidatePuzzleResult', 'true');
    //return HIPRInternal.ValidatePuzzleResult();
    // -> "true"
}

function PlayerPayoutInternal()
{
	var address = window.web3.eth.accounts[0];
	
	console.log("PlayerPayout pressed!");
	//SendResultBack("PlayerPayout", 213812);
	/*SendResultBack("GetPuzzle", 
		'{"puzzleId":53,"hash":"0xb5ae2176999f7f4169c5c97eadab42c2b536c7196c2f9d3abdeaa2faf5f64ca8","field":[6,3,2,7,1,5,0,4,8]}'
	);*/
	HIPRInternal.PlayerPayout(address);
}