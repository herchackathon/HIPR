using UnityEngine;
using Random = System.Random;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class BoardManager : MonoBehaviour
{

    ////public Color[] colors = new Color[19];
    public Material[] materials = new Material[5];

    // The max height a cube can go before the upward force is no longer applied 
    private int maxHeight = 4;

    // The strength of upwards force applied to a cube when clicked 
    private float upForce = 100f;

    // The puzzle's difficulty setting - how much is the board mixed
    private static int level = 1;

    private int levelCompleteScore;

    // The size of the board / The X and Y axis sizes for the board 
    private static int xAxisLength = 5; // x-axis (horizontal) 
    private static int yAxisLength = 5; // y-axis (vertical) 

    public GameObject completeSign;
    public GameObject gameOverSign;
    public GameObject failureSign;
    public GameObject board5x5;
    public GameObject board7x7;
    public GameObject timerScript;
    public GameObject clickerScript;

    public GameObject passNormal;
    public GameObject passHighlight;
    public static int passCount;

    public static bool levelComplete;
    public static bool gameOver;


    void Start()
    {
        // Starting Level Settings
        levelCompleteScore = (yAxisLength * xAxisLength);
        levelComplete = false;
        gameOver = false;

        // Set the colors for the board 
        ColorScheme colorScheme = new ColorScheme();
        int num = GetRandomNumber(0, FindObjectOfType<ColorSchemeManager>().colorSchemes.Length);
        colorScheme = FindObjectOfType<ColorSchemeManager>().colorSchemes[num];

        materials[0].SetColor("_Color", colorScheme.color1);
        materials[1].SetColor("_Color", colorScheme.color2);
        materials[2].SetColor("_Color", colorScheme.color3);
        materials[3].SetColor("_Color", colorScheme.color4);
        materials[4].SetColor("_Color", colorScheme.color5);

        // Shuffle the materials
        Random rand = new Random();
        int n = materials.Length;
        while (n > 1)
        {
            n--;
            int k = GetRandomNumber(0, n + 1);
            Color temp = materials[k].color;
            materials[k].color = materials[n].color;
            materials[n].color = temp;
        }

        // Set the cube starting colours and goal colours 
        SetBoard();

        // Set the difficulty/level 
        MixBoard();
    }

    void Update()
    {
        // Check if the level is complete
        if (CheckBoard())
        {
            NextLevelStep1();
        }

        // If time is up, game over == true 
        if (gameOver)
        {
            GameOver();
        }

        if (passCount > 0 && passHighlight.active == true)
        {
            PassSolve();
            return;
        }

        // If the left button is pressed
        if (Input.GetMouseButtonDown(0) && levelComplete == false && gameOver == false)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if (hit.transform.gameObject.tag == "Cube")
                {
                    //Clicker temp = new Clicker();
                    //temp.UpdateClicker();
                    string cubeName = hit.transform.gameObject.name;
                    FindObjectOfType<AudioManager>().Play("Click");
                    ChangeCubeColours(cubeName, false);
                }
            }
        }
    }


    void ChangeCubeColours(string objectName, bool pass)
    {
        // Find the object thats been clicked 
        GameObject cubeClicked = GameObject.Find(objectName);
        Material materialTemp = cubeClicked.GetComponent<Renderer>().material;

        // Split the cube's name into 3 parts & increment to find cubes around
        // Part 1 being the object name/type "Cube"
        // Part 2 being the object's row 'A', index will be [1]
        // Part 3 being the object's column '1', index will be [2]
        string[] cubeNameSplit = objectName.Split(' ');
        char aboveLetter = char.Parse(cubeNameSplit[1]);
        char belowLetter = char.Parse(cubeNameSplit[1]);
        char leftNumber = char.Parse(cubeNameSplit[2]);
        char rightNumber = char.Parse(cubeNameSplit[2]);
        --aboveLetter;
        ++belowLetter;
        --leftNumber;
        ++rightNumber;

        if (cubeClicked.GetComponent<Transform>().position.y < maxHeight)
        {
            Rigidbody rb = cubeClicked.GetComponent<Rigidbody>();
            rb.AddForce(0, upForce, 0);
        }

        for (int x = 0; x < materials.Length; x++)
        {
            if (materialTemp.color.Equals(materials[x].color))
            {
                if (x < 4)
                {
                    cubeClicked.GetComponent<Renderer>().material = materials[x + 1];
                }
                else
                {
                    cubeClicked.GetComponent<Renderer>().material = materials[0];
                }
            }
        }


        ChangeCubesAround(aboveLetter, char.Parse(cubeNameSplit[2]), pass);
        ChangeCubesAround(belowLetter, char.Parse(cubeNameSplit[2]), pass);
        ChangeCubesAround(char.Parse(cubeNameSplit[1]), leftNumber, pass);
        ChangeCubesAround(char.Parse(cubeNameSplit[1]), rightNumber, pass);


        if (pass)
        {
            cubeClicked.GetComponent<Renderer>().material = materials[0];
            return;
        }

    }


    void ChangeCubesAround(char aboveLetter, char aboveNumber, bool pass)
    {
        GameObject cubeTemp = GameObject.Find("Cube " + aboveLetter + " " + aboveNumber);

        if (cubeTemp != null)
        {

            if (pass)
            {
                cubeTemp.GetComponent<Renderer>().material = materials[0];
                return;
            }

            Material materialTemp = cubeTemp.GetComponent<Renderer>().material;

            if (cubeTemp.GetComponent<Transform>().position.y < maxHeight)
            {
                Rigidbody rb = cubeTemp.GetComponent<Rigidbody>();
                rb.AddForce(0, upForce, 0);
            }

            for (int x = 0; x < materials.Length; x++)
            {
                if (materialTemp.color.Equals(materials[x].color))
                {
                    if (x < 4)
                    {
                        cubeTemp.GetComponent<Renderer>().material = materials[x + 1];
                    }
                    else
                    {
                        cubeTemp.GetComponent<Renderer>().material = materials[0];
                    }
                }
            }
        }
    }

    void SetBoard()
    {
        // Rows...... = A B C D E ... F G
        // Columns... = 1 2 3 4 5 ... 6 7 
        GameObject temp;

        char row = 'A';  // Set the first/starting row

        // Loop through the rows and columns and set the material for each object
        // The first IF LOOP goes through the rows
        for (int i = 0; i < yAxisLength; i++)
        {
            int column = 1; // Set the first/starting column 

            // Loop through the columns
            for (int x = 0; x < xAxisLength; x++)
            {
                // Find the cube and set its material 
                temp = GameObject.Find("Cube " + row + " " + column);
                temp.GetComponent<Renderer>().material = materials[0];

                column++; // Increment to the next column/number, e.g. 1 to 2 to 3...etc. 
            }
            row++; // Increment to the next row/letter, e.g. A to B to C...etc. 
        }

        // Set the goals (which are also the walls)
        GameObject.Find("Wall Top").GetComponent<Renderer>().material = materials[0];
        GameObject.Find("Wall Bottom").GetComponent<Renderer>().material = materials[0];
        GameObject.Find("Wall Left").GetComponent<Renderer>().material = materials[0];
        GameObject.Find("Wall Right").GetComponent<Renderer>().material = materials[0];

        GameObject.Find("Key1").GetComponent<Renderer>().material = materials[0];
        GameObject.Find("Key2").GetComponent<Renderer>().material = materials[1];
        GameObject.Find("Key3").GetComponent<Renderer>().material = materials[2];
        GameObject.Find("Key4").GetComponent<Renderer>().material = materials[3];
        GameObject.Find("Key5").GetComponent<Renderer>().material = materials[4];
    }


    void MixBoard()
    {
        for (int i = 0; i < level; i++)
        {
            char[] letters = { 'A', 'B', 'C', 'D', 'E', 'F', 'G' };
            int row = GetRandomNumber(0, yAxisLength);
            int column = GetRandomNumber(1, (xAxisLength + 1));
            string cubeNamesToMix = "Cube " + letters[row] + " " + column;
            ChangeCubeColours(cubeNamesToMix, false);
        }
    }

    //Function to get random number
    private static readonly Random getrandom = new Random();
    public static int GetRandomNumber(int min, int max)
    {
        lock (getrandom) // synchronize
        {
            return getrandom.Next(min, max);
        }
    }


    public bool CheckBoard()
    {
        GameObject cube;
        GameObject goal = GameObject.Find("Wall Top");

        string materialNameGoal = goal.GetComponent<Renderer>().material.name;

        int playerScore = 0;

        // Loop through the rows and columns and check the materials/colors
        char rowLetter = 'A';
        for (int i = 0; i < yAxisLength; i++)
        {
            int columnNumber = 1;

            for (int x = 0; x < xAxisLength; x++)
            {
                cube = GameObject.Find("Cube " + rowLetter + " " + columnNumber);
                string materialName = cube.GetComponent<Renderer>().material.name;

                if (materialName.Equals(materialNameGoal))
                {
                    playerScore++;
                }
                columnNumber++;
            }
            rowLetter++;
        }

        if (playerScore == levelCompleteScore)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public void NextLevelStep1()
    {
        completeSign.SetActive(true);
        FindObjectOfType<AudioManager>().Play("NextLevel");

        level += 1;

        int time = (level * 2) + 7;
        Timer.AddTime(time);

        Invoke("NextLevelStep2", 1f); // go to next level, but leave a delay for sound 

        this.gameObject.SetActive(false);
    }

    public void NextLevelStep2()
    {
        completeSign.SetActive(false);

        if (level == 10)
        {
            board5x5.SetActive(false);
            board7x7.SetActive(true);
            xAxisLength = 7;
            yAxisLength = 7;
        }

        if ((level % 3) == 0)
        {
            IncrementPassCount();
            passNormal.SetActive(true);
            passHighlight.SetActive(false);
        }

        Start();
        this.gameObject.SetActive(true);
    }


    public static void SetGameOver(bool trueOrFalse)
    {
        gameOver = trueOrFalse;
    }


    public void GameOver()
    {
        failureSign.SetActive(true);
        FindObjectOfType<AudioManager>().Play("PopUp");

        Invoke("DisplayGameOverSign", 1f);

        this.gameObject.SetActive(false);
    }

    public void DisplayGameOverSign()
    {
        gameOverSign.SetActive(true);
        FindObjectOfType<AudioManager>().Play("GameOver");
    }

    public void Refresh()
    {
        level = 1;
        xAxisLength = 5;
        yAxisLength = 5;
        passCount = 0;

        board5x5.SetActive(true);
        board7x7.SetActive(false);

        gameOverSign.SetActive(false);
        failureSign.SetActive(false);
        completeSign.SetActive(false);

        Clicker.SetClicker(0);
        Timer.SetTime(5);

        Start();
        this.gameObject.SetActive(true);
    }


    public void Exit()
    {
        Refresh();
        SceneManager.LoadScene(1);
    }


    public void IncrementPassCount()
    {
        passCount++;

        passNormal.GetComponentInChildren<Text>().text = "PASS:" + passCount.ToString("0");
        passHighlight.GetComponentInChildren<Text>().text = "PASS:" + passCount.ToString("0");
        //passText1.GetComponent<Text>().text = "PASS:" + passCount.ToString("0");
        //passText2.GetComponent<Text>().text = "PASS:" + passCount.ToString("0");
    }

    public void DecrementPassCount()
    {
        passCount--;
        passNormal.GetComponentInChildren<Text>().text = "PASS:" + passCount.ToString("0");
        passHighlight.GetComponentInChildren<Text>().text = "PASS:" + passCount.ToString("0");
    }

    public void PassSolve()
    {
        if (Input.GetMouseButtonDown(0) && levelComplete == false && gameOver == false)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if (hit.transform.gameObject.tag == "Cube")
                {
                    string cubeName = hit.transform.gameObject.name;
                    FindObjectOfType<AudioManager>().Play("Click");
                    ChangeCubeColours(cubeName, true);

                    DecrementPassCount();

                    if (passCount <= 0)
                    {
                        passNormal.SetActive(false);
                        passHighlight.SetActive(false);
                    }
                    else
                    {
                        passNormal.SetActive(true);
                        passHighlight.SetActive(false);
                    }

                }
            }
        }
    }


}//end class