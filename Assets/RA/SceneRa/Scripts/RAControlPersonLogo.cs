using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MEC;

public class RAControlPersonLogo : MonoBehaviour {

    // 0 zeca
    // 1 Tati
    // 2 Paulo
    // 3 Manu
    // 4 Joao
    // 5 Bia

    public GameObject[] personG;
    public RectTransform setaBalao;
    public GameObject[] posEnd;
    public GameObject[] btProx;
    public GameObject btProxChamar;
    public GameObject btVoltar;
    public GameObject balaoG;
    public Text textChamar;
    Transform personAtual;
   
    Color imageColor;
    public Animator[] animCorrendo;
    public Vector3[] offsetFollow;
    public Vector3[] defaultOffset;
    public Vector3 offsetFollowB;
    public Vector3 defaultOffsetB;
    public float transitionDuration;
    public float transitionDurationB;
    public AnimationCurve transitionCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
    public int numbPerson;
    public int numbText;
    public float durationOfFade = 1.0f;
    public AnimationCurve fadeCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
    public Text textFrases;
    public string[] frasesText;
    Color colorAtual;
    public Color colorTrans;
    bool checkClick;
    bool checkClick2;
    bool checkClick3;
    public ExpressFacialZeca[] ExpressFacial;
    public bool chamarPerson;
    int chamarperson;
    public float[] posSeta;
    bool checkIni;
    bool checkVoltar;
    public GameObject DesObj;
    public LoadManager loadManager;
    public GameObject BTFim;

    public AudioClip[] audios;
    public SoundManager soundManager;
    public bool checkTelacheia;
    bool checkFalarin;

    static readonly int NumbARHash = Animator.StringToHash("NumbAR");

    void Start() {
        //textChamar.text = textNome[numbPerson];
        
        setaBalao.anchoredPosition =new Vector2(posSeta[0], setaBalao.anchoredPosition.y);
        int tempCount = personG.Length;
        for (int i = 0; i < tempCount; i++)
        {
            ExpressFacial[i] = personG[i].GetComponent<ExpressFacialZeca>();
        }

        colorAtual = textFrases.color;
     
        textFrases.color = colorTrans;
        textFrases.text = frasesText[numbText].ToUpper();
       
        if (!checkTelacheia) {
			soundManager = GameObject.FindWithTag ("SoundManager").GetComponent<SoundManager> ();
			soundManager.startVoiceFX(audios[numbText]);
        }

        tempCount = personG.Length;
        for (int i = 0; i < tempCount; i++)
        {
            animCorrendo[i] = personG[i].GetComponent<Animator>();
        }

        tempCount = personG.Length;

        for (int i = 0; i < tempCount; i++) {
			offsetFollow[i] = personG[i].transform.localPosition;
            defaultOffset[i] = posEnd[i].transform.localPosition;
        }

        /*offsetFollow[0] = personG[0].transform.position;
        offsetFollow[1] = personG[1].transform.position;
        offsetFollow[2] = personG[2].transform.position;
        offsetFollow[3] = personG[3].transform.position;
        offsetFollow[4] = personG[4].transform.position;
        offsetFollow[5] = personG[5].transform.position;

        defaultOffset[0] = posEnd[0].transform.position;
        defaultOffset[1] = posEnd[1].transform.position;
        defaultOffset[2] = posEnd[2].transform.position;
        defaultOffset[3] = posEnd[3].transform.position;
        defaultOffset[4] = posEnd[4].transform.position;
        defaultOffset[5] = posEnd[5].transform.position;
        */


        animCorrendo[0].enabled = true;
        btProx[numbPerson].SetActive(false);
        btProxChamar.SetActive(false);
        btVoltar.SetActive(false);
        StartCoroutine(MoverPerson(numbPerson));
        loadManager = FindObjectOfType<LoadManager>();
        Timing.RunCoroutine(TimeButton());
        if (!checkTelacheia) {
			soundManager = GameObject.FindWithTag ("SoundManager").GetComponent<SoundManager> ();
        }
    }

    public void Mfim() {
        KillAllCoroutines();
        Invoke("InvokeM", 1f);
        DesObj.SetActive(false);
    }

    void InvokeM()
    {
        loadManager.gameObject.SetActive(true);
        loadManager.LoadAsync("selectionMinigames");

    }

