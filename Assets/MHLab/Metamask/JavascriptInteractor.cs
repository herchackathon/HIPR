using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MHLab.Metamask
{
	public class JavascriptInteractor : MonoBehaviour
	{
		public static Dictionary<string, Action<string>> Actions = new Dictionary<string, Action<string>>();

		protected void Awake()
		{
			DontDestroyOnLoad(this);
		}

		public void ProcessResult(string result)
		{
			var tmp = result.Split('#');

			Actions[tmp[0]].Invoke(tmp[1]);
		}

		public static void ProcessResultGlobal(string result)
		{
			var tmp = result.Split('#');

			Actions[tmp[0]].Invoke(tmp[1]);
		}
	}
}
