// API

// CALLBACK

function SendResultBack(key, result)
{
    var message = key + "#" + result;
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
	
	SendResultBack("PlayerPayout", 213812);
	//HIPRInternal.PlayerPayout(address);
}