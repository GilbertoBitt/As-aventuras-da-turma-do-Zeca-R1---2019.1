using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MEC;

public class ControlR6Apress : MonoBehaviour {

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
    public AudioClip[] audios;
    public SoundManager soundManager;

    public GameObject DesObj;
    public LoadManager loadManager;
    public GameObject BTFim;
    public bool checkTelacheia;

    void Start()
    {
        BtVoltar.SetActive(false);
        textBalao.text = textMy[0].ToUpper();       
        BtAvancar.SetActive(false);
        BtVoltar.SetActive(false);
        loadManager = FindObjectOfType<LoadManager>();
        Invoke("ActiveBt", 2f);

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

    void InvokeM()   {

       
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

        textBalao.text = textMy[nunbT].ToUpper();
        soundManager.startVoiceFX(audios[numtext]);
        controlAnim.SetInteger("posBoneco", nunbT);

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
            balaoG.transform.localScale = Vector3.Lerp(startOffsetB, endOffsetB, transitionCurve.Evaluate(s));
            yield return 0f;
        }
        soundManager.startVoiceFX(audios[numtext]);

        BtAvancar.SetActive(true);
        BtVoltar.SetActive(false);
    }
    public void KillAllCoroutines()
    {
        Timing.KillCoroutines();
    }

    public void ActiveBt()
    {
      
        BtAvancar.SetActive(true);
        BtVoltar.SetActive(false);
        controlAnim.SetInteger("posBoneco", 0);
        Timing.RunCoroutine(BalaoScaleStart());
    }
}

