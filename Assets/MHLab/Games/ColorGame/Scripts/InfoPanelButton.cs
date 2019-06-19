using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoPanelButton : MonoBehaviour
{

    static int temp = 0;

    public GameObject infoPanel;

    public void SetInfoPanel()
    {
        if((infoPanel != null) && (temp == 0))
        {
            infoPanel.active = true;
            temp = 1;
        }
        else
        {
            infoPanel.active = false;
            temp = 0;
        }
    }



}
