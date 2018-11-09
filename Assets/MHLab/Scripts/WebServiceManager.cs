using MHLab.Ethereum;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace MHLab
{
	public class WebServiceManager
    {
        private const string Endpoint = "";
        ///GET api/1.0/getTopScores (index=0, count=1) => {topScores::array[]}
        private const string GetTopScoresAddress = Endpoint + "/api/1.0/getTopScores";
        ///POST api/1.0/setScore (score=42) => {result::web3.transactionObject}
        private const string SetTopScoreAddress = Endpoint + "/api/1.0/setScore";
        ///POST api/1.0/createPuzzle (metrics="metrics007") => {puzzleId:int}

        public static IEnumerator GetTopScores(Action<List<TopScore>> callback)
        {
            using (var www = UnityWebRequest.Get(GetTopScoresAddress))
            {
                yield return www.SendWebRequest();

                if (www.isHttpError || www.isNetworkError)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    var result = JsonUtility.FromJson<List<TopScore>>(www.downloadHandler.text);
                    callback.Invoke(result);
                }
            }
        }

        public static IEnumerator SetTopScore(int score, Action<int> callback)
        {
            using (var www = UnityWebRequest.Post(SetTopScoreAddress, score.ToString()))
            {
                yield return www.SendWebRequest();

                if (www.isHttpError || www.isNetworkError)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    //var result = JsonUtility.FromJson<List<TopScore>>(www.downloadHandler.text);
                    callback.Invoke(score);
                }
            }
        }
    }
}
