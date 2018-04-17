using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using diskgame;

public class Judge : MonoBehaviour {

	public enum State {WIN, InRound, ChangeRound, LOSE}

	private SceneController scene;
	private int win_point = 20;

	public State state = State.ChangeRound; 

	void Awake(){
		scene = SceneController.get_instance ();
		scene.setJudge (this);
	}

	// Use this for initialization
	void Start () {
		state = State.ChangeRound;
		scene.nextRound ();
	}

	public void scoreAdd(){
		scene.setPoint (scene.getPoint () + 5);
		if (scene.getPoint () == win_point * scene.getRound() && state == State.InRound) {
			state = State.ChangeRound;
			scene.nextRound ();
		}
	}

	public void scoreSub(){
		scene.setPoint (scene.getPoint () - 10);
		if (scene.getPoint () < 0 && state == State.InRound)
			state = State.LOSE;
	}
}
