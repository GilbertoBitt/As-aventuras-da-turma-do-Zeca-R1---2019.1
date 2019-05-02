using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewTutorIni : MonoBehaviour
{
    // Start is called before the first frame update
  public ControlSomTutor ControlSomTutor2;
  public GameObject somPref;
  public GameObject[] btsAv;
 public GameObject btPlay;
  public Vector3 tam;

  public GraphicRaycaster graphicRaycaster2;

   public Sprite[] imgsT;
   public Image imgsC;

  public string[] textFala;

  public Text textC;

  public Animator profAnim;
  public GameObject profBoca;
  public GameObject profOlhos;
  public bool fimFala;


  public 
    void Start()
    {
          somPref.SetActive(true);
          tam = btsAv[0].transform.localScale;
          ControlSomTutor2 = Camera.main.GetComponent<ControlSomTutor>();
            Invoke("profStopFalandoTime",9);
           // atvBt();
          Invoke("audioChamarIni",1);
    }

    // Update is called once per frame
    void audioChamarIni(){
        ControlSomTutor2.numTutor = 0;
        ControlSomTutor2.SomTutor();
         graphicRaycaster2.enabled=true; 
    }


void profStopFalandoTime(){
  fimFala=true;
    profAnim.enabled=false;
    profBoca.SetActive(false);
    profOlhos.SetActive(false);
    checkActBt();

}
 public void somTutor2(){
     
 ControlSomTutor2.SomTutor();
 }

    public void audioChamar(bool check){

        if(fimFala){
          avancBt(check);
        }
        for (int i = 0; i < btsAv.Length; i++)
        {
            btsAv[i].SetActive(false);
        }
    }

    void checkActBt(){
        if(ControlSomTutor2.numTutor == 0){
            btsAv[0].SetActive(false);            
        }
        else{
            btsAv[0].SetActive(true);
            btsAv[0].transform.localScale= tam;
        }
        if(ControlSomTutor2.numTutor == 4){
              btsAv[1].SetActive(false);
        }
        else{
             btsAv[1].SetActive(true);
             btsAv[1].transform.localScale= tam;
        }
        btPlay.SetActive(true);
    }

     public void  avancBt(bool check)
     {
        // wait for 1 second
        profAnim.enabled=true;
       
        for (int i = 0; i < btsAv.Length; i++)
        {
            btsAv[i].SetActive(false);
        }
      
        if(check){
            ControlSomTutor2.numTutor++;
            ControlSomTutor2.SomTutor();
             //yield return new WaitForSeconds(3.0f);
          
           
        }
        else{
             ControlSomTutor2.numTutor--;
             ControlSomTutor2.SomTutor();
              //yield return new WaitForSeconds(3.0f);
           
        }
      
        fimFala=false;
        if(ControlSomTutor2.numTutor==0){
        
          Invoke("profStopFalandoTime",9);
        }
        else if(ControlSomTutor2.numTutor==1){
        textC.text = textFala[ControlSomTutor2.numTutor];  
          Invoke("profStopFalandoTime",5);
        }
        else if(ControlSomTutor2.numTutor==2){
        textC.text = textFala[ControlSomTutor2.numTutor] + System.Environment.NewLine;  
         Invoke("profStopFalandoTime",5);
        }
        else if(ControlSomTutor2.numTutor==3){
        textC.text = textFala[ControlSomTutor2.numTutor] + System.Environment.NewLine;  
          Invoke("profStopFalandoTime",4);
        }
        else if(ControlSomTutor2.numTutor==4){
        textC.text = textFala[ControlSomTutor2.numTutor] + System.Environment.NewLine;  
          Invoke("profStopFalandoTime",4);
        }
        else{
        textC.text = textFala[ControlSomTutor2.numTutor];        
        }
         imgsC.sprite = imgsT[ControlSomTutor2.numTutor];
    }
}