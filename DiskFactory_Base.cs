using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using diskgame;

namespace diskgame{
	public class DiskFactory : System.Object {
		
		private static DiskFactory instance;
		private static List<GameObject> disks;
		public GameObject diskInstance;

		public static DiskFactory getInstance(){
			if (instance == null){
				instance = new DiskFactory ();
				disks = new List<GameObject> ();
			}
			return instance;
		}
	
		public GameObject getDisk(){
			for (int i = 0; i < disks.Count; i++) {
				if (!disks [i].activeInHierarchy)
					return disks[i];
			}
			disks.Add (GameObject.Instantiate (diskInstance) as GameObject);
			return disks [disks.Count - 1];
		}

		public void freeDisk(int id)
		{
			if (id >= 0 && id <= disks.Count - 1) {
				disks [id].GetComponent<Rigidbody>().velocity = Vector3.zero;
				disks [id].SetActive (false);
			}
		}
	}

	public class DiskFactory_Base : MonoBehaviour{
		public GameObject diskInstance;

		void Awake(){
			DiskFactory.getInstance ().diskInstance = diskInstance;
		}
	}
}
