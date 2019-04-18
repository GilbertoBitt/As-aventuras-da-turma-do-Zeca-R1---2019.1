using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetarCan : MonoBehaviour {

    public Camera eventCamera;
    void Start () {
        GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
        GetComponent<Canvas>().worldCamera = Camera.main;



     
     

    }
	
}
