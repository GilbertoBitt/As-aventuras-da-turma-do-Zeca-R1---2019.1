using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class JucaBauMontanha : MonoBehaviour {

	public Manager1_4A Manager1_Mont;
	public Animator bau;
	public Animator zeca;
    bool checkpassAudio;
  //  public TutorMontanha TutorMontanha2;
    void Start() {
       // TutorMontanha2 = Manager1_Mont.TutorMontanha2;

    }
    void JucaFecha () {
		bau.enabled = true;
		Manager1_Mont.closeChest();
		
	}
	void Jucafalse () {
		GetComponent<Animator> ().enabled = true;
		Manager1_Mont.checkJucaBau=false;
		GetComponent<Animator>().SetBool("fecharBau",false);
		
	}


	void JucaBautrue () {
		bau.enabled = true;
		zeca.enabled = true;
		zeca.GetComponent<Animator>().SetBool("AbrirBau",false);
	}

}
