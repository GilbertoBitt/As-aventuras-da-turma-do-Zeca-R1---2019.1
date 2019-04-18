using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MEC;

public class ControlR5Apress : MonoBehaviour {

    public Vector3 offsetFollowB;
    public Vector3 defaultOffsetB;
    public float transitionDurationB;
    public GameObject balaoG;
    public AnimationCurve transitionCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
    public GameObject BtAvancar;
    public GameObject BtVoltar;
    public Animator controlAnim;
    public Text textBalao;
    public string[] textMy;
    public int numtext;

    public RectTransform posBalao;
    public RectTransform[] posOBJs;
    public GameObject[] setasB;
    public PiscarAro[] PiscarAro2;

    public AudioClip[] audios;
    public SoundManager soundManager;

    public GameObject DesObj;
    public LoadManager loadManager;
    public GameObject BTFim;
    public bool checkTelacheia;

    void Start () {
        BtVoltar.SetActive(false);
        Timing.RunCoroutine(BalaoScaleStart());
        textBalao.text = textMy[0].ToUpper();
        posBalao.transform.position = posOBJs[0].transform.position;
        setasB[0].SetActive(true);
        PiscarAro2[0].checkFalar = true;
        if (!checkTelacheia) {
			soundManager = GameObject.FindWithTag ("SoundManager").GetComponent<SoundManager> ();
        }
        // soundManager = Camera.main.GetComponent<SoundManager>();
        loadManager = FindObjectOfType<LoadManager>();
    }

    public void Mfim()
    {
        KillAllCoroutines();
        Invoke("InvokeM", 1f);
        DesObj.SetActive(false);
    }

    void InvokeM()
    {
        loadManager.gameObject.SetActive(true);
        loadManager.LoadAsync("selectionMinigames");
    }

    public void BtAvacnca(bool checkVoltar)
    {
        if (!checkVoltar && numtext <= 3)
        {
            BtAvancar.SetActive(false);
            BtVoltar.SetActive(false);
            numtext = numtext + 1;
            Timing.RunCoroutine(BalaoScale(numtext));

        }
        else if (checkVoltar && numtext <= 3)
        {
            BtAvancar.SetActive(false);
            BtVoltar.SetActive(false);
            numtext = numtext - 1;
            Timing.RunCoroutine(BalaoScale(numtext));
        }


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
            yield return 0f;
        }

        textBalao.text = textMy[nunbT].ToUpper(); ;

        for (int i = 0; i < setasB.Length; i++)
        {
            setasB[i].SetActive(false);
        }
        for (int i = 0; i < PiscarAro2.Length; i++)
        {
            PiscarAro2[i].checkFalar = false;
            PiscarAro2[i].bocaE.enabled = false;
        }


        if (numtext == 0)
        {
           
            setasB[0].SetActive(true);
            posBalao.transform.position = posOBJs[0].transform.position;
            PiscarAro2[0].checkFalar = true;          


        }
        else if (numtext == 1) {
            setasB[1].SetActive(true);
            setasB[5].SetActive(true);
            posBalao.transform.position = posOBJs[2].transform.position;
        }
        else if (numtext == 2)
        {
            posBalao.transform.position = posOBJs[1].transform.position;
            setasB[4].SetActive(true);
            PiscarAro2[1].checkFalar = true;
        }
        else if (numtext == 3)
        {
            posBalao.transform.position = posOBJs[1].transform.position;
            setasB[2].SetActive(true);
            setasB[3].SetActive(true);
            PiscarAro2[2].checkFalar = true;
            PiscarAro2[3].checkFalar = true;
        }
        soundManager.startVoiceFX(audios[numtext]);
        timesB = 0.0f;
        while (timesB < transitionDurationB)
        {
            timesB += Time.deltaTime;
            float s = timesB / transitionDurationB;
            balaoG.transform.localScale = Vector3.Lerp(startOffsetB, endOffsetB, transitionCurve.Evaluate(s));
            yield return 0f;
        }
        if (numtext < 3)
        {
            BtAvancar.SetActive(true);
        }
        if (numtext <= 0)
        {
            BtVoltar.SetActive(false);
        }
        else
        {
            BtVoltar.SetActive(true);
        }
        if (numtext > 2)
        {
            yield return Timing.WaitForSeconds(3f);
            BTFim.SetActive(true);
        }
        else
        {
            BTFim.SetActive(false);
        }

    }


    public IEnumerator<float> BalaoScaleEnd()
    {
        // StopCoroutine(BalaoScale(numtext));

        BtAvancar.SetActive(false);
        BtVoltar.SetActive(false);

        Vector3 startOffsetB = offsetFollowB;
        Vector3 endOffsetB = defaultOffsetB;
        float timesB = 0.0f;

        yield return Timing.WaitForSeconds(3f);
        //jucaAnim.SetInteger("animJuca", 1);
        while (timesB < transitionDurationB)
        {
            timesB += Time.deltaTime;
            float s = timesB / transitionDurationB;
            balaoG.transform.localScale = Vector3.Lerp(endOffsetB, startOffsetB, transitionCurve.Evaluate(s));
            yield return 0f;
        }
        //  jucaAnim.SetInteger("animJuca",1);

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
				yield return 0f;
			}
        }
         soundManager.startVoiceFX(audios[numtext]);
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
