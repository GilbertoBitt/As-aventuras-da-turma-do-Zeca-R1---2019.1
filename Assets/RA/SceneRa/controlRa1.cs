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

	private static readonly int Andando = Animator.StringToHash("andando");

	private static readonly int Acenando = Animator.StringToHash("acenando");

	private static readonly int NumAnim = Animator.StringToHash("numAnim");

	private static readonly int NumText = Animator.StringToHash("numText");

	private static readonly int NumC = Animator.StringToHash("numC");

	private static readonly int NumPersonagens = Animator.StringToHash("NumPersonagens");
	//public float h;

	
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		person.GetComponent<Animator>().SetBool (Andando,andando);
		person.GetComponent<Animator>().SetBool (Acenando,acenando);
		person.GetComponent<Animator>().SetInteger(NumAnim,numAnim);

		
	}
	public void andarOn(){
		andando = true;
		if(numAnim==2) {
			numAnim = 1;
			person.GetComponent<Animator>().SetInteger(NumText, person.GetComponent<ControlTextBalao>().numTexBalao);
		}
		else if(numAnim==3){
			numAnim = 3;
			person.GetComponent<Animator>().SetInteger(NumText, person.GetComponent<ControlTextBalao>().numTexBalao);

		}
			
	}
	public void andarOff(){
		andando = false;
		if(numAnim==0){
			numAnim = 1;			
		}
		else if(numAnim==1){
			numAnim = 2;
			person.GetComponent<Animator>().SetInteger(NumText, person.GetComponent<ControlTextBalao>().numTexBalao);
		}	
		else if(numAnim==3){
			numAnim = 1;
			person.GetComponent<Animator>().SetInteger(NumText, person.GetComponent<ControlTextBalao>().numTexBalao);
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
		person.GetComponent<Animator>().SetInteger(NumText,0);
		numAnim = 0;
		acenando = false;
		andando = false;
		GetComponent<Animator>().SetInteger(NumC,0);
		person.GetComponent<ControlTextBalao>().timeMy = 5f;
		person.GetComponent<ControlTextBalao>().PersonagensG.GetComponent<Animator>().SetInteger(NumPersonagens,0);
		if (person.GetComponent<Transform> ().GetComponent<Transform> ().localScale.x < 0) {
			person.GetComponent<Transform> ().GetComponent<Transform> ().localScale = new Vector3 (person.GetComponent<Transform> ().GetComponent<Transform> ().localScale.x*-1,person.GetComponent<Transform> ().GetComponent<Transform> ().localScale.y,person.GetComponent<Transform> ().GetComponent<Transform> ().localScale.z);
		}

	}
}
