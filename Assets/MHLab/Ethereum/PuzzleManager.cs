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
            /*Task.Factory.StartNew(() =>
            {*/
	            var hash = "asdniajdiasjdsajidjaicnncavnoajvdaojvaoi";
/*#if !UNITY_EDITOR && !TEST
                try
                {
					hash = string.Empty;
                    MetamaskManager.GetPuzzle();
					do
	                {
		                hash = MetamaskManager.GetResults("GetPuzzle");
	                } while (hash == string.Empty);
                }
                catch(Exception e)
                {
                    MainThreadDispatcher.EnqueueAction(() =>
                    {
                        errorCallback.Invoke(e);
                    });
                }
#endif*/
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
            //});
        }

        public static void ValidatePuzzleResult(string hash, Action<bool> callback)
        {
            /*Task.Factory.StartNew(() =>
            {*/
/*#if UNITY_EDITOR
                var result = true;
#else
                MetamaskManager.ValidatePuzzleResult(hash);
				var tmp = string.Empty;
				do
	            {
		            tmp = MetamaskManager.GetResults("GetPuzzle");
	            } while (tmp == string.Empty);

				var result = bool.Parse(tmp);
#endif*/

				CurrentHash = null;

                MainThreadDispatcher.EnqueueAction(() =>
                {
                    callback.Invoke(true);
                });
            //});
        }
    }
}
