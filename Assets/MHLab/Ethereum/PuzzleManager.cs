using MHLab.Metamask;
using System;
using System.Threading.Tasks;
using MHLab.Utilities;

namespace MHLab.Ethereum
{
    public class PuzzleManager
    {
        public static string CurrentHash;

        public static void GetPuzzleHash(Action<string> callback, Action<Exception> errorCallback)
        {
            Task.Factory.StartNew(() =>
            {
#if UNITY_EDITOR
                var hash = "asdniajdiasjdsajidjaicnncavnoajvdaojvaoi";
#else
                try
                {
                    var hash = MetamaskManager.GetPuzzle();
                }
                catch(Exception e)
                {
                    MainThreadDispatcher.EnqueueAction(() =>
                    {
                        errorCallback.Invoke(e);
                    });
                }
#endif
                CurrentHash = hash;

                if (string.IsNullOrEmpty(CurrentHash))
                {
                    MainThreadDispatcher.EnqueueAction(() =>
                    {
                        errorCallback.Invoke(new Exception("No hash fetched from the puzzle manager. Retry later."));
                    });
                }
                else
                {
                    MainThreadDispatcher.EnqueueAction(() => { callback.Invoke(hash); });
                }
            });
        }

        public static void ValidatePuzzleResult(string hash, Action<bool> callback)
        {
            Task.Factory.StartNew(() =>
            {
#if UNITY_EDITOR
                var result = true;
#else
                var result = MetamaskManager.ValidatePuzzleResult(hash);
#endif

                CurrentHash = null;

                MainThreadDispatcher.EnqueueAction(() =>
                {
                    callback.Invoke(result);
                });
            });
        }
    }
}
