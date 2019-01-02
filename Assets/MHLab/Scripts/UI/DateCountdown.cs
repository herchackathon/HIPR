using System;
using MHLab.Metamask;
using UnityEngine;
using UnityEngine.UI;

public class DateCountdown : MonoBehaviour
{
	public string TargetDate;
	private DateTime _target;
	private Text _text;
	private float _timer;
    private bool _canUpdate = false;

    private void Start()
    {
	    _text = GetComponent<Text>();

        GetEndOfSeason((result) =>
        {
            try
            {

                _target = DateTimeOffset.FromUnixTimeMilliseconds(result).DateTime;
            }
            catch
            {
                _target = DateTime.ParseExact(TargetDate, "dd/MM/yyyy HH:mm", null);
            }

            _canUpdate = true;
            UpdateText();
        });
    }

	private void UpdateText()
	{
	    if (!_canUpdate) return;
		var diff = _target - DateTime.UtcNow;
		_text.text = string.Format("{0:00}:{1:00}:{2:00}", (int)diff.TotalHours, diff.Minutes, diff.Seconds); ;
	}

    // Update is called once per frame
    private void Update()
    {
	    if (_timer >= 1f)
	    {
			UpdateText();
		    _timer = 0f;
	    }
	    else
	    {
		    _timer += Time.deltaTime;
	    }
    }

    private static void GetEndOfSeason(Action<long> callback)
    {
        if (!JavascriptInteractor.Actions.ContainsKey("GetEndOfSeason"))
            JavascriptInteractor.Actions.Add("GetEndOfSeason", (result) => { ProcessEndOfSeason(result, callback); });
        else
            JavascriptInteractor.Actions["GetEndOfSeason"] = (result) => { ProcessEndOfSeason(result, callback); };
        MetamaskManager.GetEndOfSeason();
    }

    private static void ProcessEndOfSeason(string result, Action<long> callback)
    {
        var time = long.Parse(result);
        callback.Invoke(time);
    }
}
