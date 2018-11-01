using System;
using System.Collections.Generic;
using UnityEngine;

namespace MHLab.Utilities
{
    public class MainThreadDispatcher : MonoBehaviour
    {
        private static readonly Queue<Action> Actions = new Queue<Action>();
		private static readonly Queue<Action> NextFrameActions = new Queue<Action>();

        protected void Awake()
        {
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
