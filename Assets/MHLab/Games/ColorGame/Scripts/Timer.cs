using UnityEngine;
//using TMPro;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    //private TextMeshProUGUI text;
    private Text text;

    static float time = 5f;
    //static float extraTime = 10f;

    void Start()
    {
        text = this.gameObject.GetComponent<Text>();
    }

    void Update()
    {
        if (time >= 0.00f && BoardManager.levelComplete == false)
        {
            time -= Time.deltaTime;
            //text.SetText(time.ToString("0") + "s");
            text.text = time.ToString("0") + "s";

        }
        else
        {
            BoardManager.SetGameStatus(true);
            GetComponent<Timer>().enabled = false;
        }


        //if (timer <= 0)
        //{
        //    InGameManager.gameOver = true;
        //}

        //if (InGameManager.levelComplete == false)
        //{
        //    timer -= Time.deltaTime;
        //    timerText.SetText(timer.ToString("0") + "s");
        //}

        //if (InGameManager.levelComplete == true)
        //{
        //    timer += 5f;
        //    this.gameObject.SetActive(false);
        //}

    }

    public static void AddTime(float num)
    {
        time += num;
    }

    public static void SetTime(float num)
    {
        time = num;
    }


    //void Start()
    //{
    //    timer = 0;
    //}


    //void Update()
    //{
    //    if (GameOver.gameOver == false)
    //    {
    //        timer += Time.deltaTime;
    //        seconds = timer;
    //        stopwatchText.SetText(seconds.ToString("0") + "s");
    //    }
    //}


}
