using UnityEngine;
using UnityEngine.UI;

public class MovesCountUpdater : MonoBehaviour
{
    private Text m_text;

	// Use this for initialization
	protected void Start()
	{
	    m_text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	protected void Update()
	{
	    m_text.text = ST_PuzzleDisplay.PuzzleMoves.ToString();
	}
}
