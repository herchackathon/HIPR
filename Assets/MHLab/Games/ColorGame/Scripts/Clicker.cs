using UnityEngine;
//using TMPro;
using UnityEngine.UI;

public class Clicker : MonoBehaviour
{

    //private TextMeshProUGUI text;
    public static Text textDisplayed;

    static int clicks = 0;

    void Start()
    {
        textDisplayed = this.gameObject.GetComponent<Text>();
        //text.SetText(clicks.ToString("0"));
        textDisplayed.text = clicks.ToString("0");
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0) && BoardManager.gameOver == false
            && BoardManager.levelComplete == false)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100.0f))
            {

                if (hit.transform.gameObject.tag == "Cube")
                {
                    clicks++;
                    //text.SetText(clicks.ToString("0"));
                    textDisplayed.text = clicks.ToString("0");
                }
            }
        }
    }


    public static void SetClicker(int num)
    {
        clicks = num;
        textDisplayed.text = clicks.ToString("0");
    }


}
