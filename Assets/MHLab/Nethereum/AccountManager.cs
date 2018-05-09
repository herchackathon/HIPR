using MHLab.SlidingTilePuzzle.Data;
using MHLab.Web.Storage;
using Nethereum.JsonRpc.UnityClient;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Util;
using System;
using System.Collections;
using UnityEngine;

namespace MHLab.Nethereum
{
    public class Account
    {
        public string Address = "";
        public decimal Balance;
    }

    public class AccountManager
    {
        public static Account Account;
        public static string EthereumEndpoint = "https://rinkeby.infura.io";//"/CHs7q12LsOAlHu4D3Kvr";

        public static IEnumerator Login(string accountAddress, Action<bool> callback)
        {
            var getBalanceRequest = new EthGetBalanceUnityRequest(EthereumEndpoint);
            yield return getBalanceRequest.SendRequest(accountAddress, BlockParameter.CreateLatest());

            if (getBalanceRequest.Exception == null)
            {
                var balance = getBalanceRequest.Result.Value;

                var account = new Account
                {
                    Address = accountAddress,
                    Balance = UnitConversion.Convert.FromWei(balance, 18)
                };

                LocalStorage.Store(StorageKeys.AccountAddressKey, accountAddress);

                callback.Invoke(true);
            }
            else
            {
                Debug.Log(getBalanceRequest.Exception.Message);
                callback.Invoke(false);
            }
        }

        public static void AssignHercTokens(int amount)
        {
            // TODO: call the HERC token assignment function.
        }
    }
}
