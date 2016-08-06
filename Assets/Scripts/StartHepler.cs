using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StartHepler : MonoBehaviour {

	GameObject startScr, mainScr, game, gameOverScr;
	// Use this for initialization
	void Start () {
        startScr    = GameObject.Find ("StartScr");
		mainScr     = GameObject.Find ("GameScr");
		game         = GameObject.Find ("Game");
        gameOverScr = GameObject.Find ("GameOverScr");
		game.SetActive (false);
		gameOverScr.SetActive (false);
		Canvas paintCanvas = mainScr.GetComponent<Canvas> ();
		paintCanvas.enabled = false;	
	}

	public void Exit()
	{
		Application.Quit ();
	}
	public void LetsStartIt()
	{
		Canvas menuCanvas = startScr.GetComponent<Canvas> ();
		Canvas paintCanvas = mainScr.GetComponent<Canvas> ();
		menuCanvas.enabled = false;
		paintCanvas.enabled = true;
		game.SetActive (true);

	}
	// Update is called once per frame 
	void Update () {
	
	}
}
