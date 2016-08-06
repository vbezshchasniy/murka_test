using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DrawHelper : MonoBehaviour {

	public LineRenderer pattern;
	public LineRenderer tail;
	public LineRenderer line;
	
	public GameObject gameOverScr;

	bool drawing=false;
	bool isGameOver=false;
	int tailLenght=0;
	public static int tailLimit = 25;
	public static int anchorsLimit = 6;
	public static float maxTime = 10;

	int pointsCount=0;
	int staticPointsCount=0;
	int patternPointsCount=0;


	float angle1,angle2;

	Vector3[]  tailPoints = new Vector3[tailLimit+1];
	Vector3[]  staticPoints = new Vector3[anchorsLimit];
	Vector3[]  patternPoints = new Vector3[anchorsLimit];

	int score=0;
	float time_left=maxTime;

	Text score_state;
	Text time_state;

	// Use this for initialization
	void Start () {
		//gameOverScr=GameObject.Find ("GameOverScr");
		CreatePattern ();
	}
	
	// Update is called once per frame
	void Update () {


		if (!isGameOver) {
			TimeProceed ();
			SetCurrentScore ();
			if (!drawing && Input.GetMouseButtonDown (0)) {
				SetStateDrawing ();
            }

			if (drawing && Input.GetMouseButtonUp (0)) {
				SetStateIdle ();
				//CreatePattern ();
            }

			if (drawing) {
				DrawProceed ();
            }
		}
		if (time_left < 0) {
			line.SetVertexCount(0);
			pattern.SetVertexCount(0);
			tail.SetVertexCount(0);

			isGameOver=true;
			gameOverScr.SetActive(true);
			Text finalscore = GameObject.Find ("FinalScore").GetComponent<Text>();
			finalscore.text="Final score: "+score;

		}
	}

	public void ResetGame()
	{
		CreatePattern ();
		gameOverScr.SetActive(false);
		isGameOver = false;
		score = 0;
		time_left = maxTime;
	}

	private void SetStateDrawing()
	{
		
		staticPointsCount=0;
		staticPoints[staticPointsCount]=Input.mousePosition;
		staticPoints[staticPointsCount].z=1;
				
		drawing=true;
		Debug.Log ("Started Drawing");
				
		pointsCount=0;
		tailLenght = 0;
		staticPointsCount = 0;
		line.SetVertexCount (pointsCount);
		tail.SetVertexCount(tailLenght);
	}

	private void SetStateIdle()
	{
		drawing=false;
		Debug.Log ("Ended Drawing");
		Debug.Log (pointsCount+"Ended");

		staticPointsCount = 0;
		pointsCount=0;
		tailLenght = 0;
		line.SetVertexCount (pointsCount);
		tail.SetVertexCount(tailLenght);

	}

	public void DrawProceed()
	{
		Vector3 msPos=Input.mousePosition;
		msPos.z = 1;
		if(msPos.y>490){msPos.y=490;}
		
		pointsCount++;
		Vector3 wP = Camera.main.GetComponent<Camera>().ScreenToWorldPoint(msPos);
		line.SetVertexCount (pointsCount);
		line.SetPosition (pointsCount - 1, wP);

		if (patternPointsCount >= staticPointsCount)
			for (int i=0; i<=patternPointsCount; i++) {
				bool unable = false;
				if (GetDistance2_3D (msPos, patternPoints [i]) <= 20) {
					for (int j=0; j<=staticPointsCount; j++) {
						if (staticPoints [j] == patternPoints [i]) {
							unable = true;

							if (j == 0 && patternPointsCount == staticPointsCount) {
								unable = false;
							}
						}

					}
					if (!unable) {
						staticPoints [staticPointsCount] = patternPoints [i];
						Vector3 toWorld = Camera.main.GetComponent<Camera> ().ScreenToWorldPoint (staticPoints [staticPointsCount]);
						staticPointsCount++;
						line.SetVertexCount (staticPointsCount);
						line.SetPosition (staticPointsCount - 1, toWorld);
						pointsCount = staticPointsCount;
					}
				}
			}
		else 
		{
			Goal ();
			CreatePattern();
			SetStateIdle ();
		}


		if (tailLenght >= tailLimit) 
		{
			for(int i=0; i<tailLenght; i++)
			{
				tailPoints[i]=tailPoints[i+1];
				tailPoints [tailLenght] = msPos;
				Vector3 toWorld = Camera.main.GetComponent<Camera>().ScreenToWorldPoint(tailPoints[i]);
				tail.SetPosition(i,toWorld);
			}
		} 
		else 
		{
			tailPoints [tailLenght] = msPos;
			tailLenght++;
			tail.SetVertexCount(tailLenght);
			tail.SetPosition(tailLenght-1,wP);
		}
	}

	public float GetDistance2_3D(Vector3 a, Vector3 b)
	{
		return Mathf.Abs (a.x - b.x) + Mathf.Abs (a.y - b.y);
	}

	public void CreatePattern()
	{
		patternPointsCount = Random.Range (3, anchorsLimit);
		Debug.Log (patternPointsCount);
		float getAverageAngle = 360 / patternPointsCount;
		patternPoints [0].x = 1366/2; 		patternPoints [0].y = 768/2;
		for (int i=1; i<=patternPointsCount; i++) 
		{
			patternPoints[i].x = Random.Range(200,1166);
			patternPoints[i].y = Random.Range(0,300);
		}

		for (int i=0; i<=patternPointsCount; i++) 
		{
			patternPoints[i].z=2;
			Vector3 toWorld = Camera.main.GetComponent<Camera>().ScreenToWorldPoint(patternPoints[i]);
			pattern.SetVertexCount(i+1);
			pattern.SetPosition(i,toWorld);
		}
		Vector3 wP = Camera.main.GetComponent<Camera>().ScreenToWorldPoint(patternPoints[0]);
		pattern.SetPosition(patternPointsCount,wP);

	}
	public void Goal()
	{
		score++;
		SetCurrentScore ();
		time_left = 20 - score;
		if (time_left < 2) 
		{
			time_left = 2;
			SetCurrentTime();
		}
	}

	public void SetCurrentTime()
	{
		time_state = GameObject.Find ("GameTimer").GetComponent<Text>();
		time_state.text = "Time: " + time_left + " s";
	}
	public void TimeProceed()
	{
		time_left -= Time.deltaTime;
		SetCurrentTime ();
	}

    private void SetCurrentScore()
    {
        score_state = GameObject.Find("GameScore").GetComponent<Text>();
        score_state.text = "Score: " + score;
    }
}
