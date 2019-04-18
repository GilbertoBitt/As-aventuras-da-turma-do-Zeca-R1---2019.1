using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformAmigos : MonoBehaviour {

	public GameObject biaG;
	public GameObject tatiG;
	public GameObject pauloG;
	public GameObject manuG;
	public GameObject joaoG;
	void Start () {

		biaG.SetActive(false);
		tatiG.SetActive(false);
		pauloG.SetActive(false);
		manuG.SetActive(false);
		joaoG.SetActive(false);

		
	}

	void AmaigosOff () {

		biaG.SetActive(false);
		tatiG.SetActive(false);
		pauloG.SetActive(false);
		manuG.SetActive(false);
		joaoG.SetActive(false);

		
	}

	void BiaON () {

		biaG.SetActive(true);
			
	}
	void TatiON () {

		tatiG.SetActive(true);
			
	}
	void PauloON () {

		pauloG.SetActive(true);
			
	}
	void ManuON () {

		manuG.SetActive(true);
			
	}
	void JoaoON () {

		joaoG.SetActive(true);
			
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
