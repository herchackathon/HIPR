using System;
using System.Collections.Generic;
using UnityEngine;

namespace MHLab.Metamask
{
    public class JavascriptInteractor : MonoBehaviour
    {
        public static Dictionary<string, Action<string>> Actions = new Dictionary<string, Action<string>>();

        // This is a static reference to the current instance of 
        // the JavascriptInteractor that we have in our scene  
        public static JavascriptInteractor instance;

        protected void Awake()
        {

            // Make sure this is only on instance 
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
                // Leave before anymore code is executed 
                return;
            }

            DontDestroyOnLoad(this);

        }

        public void ProcessResult(string result)
        {
            ProcessResultGlobal(result);
        }

        public static void ProcessResultGlobal(string result)
        {
            var tmp = result.Split('#');

            MetamaskManager.DebugLog("Received interop request. Key: " + tmp[0] + " Value: " + tmp[1]);

            if (Actions.ContainsKey(tmp[0]))
                Actions[tmp[0]].Invoke(tmp[1]);
            else
            {
                MetamaskManager.DebugLog("Key: " + tmp[0] + " cannot be found! Registered keys are:");
                foreach (var action in Actions)
                {
                    MetamaskManager.DebugLog(" - " + action.Key);
                }
            }
        }
    }
}
