using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using diskgame;


namespace mygame{
	
	public interface UserInterface{
		void emitDisk ();
	}

	public interface QueryStatus{
		bool isShooting();
		int getRound();
		int getPoint();
	} 

	public interface JudgeEvent{
		void nextRound();
		void setPoint(int point);
	}


	public class SceneController : System.Object, QueryStatus, UserInterface, JudgeEvent {

		private static SceneController instance;
		private	SceneController_Base current_scene;
		private Model gameModel;
		private Judge judge;

		private int round = 0;
		private int point = 0;

		public static SceneController getInstance(){
			if (instance == null) {
				instance = new SceneController ();
			}
			return instance;
		}

		public void emitDisk (){
			gameModel.prepareemit ();
		}

		public Model getModel(){
			return gameModel;
		}

		public void setModel(Model one){
			gameModel = one;
		}

		public Judge getJudge(){
			return judge;
		}

		public void setJudge(Judge one){
			judge = one;
		}

		public SceneController_Base getSceneController_Base(SceneController_Base one){
			return current_scene;
		}

		public void setSceneController_Base(SceneController_Base one){
			current_scene = one;
		}
			
		public void nextRound(){
			point = 0;
			current_scene.loadRound (++round);
		}

		public bool isShooting(){
			return gameModel.isShooting ();	
		}

		public int getRound(){
			return round;	
		}

		public int getPoint(){
			return point;
		}

		public void setPoint(int point){
			this.point = point;
		}

		public void Restart(){
			round = 0;
			nextRound ();
			judge.state = Judge.State.ChangeRound;
		}
	}

	public class SceneController_Base : MonoBehaviour{
		private Color color;
		private Vector3 position;
		private Vector3 direction;
		private float speed;

		void Awake(){
			SceneController.getInstance ().setSceneController_Base (this);
		}

		public void loadRound (int Round)
		{
			switch (Round) {
			case 1:
				color = Color.green;
				position = new Vector3 (-2f, 0.2f, -5f);
				direction = new Vector3 (20f, 30f, 50f);
				speed = 3;
				SceneController.getInstance ().getModel ().setting (color, position, direction.normalized, speed, 1);
				break;
			case 2:
				color = Color.red;
				position = new Vector3 (2f, 0.2f, -5f);
				direction = new Vector3 (-20f, 30f, 50f);
				speed = 3;
				SceneController.getInstance ().getModel ().setting (color, position, direction.normalized, speed, 2);
				break;
			default:
				SceneController.getInstance ().getJudge ().state = Judge.State.WIN;
				break;
			}
		}
	}
}
