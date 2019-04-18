using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;
public class controlRa1 : MonoBehaviour {

	public GameObject person;
	



	public bool andando;
	public bool acenando;
	public int numAnim;
	//public float h;

	
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		person.GetComponent<Animator>().SetBool ("andando",andando);
		person.GetComponent<Animator>().SetBool ("acenando",acenando);
		person.GetComponent<Animator>().SetInteger("numAnim",numAnim);

		
	}
	public void andarOn(){
		andando = true;
		if(numAnim==2) {
			numAnim = 1;
			person.GetComponent<Animator>().SetInteger("numText", person.GetComponent<ControlTextBalao>().numTexBalao);
		}
		else if(numAnim==3){
			numAnim = 3;
			person.GetComponent<Animator>().SetInteger("numText", person.GetComponent<ControlTextBalao>().numTexBalao);

		}
			
	}
	public void andarOff(){
		andando = false;
		if(numAnim==0){
			numAnim = 1;			
		}
		else if(numAnim==1){
			numAnim = 2;
			person.GetComponent<Animator>().SetInteger("numText", person.GetComponent<ControlTextBalao>().numTexBalao);		
		}	
		else if(numAnim==3){
			numAnim = 1;
			person.GetComponent<Animator>().SetInteger("numText", person.GetComponent<ControlTextBalao>().numTexBalao);		
		}	
		
		
	}
	public void backR1()
	{
		StartCoroutine (TimebackR1());
	}

	public IEnumerator TimebackR1()
	{
		yield return new WaitForSeconds (1f);
		person.GetComponent<ControlTextBalao>().numTexBalao=0;
		person.GetComponent<Animator>().SetInteger("numText",0);
		numAnim = 0;
		acenando = false;
		andando = false;
		GetComponent<Animator>().SetInteger("numC",0);
		person.GetComponent<ControlTextBalao>().timeMy = 5f;
		person.GetComponent<ControlTextBalao>().PersonagensG.GetComponent<Animator>().SetInteger("NumPersonagens",0);
		if (person.GetComponent<Transform> ().GetComponent<Transform> ().localScale.x < 0) {
			person.GetComponent<Transform> ().GetComponent<Transform> ().localScale = new Vector3 (person.GetComponent<Transform> ().GetComponent<Transform> ().localScale.x*-1,person.GetComponent<Transform> ().GetComponent<Transform> ().localScale.y,person.GetComponent<Transform> ().GetComponent<Transform> ().localScale.z);
		}

	}
}
