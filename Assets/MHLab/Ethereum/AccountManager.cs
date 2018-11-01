using MHLab.Metamask;
using MHLab.Utilities;
using System;

namespace MHLab.Ethereum
{
    public class AccountManager
    {
        public static string Address;
        
        public static void GetAccount(Action<string> callback)
        {
            Address = MetamaskManager.GetAccount();
            MainThreadDispatcher.EnqueueAction(() => callback(Address));
        }
    }
}
