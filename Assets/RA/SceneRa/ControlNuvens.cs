using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlNuvens : MonoBehaviour {
	

	public GameObject[] nuvemPref;
	public Transform[] posFim;
	public Transform[] posN;
	float timeM;
	public float[] timeMy;
	public float[] velNuvem;
	int numTemp;
	public int numbLimtNuv;
	public int numNuvs;
	public bool dir;
	void Start () {
		//timeM = Time.realtimeSinceStartup;
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.realtimeSinceStartup - timeM > timeMy[numTemp] && numNuvs<numbLimtNuv ) {
			numTemp = Random.Range (0,timeMy.Length);	
			int numPos = Random.Range (0,posN.Length);
			int numNuvemPref = Random.Range (0,nuvemPref.Length);
			GameObject nuvem = Instantiate(nuvemPref[numNuvemPref], posN[numPos].transform.position, Quaternion.identity, this.transform) as GameObject;
			this.nuvemPref[numNuvemPref].GetComponent<MoveNuvem>().dirpref = dir;
			numNuvs = numNuvs + 1;
			this.nuvemPref[numNuvemPref].GetComponent<MoveNuvem>().posFim = posFim[numPos];
			this.nuvemPref[numNuvemPref].GetComponent<MoveNuvem>().posIni = posN[numPos];
			int numVel = Random.Range (0,velNuvem.Length);
			this.nuvemPref[numNuvemPref].GetComponent<MoveNuvem>().movementTime = velNuvem[numVel];
			timeM = Time.realtimeSinceStartup;
		}

		
	}
}
