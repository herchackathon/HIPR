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
#if UNITY_EDITOR
            Address = "0xf5ac78a87ac787ca87c87a";
#else
            Address = MetamaskManager.GetAccount();
#endif
            MainThreadDispatcher.EnqueueAction(() => callback(Address));
        }
    }
}