    public IEnumerator MoverPerson(int personN)
    {
        if (!checkClick)
        {
            checkClick = true;
            personN = numbPerson;
        }
        if (numbPerson < 6)
        {
            personG[personN].SetActive(true);
            personAtual = personG[personN].GetComponent<Transform>();
            animCorrendo[personN].enabled = true;
            if (personG[personN].activeInHierarchy == true && animCorrendo[personN].enabled == true) {
               
                animCorrendo[personN].SetInteger(NumbARHash, 0);
            }
           
            Vector3 startOffset = offsetFollow[personN];
            Vector3 endOffset = defaultOffset[personN];


            float times = 0.0f;
          
            while (times < transitionDuration)
            {
                times += Time.deltaTime;
                float s = times / transitionDuration;
                personG[personN].transform.localPosition = Vector3.Lerp(startOffset, endOffset, transitionCurve.Evaluate(s));
                yield return Timing.WaitForOneFrame;
            }

            btProx[numbPerson].SetActive(true);
            btProxChamar.SetActive(true);
           // btVoltar.SetActive(true);
            if (personG[personN].activeInHierarchy == true && animCorrendo[personN].enabled == true) {
                animCorrendo[personN].SetInteger(NumbARHash, 1);
            }
           
            textFrases.enabled = true;
            Timing.RunCoroutine(FadeText0a1(false));
            Timing.RunCoroutine(BalaoScale0a1());
            if (!checkClick2)
            {
                checkClick2 = true;
                ExpressFacial[numbPerson].checkFalando = true;

            }
            else {
                float personAtualValor = personAtual.transform.localScale.x;
                personAtualValor = personAtualValor * -1;
                personAtual.transform.localScale = new Vector2(personAtualValor, personAtual.transform.localScale.y);
            }
           

        }       
    }


    public void voltarBt() {
        checkVoltar = true;
        numbText = numbText - 1;
        Timing.RunCoroutine(FadeText0a1(true));
        
        //Timing.RunCoroutine(BalaoScale1a0(checkVoltar));
       
        
    }
    
    public IEnumerator<float> BalaoScale0a1()        
    {
        Vector3 startOffsetB = offsetFollowB;
        Vector3 endOffsetB = defaultOffsetB;
        float timesB = 0.0f;
        while (timesB < transitionDurationB)
        {
            timesB += Time.deltaTime;
            float s = timesB / transitionDurationB;
            balaoG.transform.localScale = Vector3.Lerp(startOffsetB, endOffsetB, transitionCurve.Evaluate(s));
            yield return Timing.WaitForOneFrame;
        }
      
    }
    public IEnumerator<float> BalaoScale1a0()
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
            setaBalao.anchoredPosition = new Vector2(posSeta[numbPerson], setaBalao.anchoredPosition.y);
       

    }
  


    public void ChamarPerson() {      
     
        animCorrendo[numbPerson].SetInteger(NumbARHash, 2);       
        btProxChamar.SetActive(false);
        btVoltar.SetActive(false);
        Timing.RunCoroutine(TimeButton());          
    }
    
    IEnumerator<float> TimeButton() {
        if (!checkClick3)
        {
            checkClick3 = true;
       }
        else
        {
            numbText = numbText + 1;
        }

        if (numbText == 2 || numbText == 4 || numbText == 6 || numbText == 8 || numbText == 10)
        {
            textFrases.enabled = false;
            Timing.RunCoroutine(BalaoScale1a0());
            numbPerson = numbPerson + 1;
            ExpressFacial[numbPerson-1].checkFalando = false;
            ExpressFacial[numbPerson].checkFalando = true;
            StartCoroutine(MoverPerson(numbPerson));
            yield return Timing.WaitForSeconds(2f);
            if (numbText < 11)
            {
                btProxChamar.SetActive(true);
            }
            else {
              
            }
        }
        else {
            Timing.RunCoroutine(FadeText0a1(false));
         
            yield return Timing.WaitForSeconds(2f);
            if (numbText < 11 && !checkIni) {
                if (btProxChamar != null) {
                    btProxChamar.SetActive(true);
                }
                checkIni = true;
            }
            else if (numbText < 11 && checkIni)
            {
                if (btProxChamar != null) {
                    btProxChamar.SetActive(true);
                }
                btVoltar.SetActive(true);
            }
                
        }
       // soundManager.startVoiceFX(audios[numbText]);

    }
    IEnumerator<float> FadeText1a0()
    {
        float times = 0.0f;
        if (numbPerson > 0) {
            while (times < durationOfFade)
        {
            times += Time.deltaTime;
            float s = times / durationOfFade;
            textFrases.color = Color.Lerp(colorAtual, colorTrans, fadeCurve.Evaluate(s));
        
            yield return Timing.WaitForOneFrame;
        }
     
        }

    }
    IEnumerator<float> FadeText0a1(bool checkVolta3)
    {
        if (!checkVolta3)
        {
            textFrases.text = frasesText[numbText].ToUpper(); 
        }
        else {
            btVoltar.SetActive(false);
            textFrases.text = frasesText[numbText].ToUpper();
        }
            textFrases.text = frasesText[numbText].ToUpper();
        if (checkFalarin) {
            soundManager.startVoiceFX(audios[numbText]);
        } else {
            checkFalarin = true;
        }
       
        float times = 0.0f;

        times = 0.0f;
        while (times < durationOfFade)
        {
            times += Time.deltaTime;
            float s = times / durationOfFade;
			if (textFrases != null) {
				textFrases.color = Color.Lerp (colorTrans, colorAtual, fadeCurve.Evaluate (s));        
			}
            yield return Timing.WaitForOneFrame;
        }
        if (checkVolta3) {
            btProxChamar.SetActive(true);
        }
        if (numbText > 10)
        {
            yield return Timing.WaitForSeconds(3f);
            BTFim.SetActive(true);
        }
       
    }


    public void KillAllCoroutines()
    {
        Timing.KillCoroutines();
    }
    }
