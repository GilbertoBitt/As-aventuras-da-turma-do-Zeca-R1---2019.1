using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveNuvem : MonoBehaviour {

	public float movementTime;
	public Transform posFim;
	public Transform posIni;
	bool passou;
	public bool dirpref;

	void Start () {
		
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (posFim == null || posIni == null){
			Destroy (gameObject);
		}
		else{
			transform.position = Vector3.Lerp (transform.position, posFim.transform.position, (Time.deltaTime * movementTime));
		if (transform.position.x > posFim.transform.position.x-3 && passou==false && dirpref) {
			passou = true;
			transform.position = new Vector3(posIni.transform.position.x,posIni.transform.position.y,posIni.transform.position.z);
			passou = false;
			
		}
		else if (transform.position.x < posFim.transform.position.x+3 && passou==false && dirpref==false) {
			passou = true;
			transform.position = new Vector3(posIni.transform.position.x,posIni.transform.position.y,posIni.transform.position.z);
			passou = false;

		}

		}
		
	}
}
