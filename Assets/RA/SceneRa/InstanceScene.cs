using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MEC;

public class InstanceScene : MonoBehaviour {

	public GameObject Prefabs;
	public Transform ParentInstance;
	public GameObject InstanceOnScene;
    public int NumInst;
    public ControlTelaCheia ControlTelaCheia2;
    public GameObject BtTelaCheia;
   
  
    //public void Instan

    public void EnterOnScene(){

        //Invoke("Instance", 0.2f);
        //Debug.Log("Marker Encontrado ----------------------");
        Instance();
        if (BtTelaCheia != null) {
            BtTelaCheia.SetActive(true);
        }
       
        ControlTelaCheia.ArIns = NumInst;
    }

	public void ExitOnScene(){
        BtTelaCheia.SetActive(false);
        Destroy (InstanceOnScene);
	}

    public void Instance() {
        if(InstanceOnScene != null)  {
            Destroy(InstanceOnScene);
        }
        InstanceOnScene = Instantiate(Prefabs, ParentInstance) as GameObject;

        
    }

}
