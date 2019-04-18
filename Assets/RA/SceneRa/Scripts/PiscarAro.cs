using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityStandardAssets.CrossPlatformInput;
using MEC;

public class PiscarAro : MonoBehaviour {

    public GameObject olhosFechados;
    public Image bocaE;
    public bool checkPiscar;
    bool checkPiscar2;
    public bool checkFalar;
    bool checkFalar2;
    public float[] timePiscar;
    public float timeIN;
    public float timeINB;
    bool checkOlhos;
    int NumTimeOlhos;
    public float[] myTimeOlhos;
    float timeOlhos;

    void Start () {
        timeIN = Random.Range(0, timePiscar.Length);
        timeINB = Random.Range(1, 1.5f);

    }
	
	// Update is called once per frame
	void Update () {
        if (checkOlhos == false)
        {
            if (Time.realtimeSinceStartup - timeOlhos > myTimeOlhos[NumTimeOlhos])
            {
                olhosFechados.SetActive(true);
                StartCoroutine(FecharOlhos());
                timeOlhos = Time.realtimeSinceStartup;
                NumTimeOlhos = Random.Range(0, myTimeOlhos.Length);
            }

        }


        if (checkFalar && !checkFalar2) {
            checkFalar2 = true;
            Invoke("Mboca", timeINB);       
        }    
    }
    IEnumerator FecharOlhos()
    {
        yield return Yielders.Get(1f);
        olhosFechados.SetActive(false);
        checkOlhos = false;
        //StopCoroutine(FecharOlhos());




    }

    void Mboca()
    {
        timeINB = Random.Range(0.5f, 1f);

        bocaE.enabled = !bocaE.enabled;
        checkFalar2 = false;
    }
    void BocaM()
    {
        if (bocaE.enabled == true)
        {
            bocaE.enabled = false;
        }
    }

   
}
