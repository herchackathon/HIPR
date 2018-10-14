using MHLab.SlidingTilePuzzle.Data;
using MHLab.Web.Storage;
using Nethereum.JsonRpc.UnityClient;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.Transactions;
using UnityEngine;

namespace MHLab.Nethereum
{
    public class Account
    {
        public string Address = "";
        public string PrivateKey = "";
        public decimal Balance;
    }

    public class AccountManager
    {
        public static Account Account;

        public static string EthereumEndpoint = "https://rinkeby.infura.io/CHs7q12LsOAlHu4D3Kvr";

        // TODO: remove hardcoded ABI and Addresses. Create a web service to fetch information from. This is just to speed things up.
        public static string AssignTokenContractAbi = @"";
        // TODO: remove hardcoded ABI and Addresses. Create a web service to fetch information from. This is just to speed things up.
        public static string AssignTokenContractAddress = "0x0C97b0B42140D77dE45Fc669E826225E6bb6B3D2";

        // TODO: remove hardcoded ABI and Addresses. Create a web service to fetch information from. This is just to speed things up.
        public static string PlayerScoreContractAbi = @"[{""constant"": false,""inputs"": [{""name"": ""score"",""type"": ""int256""}],""name"": ""SetScore"",""outputs"": [],""payable"": false,""stateMutability"": ""nonpayable"",""type"": ""function""},{""constant"": true,""inputs"": [{""name"": """",""type"": ""uint256""}],""name"": ""TopScores"",""outputs"": [{""name"": ""player"",""type"": ""address""},{""name"": ""score"",""type"": ""int256""}],""payable"": false,""stateMutability"": ""view"",""type"": ""function""},{""constant"": true,""inputs"": [{""name"": """",""type"": ""address""}],""name"": ""Scores"",""outputs"": [{""name"": """",""type"": ""int256""}],""payable"": false,""stateMutability"": ""view"",""type"": ""function""},{""constant"": true,""inputs"": [],""name"": ""GetTopScoresCount"",""outputs"": [{""name"": """",""type"": ""uint256""}],""payable"": false,""stateMutability"": ""view"",""type"": ""function""}]";
        // TODO: remove hardcoded ABI and Addresses. Create a web service to fetch information from. This is just to speed things up.
        public static string PlayerScoreContractAddress = "0xa1F8C3310b944732150507Fb67edd70c75A4C5a1";

        // TODO: remove hardcoded ABI and Addresses. Create a web service to fetch information from. This is just to speed things up.
        public static string PuzzleManagerContractAbi = @"";
        // TODO: remove hardcoded ABI and Addresses. Create a web service to fetch information from. This is just to speed things up.
        public static string PuzzleManagerContractAddress = "";

        public static IEnumerator Login(string accountAddress, string privateKey, Action<bool> callback)
        {
            var getBalanceRequest = new EthGetBalanceUnityRequest(EthereumEndpoint);
            yield return getBalanceRequest.SendRequest(accountAddress, BlockParameter.CreateLatest());

            if (getBalanceRequest.Exception == null)
            {
                var balance = getBalanceRequest.Result.Value;

                var account = new Account
                {
                    Address = accountAddress,
                    Balance = UnitConversion.Convert.FromWei(balance, 18),
                    PrivateKey = privateKey
                };

                Account = account;
                LocalStorage.Store(StorageKeys.AccountAddressKey, accountAddress);
                LocalStorage.Store(StorageKeys.AccountPrivateKey, privateKey);

                callback.Invoke(true);
            }
            else
            {
                Debug.Log(getBalanceRequest.Exception.Message);
                callback.Invoke(false);
            }
        }

        public static IEnumerator AssignHercTokens(int amount, Action<int> callback)
        {
            //var assignTokenRequest = new EthCallUnityRequest(EthereumEndpoint);
            //var contract = new Contract(null, AssignTokenContractAbi, AssignTokenContractAddress);
            //var function = contract.GetFunction("transferFrom");

            //yield return assignTokenRequest.SendRequest(function.CreateCallInput(AssignTokenContractAddress, Account.Address), BlockParameter.CreateLatest());

            //var result = function.DecodeSimpleTypeOutput<bool>(assignTokenRequest.Result);


            var assignTokenRequest = new TransactionSignedUnityRequest(EthereumEndpoint, Account.PrivateKey, Account.Address);
            var contract = new Contract(null, AssignTokenContractAbi, AssignTokenContractAddress);
            var function = contract.GetFunction("transferFrom");
            
            yield return assignTokenRequest.SignAndSendTransaction(function.CreateTransactionInput(Account.Address, new HexBigInteger(280000), new HexBigInteger(41000000000), new HexBigInteger(0), amount));
            
            if (assignTokenRequest.Exception == null)
            {
                Debug.Log("Transaction sent: " + assignTokenRequest.Result);
                yield return CheckTransactionReceiptIsMined(EthereumEndpoint, assignTokenRequest.Result, (cb) => { if (cb) callback.Invoke(amount); });
            }
            else
            {
                Debug.Log("Something gone wrong in your transaction request! " + assignTokenRequest.Exception.Message);
            }
        }

