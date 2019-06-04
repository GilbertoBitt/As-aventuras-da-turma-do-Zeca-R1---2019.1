using System.Collections;
using System.Collections.Generic;
using MiniGames.Scripts;
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

  public float[] timeFala;

  public bool[] checkB;

  public GameObject btRepet;




  public 
    void Start()
    {
          somPref.SetActive(true);
          tam = btsAv[0].transform.localScale;
          ControlSomTutor2 = Camera.main.GetComponent<ControlSomTutor>();
            if(checkB[ControlSomTutor2.numTutor]){
            Invoke("profStopFalandoTime",timeFala[ControlSomTutor2.numTutor]);
            }
           
           // atvBt();
          Invoke("audioChamarIni",1);

             Debug.Log(textFala.Length);     

    }

    // Update is called once per frame
    void audioChamarIni(){
        ControlSomTutor2.numTutor = 0;
         Invoke("profStopFalandoTime",timeFala[ControlSomTutor2.numTutor]);
        if(ControlSomTutor2.audiosTutorial[ControlSomTutor2.numTutor]!=null){
          
      ControlSomTutor2.SomTutor();
        }
       
         graphicRaycaster2.enabled=true; 
    }


void profStopFalandoTime(){
  fimFala=true;
   btsAv[1].GetComponent<Button>().interactable=true;
    profAnim.enabled=false;
    profBoca.SetActive(false);
    profOlhos.SetActive(false);
    btRepet.SetActive(true);
    checkActBt();

}
 public void somTutor2(){
     
 if(ControlSomTutor2.audiosTutorial[ControlSomTutor2.numTutor]!=null){
      ControlSomTutor2.SomTutor();
        }
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
    public void audioRepete(){
       for (int i = 0; i < btsAv.Length; i++)
        {
            btsAv[i].SetActive(false);
        }   
    ControlSomTutor2.SomTutor();
 profAnim.enabled=true;
      Invoke("profStopFalandoTime",timeFala[ControlSomTutor2.numTutor]);
    

    }

    void checkActBt(){
        if(ControlSomTutor2.numTutor == 0){
            btsAv[0].SetActive(false);            
        }
        else{
            btsAv[0].SetActive(true);
            btsAv[0].transform.localScale= tam;
        }
       
        if(textFala.Length==(ControlSomTutor2.numTutor+1)){
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
            if(ControlSomTutor2.audiosTutorial[ControlSomTutor2.numTutor]!=null){
              ControlSomTutor2.SomTutor();
              }
        }
        else{
             ControlSomTutor2.numTutor--;
             if(ControlSomTutor2.audiosTutorial[ControlSomTutor2.numTutor]!=null){
               ControlSomTutor2.SomTutor();
             }
        }      
        fimFala=false;
        if(checkB[ControlSomTutor2.numTutor]){
           textC.text = textFala[ControlSomTutor2.numTutor] + System.Environment.NewLine;  
        }
        else{
          textC.text = textFala[ControlSomTutor2.numTutor];
        }
         imgsC.sprite = imgsT[ControlSomTutor2.numTutor];
          Invoke("profStopFalandoTime",timeFala[ControlSomTutor2.numTutor]);
    }
}