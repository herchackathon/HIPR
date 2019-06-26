using UnityEngine;
//using TMPro;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    //private TextMeshProUGUI text;
    private Text displayText;

    static float time = 500f;
    //static float extraTime = 10f;

    void Start()
    {
        displayText = this.gameObject.GetComponent<Text>();
    }

    void Update()
    {
        if (time >= 0.00f && BoardManager.levelComplete == false)
        {
            time -= Time.deltaTime;
            //text.SetText(time.ToString("0") + "s");
            displayText.text = time.ToString("0") + "s";
        }
        else
        {
            BoardManager.SetGameOver(true);
            //GetComponent<Timer>().enabled = false;
        }
    }

    public static void AddTime(float num)
    {
        time += num;
    }

    public static void SetTime(float num)
    {
        time = num;
    }



}
