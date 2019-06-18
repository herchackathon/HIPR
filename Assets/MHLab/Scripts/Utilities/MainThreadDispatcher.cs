using System;
using System.Collections.Generic;
using UnityEngine;

namespace MHLab.Utilities
{
    public class MainThreadDispatcher : MonoBehaviour
    {
        private static readonly Queue<Action> Actions = new Queue<Action>();
        private static readonly Queue<Action> NextFrameActions = new Queue<Action>();

        // This is a static reference to the current instance of 
        // the MainThreadDispatcher that we have in our scene  
        public static MainThreadDispatcher instance;

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

        protected void Update()
        {
            while (Actions.Count > 0)
            {
                var action = Actions.Dequeue();
                action.Invoke();
            }

            while (NextFrameActions.Count > 0)
            {
                Actions.Enqueue(NextFrameActions.Dequeue());
            }
        }

        public static void EnqueueAction(Action callback)
        {
            Actions.Enqueue(callback);
        }

        public static void EnqueueActionForNextFrame(Action callback)
        {
            NextFrameActions.Enqueue(callback);
        }
    }
}
