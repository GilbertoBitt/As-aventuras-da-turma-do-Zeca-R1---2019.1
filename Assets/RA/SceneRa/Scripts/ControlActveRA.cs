using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using MEC;

public class ControlActveRA : MonoBehaviour {

    public GameObject[] objRa;
    public LoadManager loadManager;
    public GameObject[] bts;
    public GameObject telaTrans;
    void Start () {

        if (objRa[ControlTelaCheia.ArIns]!=null)
        objRa[ControlTelaCheia.ArIns].SetActive(true);

    }

    public void MenuAR(bool checkMenu)
    {
        for (int i = 0; i < bts.Length; i++)
        {
            bts[i].SetActive(false);
        }
        for (int i = 0; i < objRa.Length; i++)
        {
            if (objRa[i] != null) {
                objRa[i].SetActive(false);
            }
          
        }
        telaTrans.SetActive(true);
        if (checkMenu)
        {
            Invoke("ChamarMenu", 1f);
        }
        else
        {
            Invoke("ChamarAR", 1f);
        }
    }

    void ChamarMenu()
    {
        loadManager.LoadAsync("selectionMinigames");
    }
    void ChamarAR()
    {
        loadManager.LoadAsync("RA2");
    }

}
