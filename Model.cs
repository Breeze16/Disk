using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using diskgame;

public class Model : MonoBehaviour {

	private bool shooting = false;

	public bool isShooting(){
		return shooting;
	}

	private List<GameObject> disks = new List<GameObject> ();

	private Color disk_color;
	private Vector3 disk_position;
	private Vector3 disk_direction;
	private float disk_speed;
	private int disk_number;
	private bool disk_enable;

	private SceneController scene;

	void Awake(){
		scene = SceneController.get_instance ();
		scene.setModel (this);
	}

	public void setting(Color color, Vector3 position, Vector3 direction, float speed, int num){
		disk_color = color;
		disk_position = position;
		disk_direction = direction;
		disk_speed = speed;
		disk_number = num;
	}

	public void prepareemit(){
		if (!shooting) {
			disk_enable = true;
		}
	}

	void emit(){
		for (int i = 0; i < disk_number; i++) {
			disks.Add (DiskFactory.get_instance ().get_disk ());
			disks [i].GetComponent<Renderer> ().material.color = disk_color;
			disks [i].transform.position = disk_position;
			disks [i].SetActive (true);
			disks [i].GetComponent<Rigidbody> ().AddForce (disk_direction * Random.Range (0.5f, 1f) * disk_speed, ForceMode.Impulse);
		}
	}

	void freedisk(int id){
		DiskFactory.get_instance ().free_disk (id);
		disks.RemoveAt (id);
	}

	void FixedUpdate(){
		if (disk_enable) {
			emit ();
			disk_enable = false;
			shooting = true;
		}
	}

	// Update is called once per frame
	void Update () {
		for (int i = 0; i < disks.Count; i++) {
			if (!disks [i].activeInHierarchy) {
				scene.getJudge ().scoreAdd ();
				freedisk (i);
			} else if (disks [i].transform.position.y < 0) {
				scene.getJudge ().scoreSub ();
				freedisk (i);
			}
			if (disks.Count == 0)
				shooting = false;
		}
	}
}
