using System;
using UnityEngine;
using UnityEngine.UI;

public class DateCountdown : MonoBehaviour
{
	public string TargetDate;
	private DateTime _target;
	private Text _text;
	private float _timer;

    private void Start()
    {
	    _text = GetComponent<Text>();
		_target = DateTime.ParseExact(TargetDate, "dd/MM/yyyy HH:mm", null);
		UpdateText();
    }

	private void UpdateText()
	{
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
}
