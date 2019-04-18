using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MEC;

public class Ra3ControlPaulo : MonoBehaviour {

    public Vector3 offsetFollowB;
    public Vector3 defaultOffsetB;
    public float transitionDurationB;
    public GameObject balaoG;
    public AnimationCurve transitionCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
    public GameObject BtAvancar;
    public GameObject BtVoltar;
    public Text textBalao;
    public string[] textMy;
    int numtext;
    public ExpressFacialZeca ExpressFacialZeca1;
    public GameObject DesObj;
    public LoadManager loadManager;
    public GameObject BTFim;

    public AudioClip[] audios;
    public SoundManager soundManager;
    public bool checkTelacheia;

    void Start () {
        ExpressFacialZeca1 = GetComponent<ExpressFacialZeca>();
        BtVoltar.SetActive(false);
        Timing.RunCoroutine(BalaoScaleStart());
        loadManager = FindObjectOfType<LoadManager>();
        if (!checkTelacheia) {
			soundManager = GameObject.FindWithTag ("SoundManager").GetComponent<SoundManager> ();
		
        }
    }
    public void Mfim()
    {
        KillAllCoroutines();
        Invoke("InvokeM", 1f);
        DesObj.SetActive(false);
    }

    void InvokeM() {
        loadManager.gameObject.SetActive(true);
        loadManager.LoadAsync("selectionMinigames");

    }

    public void BtAvacnca(bool checkVoltar) {
        if (!checkVoltar)
        {
            BtAvancar.SetActive(false);
            BtVoltar.SetActive(false);
            ExpressFacialZeca1.checkFalando = false;
            numtext = numtext + 1;
            Timing.RunCoroutine(BalaoScale(numtext));
            if (numtext == 2)
            {
                ExpressFacialZeca1.falarTriste = true;
            }
        }
        else {
            BtAvancar.SetActive(false);
            BtVoltar.SetActive(false);
            ExpressFacialZeca1.checkFalando = false;
            numtext = numtext - 1;
            Timing.RunCoroutine(BalaoScale(numtext));
            if (numtext == 2)
            {
                ExpressFacialZeca1.falarTriste = true;
            }
        }
    }

    void tristeOff() {
        ExpressFacialZeca1.falarTriste = false;
    }
    public IEnumerator<float> BalaoScale(int nunbT)
    {        
        Vector3 startOffsetB = offsetFollowB;
        Vector3 endOffsetB = defaultOffsetB;
        float timesB = 0.0f;       

        while (timesB < transitionDurationB)
        {
            timesB += Time.deltaTime;
            float s = timesB / transitionDurationB;
            balaoG.transform.localScale = Vector3.Lerp(endOffsetB, startOffsetB, transitionCurve.Evaluate(s));
            yield return Timing.WaitForOneFrame;
        }

        textBalao.text = textMy[nunbT].ToUpper();
       
        if (numtext == 2)
        {
            Invoke("tristeOff", 1.5f);
        }
        ExpressFacialZeca1.checkFalando = true;
        timesB = 0.0f;
        while (timesB < transitionDurationB)
        {
            timesB += Time.deltaTime;
            float s = timesB / transitionDurationB;
            balaoG.transform.localScale = Vector3.Lerp(startOffsetB, endOffsetB, transitionCurve.Evaluate(s));
            yield return Timing.WaitForOneFrame;
        }
        soundManager.startVoiceFX(audios[numtext]);
        if (numtext < 4)
        {
            BtAvancar.SetActive(true);         
        }
        if (numtext == 0)
        {
            BtVoltar.SetActive(false);
        }
        else
        {
         BtVoltar.SetActive(true);
        }
        if (numtext > 3)
        {
            yield return Timing.WaitForSeconds(3f);
            BTFim.SetActive(true);
        }
        else {
            BTFim.SetActive(false);
        }
       
    }
    public IEnumerator<float> BalaoScaleStart()
    {
        Vector3 startOffsetB = offsetFollowB;
        Vector3 endOffsetB = defaultOffsetB;
        float timesB = 0.0f;

        while (timesB < transitionDurationB)
        {
            timesB += Time.deltaTime;
            float s = timesB / transitionDurationB;
			if (balaoG != null) {
				balaoG.transform.localScale = Vector3.Lerp (startOffsetB, endOffsetB, transitionCurve.Evaluate (s));
				yield return Timing.WaitForOneFrame;
			}
        }
        soundManager.startVoiceFX(audios[numtext]);
        ExpressFacialZeca1.checkFalando = true;
		if (BtAvancar != null) {
			BtAvancar.SetActive (true);

		}
	
		if (BtVoltar != null) {
			
			BtVoltar.SetActive (false);
		}
    }
    public void KillAllCoroutines()
    {
        Timing.KillCoroutines();
    }
}
