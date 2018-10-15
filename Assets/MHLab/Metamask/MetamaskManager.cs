using System.Runtime.InteropServices;

namespace MHLab.Metamask
{
    public class MetamaskManager
    {
        [DllImport("__Internal")]
        public static extern string SendTransaction(string to, string data);

        [DllImport("__Internal")]
        public static extern string GetAccount();
    }
}