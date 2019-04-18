using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlTextBalao : MonoBehaviour {

	public GameObject balaoGT;
	public GameObject controlCenario;
	public GameObject PersonagensG;
	public Text textB;
	public string[] textBalao;
	public int numTexBalao;
	public GameObject avancarBT;

	float timeM;
	public float timeMy;
	bool tempPassar;
	int numberPersons;
	void Start () {
		 timeMy=5f;
		
	}
	
	// Update is called once per frame
	void Update () {
		if(balaoGT.GetComponent<RectTransform>().localScale.x==0){
			textB.text = textBalao[numTexBalao];
		}
		if (Time.realtimeSinceStartup - timeM > timeMy && tempPassar==true ){
			BTAvancar();
			timeM = Time.realtimeSinceStartup;

		}
		
	}

	public void BTAvancar(){
		StopCoroutine (AparecerPersons());
		PersonagensG.GetComponent<Animator>().SetInteger("NumPersonagens",0);
		timeM = Time.realtimeSinceStartup;
		if(numTexBalao==0){
		numTexBalao= numTexBalao+1;
		avancarBT.GetComponent<Button>().interactable=false;
		StartCoroutine(TempoBtVoltar());
		balaoGT.GetComponent<Animator>().SetInteger("numText",numTexBalao);	
		timeMy=10f;
		}
		else if(numTexBalao==1){
		numTexBalao= numTexBalao+1;
		avancarBT.GetComponent<Button>().interactable=false;
		StartCoroutine(TempoBtVoltar());
		balaoGT.GetComponent<Animator>().SetInteger("numText",numTexBalao);	
		timeMy=15f;
		}
		else if(numTexBalao==2){
		numTexBalao= numTexBalao+1;
		avancarBT.GetComponent<Button>().interactable=false;
		StartCoroutine(TempoBtVoltar());
		balaoGT.GetComponent<Animator>().SetInteger("numText",numTexBalao);	
		timeMy=20f;
		}
		else if(numTexBalao==3){
		numTexBalao= numTexBalao+1;
		avancarBT.GetComponent<Button>().interactable=false;
		StartCoroutine(TempoBtVoltar());
		balaoGT.GetComponent<Animator>().SetInteger("numText",numTexBalao);	
		timeMy=10f;
		}
		else if(numTexBalao==4){
		numTexBalao= numTexBalao+1;
		avancarBT.GetComponent<Button>().interactable=false;
		StartCoroutine(TempoBtVoltar());
		balaoGT.GetComponent<Animator>().SetInteger("numText",numTexBalao);	
		timeMy=12f;
		}
		else if (numTexBalao==5){
		avancarBT.GetComponent<Button>().interactable=false;
		StartCoroutine(TempoBtVoltar());
		controlCenario.GetComponent<Animator>().SetInteger("numC",2);
	
		numTexBalao= numTexBalao+1;
		GetComponent<Transform>().localScale= new Vector3(GetComponent<Transform>().localScale.x*-1,GetComponent<Transform>().localScale.y,GetComponent<Transform>().localScale.z);
		balaoGT.GetComponent<Animator>().SetInteger("numText",numTexBalao);	
		timeMy=12f;
		}
		else if (numTexBalao==6){
		avancarBT.GetComponent<Button>().interactable=false;
		StartCoroutine(TempoBtVoltar());
		controlCenario.GetComponent<Animator>().SetInteger("numC",3);
		numTexBalao= numTexBalao+1;
		GetComponent<Transform>().localScale= new Vector3(GetComponent<Transform>().localScale.x*-1,GetComponent<Transform>().localScale.y,GetComponent<Transform>().localScale.z);
		balaoGT.GetComponent<Animator>().SetInteger("numText",numTexBalao);	
		timeMy=10f;
		}
		else if (numTexBalao==7){
		avancarBT.GetComponent<Button>().interactable=false;
		StartCoroutine(TempoBtVoltar());
		controlCenario.GetComponent<Animator>().SetInteger("numC",4);
		numTexBalao= 8;
		GetComponent<Transform>().localScale= new Vector3(GetComponent<Transform>().localScale.x*-1,GetComponent<Transform>().localScale.y,GetComponent<Transform>().localScale.z);
		balaoGT.GetComponent<Animator>().SetInteger("numText",numTexBalao);	
		controlCenario.GetComponent<controlRa1>().numAnim=3;
		timeMy=10f;
		}
		else if  (numTexBalao==8 ){	
		avancarBT.GetComponent<Button>().interactable=false;
		StartCoroutine(TempoBtVoltar());
		controlCenario.GetComponent<Animator>().SetInteger("numC",4);
		numTexBalao= numTexBalao+1;
		controlCenario.GetComponent<controlRa1>().numAnim=3;
		//GetComponent<Transform>().localScale= new Vector3(GetComponent<Transform>().localScale.x*-1,GetComponent<Transform>().localScale.y,GetComponent<Transform>().localScale.z);
		balaoGT.GetComponent<Animator>().SetInteger("numText",numTexBalao);	
		timeMy=10f;	
		}
		else if  (numTexBalao>=9 ){	
		avancarBT.GetComponent<Button>().interactable=false;
		StartCoroutine(TempoBtVoltar());
		if(controlCenario.GetComponent<controlRa1>().numAnim==3){
			controlCenario.GetComponent<controlRa1>().numAnim=1;
		}
		else if (controlCenario.GetComponent<controlRa1>().numAnim==1){
			controlCenario.GetComponent<controlRa1>().numAnim=3;
		}
		controlCenario.GetComponent<controlRa1>().numAnim=1;
	//	controlCenario.GetComponent<Animator>().SetInteger("numC",4);
		numTexBalao= numTexBalao+1;
		//GetComponent<Transform>().localScale= new Vector3(GetComponent<Transform>().localScale.x*-1,GetComponent<Transform>().localScale.y,GetComponent<Transform>().localScale.z);
		balaoGT.GetComponent<Animator>().SetInteger("numText",numTexBalao);		
		}
		if  (numTexBalao>9 ){
			tempPassar=false;
		}


	}
	IEnumerator TempoBTAvancar(){
	yield return new WaitForSeconds (4f);
	avancarBT.GetComponent<Button>().interactable=true;
	
	}
	IEnumerator TempoBtVoltar(){
	yield return new WaitForSeconds (4f);
	avancarBT.GetComponent<Button>().interactable=true;
	
	}
	IEnumerator AparecerPersons(){
	if(numTexBalao==5){
		numberPersons = 1;
		yield return new WaitForSeconds (3f);
		if(numberPersons==1){
		PersonagensG.GetComponent<Animator>().SetInteger("NumPersonagens",1);
		}
		
	}
	if(numTexBalao==6){
		numberPersons = 2;
		yield return new WaitForSeconds (2f);
		if(numberPersons==2){
		PersonagensG.GetComponent<Animator>().SetInteger("NumPersonagens",2);
		}
	}
	if(numTexBalao==7){
		numberPersons = 3;
		yield return new WaitForSeconds (1f);
		numberPersons = 3;
		if(numberPersons==3){
		PersonagensG.GetComponent<Animator>().SetInteger("NumPersonagens",3);
		}
	}
	
	}
	
	
	void ExibirText_1(){
	if(numTexBalao==0){
		numTexBalao=1;
		balaoGT.GetComponent<Animator>().SetInteger("numText",numTexBalao);	
		 tempPassar=true;
		 timeM = Time.realtimeSinceStartup;	
	}
	if(numTexBalao==5){
		controlCenario.GetComponent<Animator>().SetInteger("numC",1);
		GetComponent<Animator>().SetFloat("velAndar",0.1f);
	    StartCoroutine(AparecerPersons());
	}
	if(numTexBalao==6){
		GetComponent<Animator>().SetInteger("numText", numTexBalao);
		GetComponent<Animator>().SetFloat("velAndar",0.1f);
		StartCoroutine(AparecerPersons());
	}
	if(numTexBalao==7){
		GetComponent<Animator>().SetInteger("numText", numTexBalao);
		GetComponent<Animator>().SetFloat("velAndar",0.1f);
		StartCoroutine(AparecerPersons());
	}
	if(numTexBalao==8){
	
		GetComponent<Animator>().SetInteger("numText", numTexBalao);
		GetComponent<Animator>().SetFloat("velAndar",0.1f);
	}
	if(numTexBalao>=9){
	//	controlCenario.GetComponent<controlRa1>().numAnim=4;
		GetComponent<Animator>().SetInteger("numText", numTexBalao);
		GetComponent<Animator>().SetFloat("velAndar",0.1f);
	}
	}
	
}
