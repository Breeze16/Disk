using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using diskgame;

public class UI : MonoBehaviour {

	private int round = 0;

	public GameObject bullet;
	public ParticleSystem explosion;
	public float bullet_rate = 0.25f;
	public float bullet_speed = 500f;

	private float nextFireTime;

	private SceneController scene;

	// Use this for initialization
	void Start () {
		bullet = GameObject.Instantiate (bullet) as GameObject;
		explosion = GameObject.Instantiate (explosion) as ParticleSystem;
		explosion.Stop ();
		scene = SceneController.getInstance ();
	}

	// Update is called once per frame
	void Update () {
		if (scene.getJudge ().state == Judge.State.WIN || scene.getJudge ().state == Judge.State.LOSE)
			return;
		if (Input.GetKeyDown ("space")) {
			scene.getJudge ().state = Judge.State.InRound;
			scene.emitDisk ();
		}
		if (scene.isShooting() && Input.GetMouseButtonDown (0) && Time.time > nextFireTime) {
			nextFireTime = Time.time + bullet_rate;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			bullet.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			bullet.transform.position = this.transform.position;
			bullet.GetComponent<Rigidbody> ().AddForce (ray.direction * bullet_speed, ForceMode.Impulse);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit) && hit.collider.gameObject.tag == "disk") {
				explosion.transform.position = hit.collider.gameObject.transform.position;
				explosion.GetComponent<Renderer> ().material = hit.collider.gameObject.GetComponent<Renderer> ().material;
				explosion.Play ();
				hit.collider.gameObject.SetActive (false);
			}
		}
		if (round != scene.getRound()) {
			round = scene.getRound();
		}
	}

	void OnGUI(){
		if (scene.getJudge ().state == Judge.State.ChangeRound) {
			GUI.Label (new Rect (100, 100, 100, 100), "Round " + round);
			GUI.Label (new Rect (20, 20, 100, 100), "Round: ");
			GUI.Label (new Rect (120, 20, 100, 100), "Score: ");
		} else if (scene.getJudge ().state == Judge.State.WIN) {
			if (GUI.Button (new Rect (100, 100, 100, 100), "You are win!"))
				scene.Restart ();
		} else if (scene.getJudge ().state == Judge.State.LOSE) {
			if (GUI.Button (new Rect (100, 100, 100, 100), "You are lose!"))
				scene.Restart ();
		}
		else {
			GUI.Label (new Rect (20, 20, 100, 100), "Round: " + round);
			GUI.Label (new Rect (120, 20, 100, 100), "Score: " + scene.getPoint());
		}
	}
}
