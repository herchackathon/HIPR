using HIPR.Encoding;
using MHLab;
using MHLab.SlidingTilePuzzle;
using MHLab.SlidingTilePuzzle.Data;
using MHLab.SlidingTilePuzzle.Leaderboards;
using MHLab.UI;
using MHLab.Web.Storage;
using System.Collections;
using System.Collections.Generic;
using MHLab.Ethereum;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class ST_PuzzleDisplay : MonoBehaviour
{
    public static int PuzzleMoves = 0;
    public static bool CanMove = false;
    public static bool CanCount = false;
    public static ST_PuzzleDisplay Instance;

    public static string OriginalHash;
    public static string CurrentHash;

	// this puzzle texture.
    public Texture2D[] PuzzleImages;
	public Texture2D PuzzleImage;
    public RectTransform CompletingPopup;
    public Text CompletingText;
    public RectTransform CurrentCanvas;
    public EnableForLimitedTime ShufflingPopup;
    public EnableForLimitedTime LetsgoPopup;

    public AudioClip VictorySound;
    public AudioClip OnMoveSound;

	// the width and height of the puzzle in tiles.
	public int Height = 3;
	public int Width  = 3;

	// additional scaling value.
	public Vector3 PuzzleScale = new Vector3(1.0f, 1.0f, 1.0f);

	// additional positioning offset.
	public Vector3 PuzzlePosition = new Vector3(0.0f, 0.0f, 0.0f);

	// seperation value between puzzle tiles.
	public float SeperationBetweenTiles = 0.5f;

	// the tile display object.
	public GameObject Tile;

	// the shader used to render the puzzle.
	public Shader PuzzleShader;

	// array of the spawned tiles.
	private GameObject[,] TileDisplayArray;
	private List<Vector3>  DisplayPositions = new List<Vector3>();

	// position and scale values.
	private Vector3 Scale;
	private Vector3 Position;

	// has the puzzle been completed?
	public bool Complete = false;

    public AudioSource AudioSource;

    public static readonly List<Vector2> Moves = new List<Vector2>();

	// Use this for initialization
	void Start()
	{
	    ST_PuzzleDisplay.PuzzleMoves = 0;
	    ST_PuzzleDisplay.CanMove = false;
	    ST_PuzzleDisplay.CanCount = false;
        ST_PuzzleDisplay.Moves.Clear();
        //Texture2D encryptImg = PuzzleImage as Texture2D;
        Instance = this;
	    AudioSource = GetComponent<AudioSource>();

	    PuzzleImage = PuzzleImages[UnityEngine.Random.Range(0, PuzzleImages.Length)];
        PuzzleImage = Steganography.Encode(PuzzleImage, PuzzleManager.CurrentHash);

        // create the games puzzle tiles from the provided image.
        CreatePuzzleTiles();

		// mix up the puzzle.
		StartCoroutine(JugglePuzzle());
	}
	
	// Update is called once per frame
	void Update() 
	{
		// move the puzzle to the position set in the inspector.
		//this.transform.localPosition = PuzzlePosition;
        PositionInView();

	    /*if (Input.GetKeyUp(KeyCode.S))
	    {
	        Complete = true;
	    }*/
    }

    public Vector2 padding = new Vector2(0.45f, 0.1f); //Distance we want to keep from the viewport borders.

    private void PositionInView()
    {
        const float DISTANCE_FROM_CAM = 50;

        if (CurrentCanvas.rect.width < CurrentCanvas.rect.height)
        {
            //Calculate the max width the object is allowed to have in world space, based on the padding we decided.
            float maxWidth = Vector3.Distance(
                Camera.main.ViewportToWorldPoint(new Vector3(padding.x, 0.5f, DISTANCE_FROM_CAM)),
                Camera.main.ViewportToWorldPoint(new Vector3(1f - padding.x, 0.5f, DISTANCE_FROM_CAM)));
            //Calculate the scale based on width only - you will have to check if the model is tall instead of wide and check against the aspect of the camera, and act accordingly.
            float scale = (maxWidth / (Width * Tile.transform.localScale.x));
            //Apply the scale to the model.
            transform.localScale = Vector3.one * scale;

            //Position the model at the desired distance.
            Vector3 desiredPosition =
                DISTANCE_FROM_CAM * Camera.main.transform.forward + Camera.main.transform.position;
            //The max width we calculated is for the entirety of the model in the viewport, so we need to position it so the front of the model is at the desired distance, not the center.
            //You will also have to keep rotation of the camera and the model in mind.
            transform.localPosition =
                desiredPosition + new Vector3(0, 0, (Height * Tile.transform.localScale.z) * scale);
        }
        else
        {
            //Calculate the max width the object is allowed to have in world space, based on the padding we decided.
            float maxWidth = Vector3.Distance(
                Camera.main.ViewportToWorldPoint(new Vector3(0.5f, padding.x, DISTANCE_FROM_CAM)),
                Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1f - padding.x, DISTANCE_FROM_CAM)));
            //Calculate the scale based on width only - you will have to check if the model is tall instead of wide and check against the aspect of the camera, and act accordingly.
            float scale = (maxWidth / (Width * Tile.transform.localScale.x));
            //Apply the scale to the model.
            transform.localScale = Vector3.one * scale;

            //Position the model at the desired distance.
            Vector3 desiredPosition =
                DISTANCE_FROM_CAM * Camera.main.transform.forward + Camera.main.transform.position;
            //The max width we calculated is for the entirety of the model in the viewport, so we need to position it so the front of the model is at the desired distance, not the center.
            //You will also have to keep rotation of the camera and the model in mind.
            transform.localPosition =
                desiredPosition + new Vector3(0, 0, (Height * Tile.transform.localScale.z) * scale);
        }
    }

    public Vector3 GetTargetLocation(ST_PuzzleTile thisTile)
	{
		// check if we can move this tile and get the position we can move to.
		ST_PuzzleTile MoveTo = CheckIfWeCanMove((int)thisTile.GridLocation.x, (int)thisTile.GridLocation.y, thisTile);

		if(MoveTo != thisTile)
		{
			// get the target position for this new tile.
			Vector3 TargetPos = MoveTo.TargetPosition;
			Vector2 GridLocation = thisTile.GridLocation;
			thisTile.GridLocation = MoveTo.GridLocation;

			// move the empty tile into this tiles current position.
			MoveTo.LaunchPositionCoroutine(thisTile.TargetPosition);
			MoveTo.GridLocation = GridLocation;

			// return the new target position.
			return TargetPos;
		}

		// else return the tiles actual position (no movement).
		return thisTile.TargetPosition;
	}

	private ST_PuzzleTile CheckMoveLeft(int Xpos, int Ypos, ST_PuzzleTile thisTile)
	{
		// move left 
		if((Xpos - 1)  >= 0)
		{
			// we can move left, is the space currently being used?
			return GetTileAtThisGridLocation(Xpos - 1, Ypos, thisTile);
		}
		
		return thisTile;
	}
	
	private ST_PuzzleTile CheckMoveRight(int Xpos, int Ypos, ST_PuzzleTile thisTile)
	{
		// move right 
		if((Xpos + 1)  < Width)
		{
			// we can move right, is the space currently being used?
			return GetTileAtThisGridLocation(Xpos + 1, Ypos , thisTile);
		}
		
		return thisTile;
	}
	
	private ST_PuzzleTile CheckMoveDown(int Xpos, int Ypos, ST_PuzzleTile thisTile)
	{
		// move down 
		if((Ypos - 1)  >= 0)
		{
			// we can move down, is the space currently being used?
			return GetTileAtThisGridLocation(Xpos, Ypos  - 1, thisTile);
		}
		
		return thisTile;
	}
	
	private ST_PuzzleTile CheckMoveUp(int Xpos, int Ypos, ST_PuzzleTile thisTile)
	{
		// move up 
		if((Ypos + 1)  < Height)
		{
			// we can move up, is the space currently being used?
			return GetTileAtThisGridLocation(Xpos, Ypos  + 1, thisTile);
		}
		
		return thisTile;
	}
	
	private ST_PuzzleTile CheckIfWeCanMove(int Xpos, int Ypos, ST_PuzzleTile thisTile)
	{
		// check each movement direction
		if(CheckMoveLeft(Xpos, Ypos, thisTile) != thisTile)
		{
			return CheckMoveLeft(Xpos, Ypos, thisTile);
		}
		
		if(CheckMoveRight(Xpos, Ypos, thisTile) != thisTile)
		{
			return CheckMoveRight(Xpos, Ypos, thisTile);
		}
		
		if(CheckMoveDown(Xpos, Ypos, thisTile) != thisTile)
		{
			return CheckMoveDown(Xpos, Ypos, thisTile);
		}
		
		if(CheckMoveUp(Xpos, Ypos, thisTile) != thisTile)
		{
			return CheckMoveUp(Xpos, Ypos, thisTile);
		}

		return thisTile;
	}

	private ST_PuzzleTile GetTileAtThisGridLocation(int x, int y, ST_PuzzleTile thisTile)
	{
		for(int j = Height - 1; j >= 0; j--)
		{
			for(int i = 0; i < Width; i++)
			{
				// check if this tile has the correct grid display location.
				if((TileDisplayArray[i,j].GetComponent<ST_PuzzleTile>().GridLocation.x == x)&&
				   (TileDisplayArray[i,j].GetComponent<ST_PuzzleTile>().GridLocation.y == y))
				{
					if(TileDisplayArray[i,j].GetComponent<ST_PuzzleTile>().Active == false)
					{
						// return this tile active property. 
						return TileDisplayArray[i,j].GetComponent<ST_PuzzleTile>();
					}
				}
			}
		}

		return thisTile;
	}

	private IEnumerator JugglePuzzle()
	{
	    ShufflingPopup.EnableFor(4);
        yield return new WaitForSeconds(1.0f);

	    int tileToHideX = UnityEngine.Random.Range(0, Width);
	    int tileToHideY = UnityEngine.Random.Range(0, Height);
        // hide a puzzle tile (one is always missing to allow the puzzle movement).
        TileDisplayArray[tileToHideX, tileToHideY].GetComponent<ST_PuzzleTile>().Active = false;

		yield return new WaitForSeconds(1.0f);

		int howManyShuffles = UnityEngine.Random.Range(20, 35);

		for(int k = 0; k < howManyShuffles; k++)
		{
			// use random to position each puzzle section in the array delete the number once the space is filled.
			for(int j = 0; j < Height; j++)
			{
				for(int i = 0; i < Width; i++)
				{		
					// attempt to execute a move for this tile.
					TileDisplayArray[i,j].GetComponent<ST_PuzzleTile>().ExecuteAdditionalMove();

					yield return new WaitForSeconds(0.02f);
				}
			}
		}

		// continually check for the correct answer.
		StartCoroutine(CheckForComplete());

		yield return null;

	    CanMove = true;
	    CanCount = true;
        LetsgoPopup.EnableFor(1);
        GameTimerUpdater.StartTimer();
	}

	public IEnumerator CheckForComplete()
	{
		while(Complete == false)
		{
			// iterate over all the tiles and check if they are in the correct position.
			Complete = true;
			for(int j = Height - 1; j >= 0; j--)
			{
				for(int i = 0; i < Width; i++)
				{
					// check if this tile has the correct grid display location.
					if(TileDisplayArray[i,j].GetComponent<ST_PuzzleTile>().CorrectLocation == false)  
					{
						Complete = false;
					}
				}
			}

			yield return null;
		}
				
		// if we are still complete then all the tiles are correct.
		if(Complete)
		{
            GameTimerUpdater.StopTimer();
            string msg = Steganography.Decode(PuzzleImage);

            PuzzleManager.ValidatePuzzleResult(msg, (isValid) =>
            {
                if (isValid)
                {
					if(AudioSource == null)
						AudioSource = GetComponent<AudioSource>();
					AudioSource.PlayOneShot(VictorySound);

	                var score = CalculateScore(PuzzleMoves, (int) GameTimerUpdater.ElapsedSeconds);

					var amount = LocalStorage.GetInt(StorageKeys.DecryptedAmountKey).Value + 1;
	                CompletingText.text = "Nicely done! You scored " + score +
	                                      "!\nBe sure to accept the transaction, or your score will not be added in the leaderboard!";
                    
                    CompletingPopup.gameObject.SetActive(true);

                    ScoresManager.PushScore(score,
                        (done) =>
                        {
                            if(done)
                                Debug.Log("Score correctly pushed");
                            else
                                Debug.Log("Score has not been pushed.");
                        });
                    LocalStorage.Store(StorageKeys.DecryptedAmountKey, amount);
                }
                else
                {
                    CompletingText.text = "Uhm, it seems you did not decrypted correctly this hash. But you can retry!";

                    CompletingPopup.gameObject.SetActive(true);
                }
            });
		}

		yield return null;
	}

    private int CalculateScore(int moves, int seconds)
    {
        return (int)(1000000 / ((moves * 1.3f) + (seconds * 0.8f)));
    }

	private Vector2 ConvertIndexToGrid(int index)
	{
		int WidthIndex = index;
		int HeightIndex = 0;

		// take the index value and return the grid array location X,Y.
		for(int i = 0; i < Height; i++)
		{
			if(WidthIndex < Width)
			{
				return new Vector2(WidthIndex, HeightIndex);
			}
			else
			{
				WidthIndex -= Width;
				HeightIndex++;
			}
		}

		return new Vector2(WidthIndex, HeightIndex);
	}

	private void CreatePuzzleTiles()
	{
		// using the width and height variables create an array.
		TileDisplayArray = new GameObject[Width,Height];

		// set the scale and position values for this puzzle.
		Scale = new Vector3(1.0f/Width, 1.0f, 1.0f/Height);
		Tile.transform.localScale = Scale;

		// used to count the number of tiles and assign each tile a correct value.
		int TileValue = 0;

		// spawn the tiles into an array.
		for(int j = Height - 1; j >= 0; j--)
		{
			for(int i = 0; i < Width; i++)
			{
				// calculate the position of this tile all centred around Vector3(0.0f, 0.0f, 0.0f).
				Position = new Vector3(((Scale.x * (i + 0.5f))-(Scale.x * (Width/2.0f))) * (10.0f + SeperationBetweenTiles), 
				                       0.0f, 
				                      ((Scale.z * (j + 0.5f))-(Scale.z * (Height/2.0f))) * (10.0f + SeperationBetweenTiles));

				// set this location on the display grid.
				DisplayPositions.Add(Position);

				// spawn the object into play.
				TileDisplayArray[i,j] = Instantiate(Tile, new Vector3(0.0f, 0.0f, 0.0f) , Quaternion.Euler(90.0f, -180.0f, 0.0f)) as GameObject;
				TileDisplayArray[i,j].gameObject.transform.parent = this.transform;

				// set and increment the display number counter.
				ST_PuzzleTile thisTile = TileDisplayArray[i,j].GetComponent<ST_PuzzleTile>();
				thisTile.ArrayLocation = new Vector2(i,j);
				thisTile.GridLocation = new Vector2(i,j);
				thisTile.LaunchPositionCoroutine(Position);
				TileValue++;

				// create a new material using the defined shader.
				Material thisTileMaterial = new Material(PuzzleShader);

				// apply the puzzle image to it.
				thisTileMaterial.mainTexture = PuzzleImage;
					
				// set the offset and tile values for this material.
				thisTileMaterial.mainTextureOffset = new Vector2(1.0f/Width * i, 1.0f/Height * j);
				thisTileMaterial.mainTextureScale  = new Vector2(1.0f/Width, 1.0f/Height);
					
				// assign the new material to this tile for display.
				TileDisplayArray[i,j].GetComponent<Renderer>().material = thisTileMaterial;
			}
		}
	}
}
