﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if (hit.transform.gameObject.tag == "Done Button")
                {
                    Invoke("Restart", 1f);
                }

            }

        }
    }

    public void Restart()
    {
        //BoardManager.level += 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
