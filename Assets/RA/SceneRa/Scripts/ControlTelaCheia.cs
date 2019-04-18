using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ControlTelaCheia : MonoBehaviour
{

    public static int ArIns;
    public LoadManager LoadManager2;
    public GameObject loadImage;
    public TextMeshProUGUI textMesh;

    public void ChamarTelaCheia(int numRA)
    {
		ArIns = numRA;
//        textMesh.text = "Mudando para Tela Cheia.";
//        loadImage.SetActive(true);
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_ANDROID
      //  ARToolkitHelper.StopCapture();
#endif
        Invoke("LoadAsyncDelay", 2f);
    }

    public void LoadAsyncDelay() {

        LoadManager2.LoadAsync("RA3");
    }

}