        public static IEnumerator GetTopScores(Action<List<TopScore>> callback)
        {
            var playerScoreRequest = new EthCallUnityRequest(EthereumEndpoint);
            var contract = new Contract(null, PlayerScoreContractAbi, PlayerScoreContractAddress);
            var countFunction = contract.GetFunction("GetTopScoresCount");
            var topScoresFunction = contract.GetFunction("TopScores");

            yield return playerScoreRequest.SendRequest(countFunction.CreateCallInput(), BlockParameter.CreateLatest());

            var count = countFunction.DecodeSimpleTypeOutput<uint>(playerScoreRequest.Result);

            var scores = new List<TopScore>((int) count);
            for (int i = 0; i < count; i++)
            {
                yield return playerScoreRequest.SendRequest(topScoresFunction.CreateCallInput(i),
                    BlockParameter.CreateLatest());
                var score = topScoresFunction.DecodeDTOTypeOutput<TopScore>(playerScoreRequest.Result);
                scores.Add(score);
            }

            callback.Invoke(scores.OrderByDescending(x => x.Score).ToList());
        }

        public static IEnumerator PushScore(int score, Action<int> callback)
        {
            var playerScoreRequest = new TransactionSignedUnityRequest(EthereumEndpoint, Account.PrivateKey, Account.Address);
            var contract = new Contract(null, PlayerScoreContractAbi, PlayerScoreContractAddress);
            var function = contract.GetFunction("SetScore");

            Debug.Log("Pending transaction...");
            yield return playerScoreRequest.SignAndSendTransaction(function.CreateTransactionInput(Account.Address,
                new HexBigInteger(280000), new HexBigInteger(41000000000), new HexBigInteger(0), score));

            if (playerScoreRequest.Exception == null)
            {
                Debug.Log("Transaction sent: " + playerScoreRequest.Result);

                yield return CheckTransactionReceiptIsMined(EthereumEndpoint, playerScoreRequest.Result, (cb) => { if(cb) callback.Invoke(score); });
            }
            else
            {
                Debug.Log("Something gone wrong in your transaction request! " + playerScoreRequest.Exception.Message);
            }
        }

        public static IEnumerator CheckTransactionReceiptIsMined(string url, string txHash, Action<bool> callback)
        {
            var mined = false;
            var tries = 999;

            while (!mined)
            {
                if (tries > 0)
                {
                    tries = tries - 1;
                }
                else
                {
                    mined = true;
                    Debug.Log("Performing last try..");
                }

                Debug.Log("Checking receipt for: " + txHash);
                var receiptRequest = new EthGetTransactionReceiptUnityRequest(url);
                yield return receiptRequest.SendRequest(txHash);
                if (receiptRequest.Exception == null)
                {
                    if (receiptRequest.Result != null)
                    {
                        if(receiptRequest.Result.Status.Value.IsOne)
                        {
                            mined = true;
                            callback(mined);
                        }
                        else
                        {
                            Debug.Log("Did not mined yet.");
                        }
                    }
                    else
                    {
                        Debug.Log("Did not mined yet. Hash: " + txHash);
                    }
                }
                else
                {
                    // If we had an error doing the request
                    Debug.Log("Error checking receipt: " + receiptRequest.Exception.Message);
                }

                yield return new WaitForSeconds(5);
            }
        }

        public static IEnumerator GetPuzzleData(uint id, Action<string, string> callback)
        {
            var puzzleManagerRequest = new EthCallUnityRequest(EthereumEndpoint);
            var contract = new Contract(null, PuzzleManagerContractAbi, PuzzleManagerContractAddress);
            var getMetricsFunction = contract.GetFunction("GetPuzzleOriginalMetrics");

            yield return puzzleManagerRequest.SendRequest(getMetricsFunction.CreateCallInput(id), BlockParameter.CreateLatest());

            var metrics = getMetricsFunction.DecodeSimpleTypeOutput<byte[]>(puzzleManagerRequest.Result);

            var original = new byte[32];
            var current = new byte[32];

            Buffer.BlockCopy(metrics, 0, original, 0, 32);
            Buffer.BlockCopy(metrics, 31, current, 0, 32);

            callback.Invoke(BitConverter.ToString(original), BitConverter.ToString(current));
        }


    }
}
