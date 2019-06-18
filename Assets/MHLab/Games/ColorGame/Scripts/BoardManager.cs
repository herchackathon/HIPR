using UnityEngine;
using Random = System.Random;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class BoardManager : MonoBehaviour
{

    public Color[] colors = new Color[19];
    public Material[] materials = new Material[5];

    // The max height a cube can go before the upward force is no longer applied 
    int maxHeight = 5;

    // The strength of upwards force applied to a cube when clicked 
    float upForce = 100f;

    // The puzzle's difficulty setting - how much is the board mixed
    public static int level = 1;

    // The size of the board / The X and Y axis sizes for the board 
    public static int xAxisLength = 5; // x-axis (horizontal) 
    public static int yAxisLength = 5; // y-axis (vertical) 

    //int colourSize = 5;
    //int colourListSize = 19;

    //
    public GameObject completeSign;
    public GameObject gameOverSign;
    public GameObject failureSign;
    //public GameObject restartButton;
    //public GameObject exitButton;

    public static bool levelComplete;

    public static bool gameOver;

    int levelCompleteScore;
    //

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
        //need to add code here to shuffle the color scheme 
        materials[0].SetColor("_Color", colorScheme.color1);
        materials[1].SetColor("_Color", colorScheme.color2);
        materials[2].SetColor("_Color", colorScheme.color3);
        materials[3].SetColor("_Color", colorScheme.color4);
        materials[4].SetColor("_Color", colorScheme.color5);

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
            NextLevel();
        }

        // If time is up, game over is true 
        if (gameOver == true)
        {
            GameOver();
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
                    ChangeCubeColours(cubeName);
                }
            }
        }
    }


    void ChangeCubeColours(string objectName)
    {
        // Find the object thats been clicked 
        GameObject cubeClicked = GameObject.Find(objectName);

        Material materialTemp = cubeClicked.GetComponent<Renderer>().material;

        // Split the cube name 
        // Separate the cube name into 3 parts
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

        //if (cubeClicked.GetComponent<Transform>().position.y < maxHeight)
        //{
        //    Rigidbody rb = cubeClicked.GetComponent<Rigidbody>();
        //    rb.AddForce(0, upForce, 0);
        //}

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

        ChangeCubesAround(aboveLetter, char.Parse(cubeNameSplit[2]));
        ChangeCubesAround(belowLetter, char.Parse(cubeNameSplit[2]));
        ChangeCubesAround(char.Parse(cubeNameSplit[1]), leftNumber);
        ChangeCubesAround(char.Parse(cubeNameSplit[1]), rightNumber);
    }


    void ChangeCubesAround(char aboveLetter, char aboveNumber)
    {
        GameObject cube = GameObject.Find("Cube " + aboveLetter + " " + aboveNumber);

        if (cube != null)
        {
            Material materialTemp = cube.GetComponent<Renderer>().material;

            //if (cube.GetComponent<Transform>().position.y < maxHeight)
            //{
            //    Rigidbody rb = cube.GetComponent<Rigidbody>();
            //    rb.AddForce(0, upForce, 0);
            //}

            for (int x = 0; x < materials.Length; x++)
            {
                if (materialTemp.color.Equals(materials[x].color))
                {
                    if (x < 4)
                    {
                        cube.GetComponent<Renderer>().material = materials[x + 1];
                    }
                    else
                    {
                        cube.GetComponent<Renderer>().material = materials[0];
                    }
                }
            }
        }
    }

    void SetBoard()
    {
        // Rows......= A B C D E
        // Columns...= 1 2 3 4 5

        // Temporary game object, just to set each cube's material 
        GameObject temp;

        // Set the first/starting row
        char row = 'A';

        // Loop through the rows and columns and set the material for each object
        // The first IF LOOP goes through the rows
        for (int i = 0; i < yAxisLength; i++)
        {
            // Set the first/starting column 
            int column = 1;

            // Loop through the columns
            for (int x = 0; x < xAxisLength; x++)
            {
                // Find the cube and set its material 
                temp = GameObject.Find("Cube " + row + " " + column);
                temp.GetComponent<Renderer>().material = materials[0];

                // Increment to the next column/number
                // e.g. 1 to 2 to 3...etc. 
                column++;
            }
            // Increment to the next row/letter
            // e.g. A to B to C...etc. 
            row++;
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
            ChangeCubeColours(cubeNamesToMix);
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
        GameObject cubeCheck;

        GameObject goal1 = GameObject.Find("Wall Top");

        string materialNameGoal = goal1.GetComponent<Renderer>().material.name;

        int playerScore = 0;

        // Loop through the rows and columns and set the material
        char rowLetter = 'A';

        for (int i = 0; i < BoardManager.yAxisLength; i++)
        {
            int columnNumber = 1;
            // Loop through columns/numbers
            for (int x = 0; x < BoardManager.xAxisLength; x++)
            {
                // Returns the Cube GameObject of name "***"
                cubeCheck = GameObject.Find("Cube " + rowLetter + " " + columnNumber);

                //Name of the material 
                string materialName = cubeCheck.GetComponent<Renderer>().material.name;

                if (materialName.Equals(materialNameGoal))
                {
                    playerScore++;
                }

                // Increment the column (number) e.g. A to B to C...etc. 
                columnNumber++;
            }
            // Increment the row (letter) e.g. A to B to C...etc. 
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


    public void NextLevel()
    {
        // make the complete sign appear
        completeSign.SetActive(true);
        // play a sound
        FindObjectOfType<AudioManager>().Play("NextLevel");
        // adjust level
        level += 1;
        //add time
        int moreT = (int)(level * 1.5f) + 7;
        Timer.AddTime(moreT);
        // go to next level, but leave a delay for sound 
        Invoke("LoadNextLevel", 1f);
        this.gameObject.SetActive(false);
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameOver()
    {
        failureSign.SetActive(true);
        FindObjectOfType<AudioManager>().Play("PopUp");
        Invoke("DisplayGameOverSign", 1f);
        level = 1;
        this.gameObject.SetActive(false);
    }

    public void DisplayGameOverSign()
    {
        gameOverSign.SetActive(true);
        FindObjectOfType<AudioManager>().Play("GameOver");
    }


    public static void SetGameStatus(bool trueOrFalse)
    {
        gameOver = trueOrFalse;
    }


    public void Exit()
    {
        //Destroy(SceneManager.GetActiveScene());
        //add code here to restart all static variables
        Reset();
        //SceneManager.LoadScene("ColorGameStart");
        SceneManager.LoadScene(1);

    }

    public void Reset()
    {
        level = 1;
        Clicker.SetClicker(0);
        Timer.SetTime(5);
    }

    public void Refresh()
    {
        Reset();
        SceneManager.LoadScene("Board 5x5");
        //Start();
    }


}//end class