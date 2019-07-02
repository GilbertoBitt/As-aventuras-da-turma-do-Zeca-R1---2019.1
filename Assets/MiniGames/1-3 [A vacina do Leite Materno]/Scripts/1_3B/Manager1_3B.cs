using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using com.csutil;
using MEC;
using DG.Tweening;
using MiniGames.Scripts;
using MiniGames.Scripts._1_3A;
using MiniGames.Scripts._1_3B;
using Sirenix.OdinInspector;
using TMPro;

public class Manager1_3B : OverridableMonoBehaviour {

	#region Vars

	public Button habilidadeInfoButton;
	[FoldoutGroup("1ª Ano")] public List<ItemWord1_3b> allWordsOfList;

	[FoldoutGroup("1ª Ano")] public List<ItemWord1_3b> choosenWords;
	[FoldoutGroup("1ª Ano")] public HabilidadeBNCCInfo habilidade1;
	[FoldoutGroup("2ª Ano")] public HabilidadeBNCCInfo habilidade2;
	[LabelText("Frases")]
	[FoldoutGroup("3ª Ano")]
	[AssetList(Path = "MiniGames/1-3 [A vacina do Leite Materno]/SO/3ª Ano/")]

	public List<ItemPhrase13B> phrase13Bs;
	[LabelText("Frases Selecionadas")]
	[FoldoutGroup("3ª Ano")]
	public List<ItemPhrase13B> choosenPhrase13Bs;

	[FoldoutGroup("3ª Ano")] public HabilidadeBNCCInfo habilidade3;

	private Camera mainCamera;
	public int anoLetivo = 1;
    public LogSystem _log;
	#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
	[SeparatorAttribute("Controles")]
	#endif
	public bool isPlaying;
	public float rotationLimit;
	public int foodQuestionTodo;
	public int foodQuestionMade;
	public List<QuestionFoodGroup1_3B> questionFood = new List<QuestionFoodGroup1_3B> ();
	public int rightIndexFood;
	public FoodItem1_3B rightFoodItem;
	public BubbleFood1_3B rightBubbleFood;
	public int scoreAmountPerRight;
    public GameObject interagindoPanel;
    public PanelDesafioControl interagindoManager;

	#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
	[SeparatorAttribute("Pontuação")]
	#endif
	public int scoreAmount;
	public Text scoreText;
	public float scoreIncreaseDuration;

	#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
	[SeparatorAttribute("Grupos de Alimentos")]
	#endif
	public FoodGroup currentFoodGroup = FoodGroup.none;
	public FoodItem1_3B[] frutasList = new FoodItem1_3B[0];
	public int frutasTimes = 1;
	public FoodItem1_3B[] verdurasList = new FoodItem1_3B[0];
	public int verdurasTimes = 1;
	public FoodItem1_3B[] pratosList = new FoodItem1_3B[0];
	public int pratosTimes = 1;
	public FoodItem1_3B[] bebidasList = new FoodItem1_3B[0];
	public int bebidasTimes = 1;

	#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
	[SeparatorAttribute("Canon")]
	#endif

	public Transform canonTransform;
	public Transform canonShootPosition;
	public Vector3 startPosCannon;
	private float oldCanonXPos;
	public bool canShoot=false;
	public float delayReturnCannon;

	#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
	[SeparatorAttribute("Bullets")]
	#endif
	public Queue<BulletManager1_3B> poolOfBullet = new Queue<BulletManager1_3B> ();
	public int poolBulletSize;
	public GameObject bulletPrefab;
	public float bulletSpeed;

	#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
	[SeparatorAttribute("Bubbles")]
	#endif
	public BubbleFood1_3B[] bubbleSprites;
	public float delayFadeIn = 3f;
	public float gotoCenterDelay = 1f;
	public Transform positionCenter;

	#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
	[SeparatorAttribute("UI")]
	#endif
	public TextMeshProUGUI textFoodName;
	public float textFoodFadeInDelay;
	public float textFoodFadeOuDelay;
    public Image fadeInOutImg;
    public float fadeInDuration;
    public AnimationCurve FadeInImgCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);

    public GameObject[] desactiveObjects;
    public GameObject[] activeGameObjects;
    public Manager1_3A oldManager;
    public Vector3 defaulCameraPos;
    public GameObject animLevel;
    Animator anim_animLevel;
    public changeLevel changeLevel2;
    public PauseManager pausemanager;
    public Button buttonPause;
    public RectTransform topLevel;
    public GameObject luz;
    public Vector2 topLevelG;
    public GameObject tutorG;
    public TutorVacine tutorVacine2;
    bool checkDitadita;
    bool checkDitadita2;
    public GameObject painelTeclado2;
    public ControlTecladoTutor ControlTecladoTutor2;
    public bool checkTutorteclado;
    public BoxCollider2D[] bolhasDid;
    bool checkTutorteclado1;
#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
    [SeparatorAttribute("Curves")]
	#endif
	public AnimationCurve fadeInCurveText = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);

#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
    [SeparatorAttribute("Sounds")]
#endif
    public AudioClip[] soundFXs = new AudioClip[0];
    public SoundManager soundM;

    public Cannon1_3B cannonManager;
    public ControlSomTutor ControlSomTutor2;

    public GameObject canudo;

    public bool didadMovel;
    public Image tutorialImageComponent;
    [ReadOnly] public InfoSkillWindow infoSkillWindow;
    #endregion

    // Use this for initialization
     void Start() {
        canudo = cannonManager.gameObject;
        ControlSomTutor2 = GetComponent<ControlSomTutor>();
        _log = GetComponent<LogSystem>();
        infoSkillWindow = FindObjectOfType<InfoSkillWindow>();
        habilidadeInfoButton.AddOnClickAction(x =>
        {
	        switch (anoLetivo)
	        {
		        case 1:
			        infoSkillWindow.ShowWindowInfo(habilidade1);
			        break;
		        case 2:
			        infoSkillWindow.ShowWindowInfo(habilidade2);
			        break;
		        case 3:
			        infoSkillWindow.ShowWindowInfo(habilidade3);
			        break;
	        }
        });
     }

    void StartGame () {
        // topLevelG = topLevel.offsetMax;Timing.RunCoroutine(StartDidatica());
        // tutorVacine2 = tutorG.GetComponent<TutorVacine>();
        topLevel.offsetMax = new Vector2(topLevel.offsetMax.x, 1633.984f);
        luz.SetActive(false);
        mainCamera = Camera.main;
        anim_animLevel = animLevel.GetComponent<Animator>();
        changeLevel2 = animLevel.GetComponent<changeLevel>();
        StartFoodQuiz ();
        mainCamera.transform.position = defaulCameraPos;
        mainCamera.orthographicSize = 5f;
        this.enabled = true;

    }

	public void OnCannonPressed(){
		if (!canShoot || didadMovel) return;
		var mousePos = mainCamera.ScreenToWorldPoint (Input.mousePosition);
		var newPos = canonTransform.position;
		newPos.x = mousePos.x;
		if (newPos.x >= -8f && newPos.x <= 8f) {
			canonTransform.position = newPos;
		}
	}



	public void OnCannonReleased(){
		if (!canShoot || poolOfBullet.Count < 1) return;
		BulletManager1_3B thisBullet = poolOfBullet.Dequeue();
		Vector3 dir = canonShootPosition.position - canonTransform.position;
		thisBullet.ShootBulletOnDirection(dir.normalized);
	}

#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN || UNITY_STANDALONE
    public override void UpdateMe()
    {
	    //Debug.Log("Updating", this);
	    if (!canShoot || checkTutorteclado != false) return;
	    //Debug.Log("Controle Override.", this);
        if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow)) {
	        //Debug.Log("Indo para a Direita.", this);
	        Vector3 newPos = canonTransform.position;
	        if (newPos.x >= -8f) {
		        newPos.x -= .2f;
	        }
	        canonTransform.position = newPos;
        } else if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow)) {
	        //  Debug.Log("Indo para a Esquerda.", this);
	        Vector3 newPos = canonTransform.position;
	        if (newPos.x <= 8f) {
		        newPos.x += .2f;
	        }
	        canonTransform.position = newPos;
        } else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space)) {
	        cannonManager.DownKey();
        } else if (Input.GetKeyUp (KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.Space)) {
	        cannonManager.UpKey();
        }

    }
#endif

	public bool ContainsLetter(ref List<ItemWord1_3b> listRef, ItemWord1_3b wordSelected)
	{
		foreach (var item in listRef)
		{
			if (item.startLetter == wordSelected.startLetter)
			{
				return true;
			}
		}

		return false;
	}

    public void StartFoodQuiz(){
	    anoLetivo = GameConfig.Instance.GetAnoLetivo();
		startPosCannon = canonTransform.position;
		LoadBulletPool ();
		allWordsOfList.Shuffle();
		switch (anoLetivo)
		{
			case 1:
				var temp = new List<ItemWord1_3b>();
				for (int i = 0; i < 4; i++)
				{
					ItemWord1_3b itemSelected = null;
					do
					{
						itemSelected = allWordsOfList.GetRandomValue();
					} while (ContainsLetter(ref temp, itemSelected));
					temp.Add(itemSelected);
				}
				choosenWords = temp;
				UpdateWords();
				break;
			case 2:
				questionFood.Clear ();
				questionFood = randomQuestionFoods ();
				//foodQuestionTodo = questionFood.Count;
				foodQuestionMade = 0;
				UpdateFoods ();
				break;
			case 3:
				phrase13Bs.Shuffle();
				choosenPhrase13Bs = phrase13Bs.Take(foodQuestionTodo).ToList();
				UpdatePhrase();
				break;
		}



		textFoodName.DOFade(1f, textFoodFadeInDelay);
		textFoodName.transform.DOScale(Vector3.one, textFoodFadeInDelay);
        if(!didadMovel)
	        canShoot = true;

	}

    public void UpdatePhrase(){
	    rightIndexFood = Random.Range (0, 4);

	    textFoodName.text = $"{choosenPhrase13Bs[foodQuestionMade].question}";
	    choosenPhrase13Bs[foodQuestionMade].wrongAlternatives.Shuffle();
	    choosenPhrase13Bs[foodQuestionMade].correctAlternatives.Shuffle();
	    var tempList = choosenPhrase13Bs[foodQuestionMade].wrongAlternatives.Take(3).ToList();
	    tempList.Add(choosenPhrase13Bs[foodQuestionMade].correctAlternatives.First());
	    tempList.Shuffle();

	    for (int i = 0; i < 4; i++) {
		    bubbleSprites [i].UpdateFood (tempList[i]);
		    bubbleSprites [i].StartFadeIn(delayFadeIn);
	    }

	    rightBubbleFood = bubbleSprites.First(x => x.word == choosenPhrase13Bs[foodQuestionMade].correctAlternatives.First());

    }

	public void UpdateFoods(){
		rightIndexFood = Random.Range (0, 4);

		textFoodName.text = questionFood [foodQuestionMade].foods [rightIndexFood].foodName;
		rightFoodItem = questionFood [foodQuestionMade].foods [rightIndexFood];
		rightBubbleFood = bubbleSprites [rightIndexFood];

		for (int i = 0; i < 4; i++) {
			bubbleSprites [i].UpdateFood (questionFood [foodQuestionMade].foods [i]);
			bubbleSprites [i].StartFadeIn(delayFadeIn);
		}
	}

	public void UpdateWords(){
		rightIndexFood = Random.Range (0, 4);

		textFoodName.text = $"Atire no alimento que começa com a letra {choosenWords[foodQuestionMade].startLetter}";

		var tempList = allWordsOfList.Where(x => x.startLetter != choosenWords[foodQuestionMade].startLetter).Take(3).ToList();
		tempList.Add(choosenWords[foodQuestionMade]);
		tempList.Shuffle();


		for (int i = 0; i < 4; i++) {
			bubbleSprites [i].UpdateFood (tempList[i]);
			bubbleSprites [i].StartFadeIn(delayFadeIn);
		}

		rightBubbleFood = bubbleSprites.First(x => x.word == choosenWords[foodQuestionMade]);

	}

	public void NextFoodsQuestion(){
		foodQuestionMade++;
        if (foodQuestionMade >= foodQuestionTodo) {
            interagindoPanel.SetActive(true);
            interagindoManager.scoreAmount = scoreAmount;
            interagindoManager.scoreInteragindo = scoreAmount;
            int tempCount = activeGameObjects.Length;
            for (int i = 0; i < tempCount; i++) {
                activeGameObjects[i].SetActive(false);
            }
            enabled = false;
        } else {
	        switch (anoLetivo)
	        {
		        case 1:
					UpdateWords();
			        break;
		        case 2:
			        UpdateFoods();
			        break;
		        case 3:
			        UpdatePhrase();
			        break;
	        }

	        textFoodName.DOFade(1f, textFoodFadeInDelay);
	        textFoodName.transform.DOScale(Vector3.one, textFoodFadeInDelay);
            canShoot = true;
        }
	}

	public void LoadBulletPool(){
		for (int i = 0; i < poolBulletSize; i++) {
			GameObject bullet = Instantiate(bulletPrefab,canonShootPosition.position * 1000f,Quaternion.identity,transform);
			BulletManager1_3B temp = bullet.GetComponent<BulletManager1_3B> ();
			temp.thisManager = this;
            temp.isOnPool = true;
            poolOfBullet.Enqueue (temp);
		}
	}

	public List<QuestionFoodGroup1_3B> randomQuestionFoods(){

		List<QuestionFoodGroup1_3B> resultList = new List<QuestionFoodGroup1_3B> ();

		for (int i = 0; i < frutasTimes; i++) {			
			FoodItem1_3B[] food = randomFoodFromGroup (FoodGroup.Frutas);
			QuestionFoodGroup1_3B qFood = new QuestionFoodGroup1_3B ();
			qFood.foods = food;
			resultList.Add (qFood);
		}

		for (int i = 0; i < verdurasTimes; i++) {
			FoodItem1_3B[] food = randomFoodFromGroup (FoodGroup.VerdurasVegetais);
			QuestionFoodGroup1_3B qFood = new QuestionFoodGroup1_3B ();
			qFood.foods = food;
			resultList.Add (qFood);
		}

		for (int i = 0; i < pratosTimes; i++) {
			FoodItem1_3B[] food = randomFoodFromGroup (FoodGroup.Pratos);
			QuestionFoodGroup1_3B qFood = new QuestionFoodGroup1_3B ();
			qFood.foods = food;
			resultList.Add (qFood);
		}

		for (int i = 0; i < bebidasTimes; i++) {
			FoodItem1_3B[] food = randomFoodFromGroup (FoodGroup.Bebidas);
			QuestionFoodGroup1_3B qFood = new QuestionFoodGroup1_3B ();
			qFood.foods = food;
			resultList.Add (qFood);
		}

		resultList.Shuffle ();
		return resultList;
	
	}

	public FoodItem1_3B[] randomFoodFromGroup(FoodGroup foodType){

		FoodItem1_3B[] result = new FoodItem1_3B[4];

		if (foodType == FoodGroup.Frutas) {
			result = randomFood (frutasList);
		} else if (foodType == FoodGroup.VerdurasVegetais) {
			result = randomFood (verdurasList);
		} else if (foodType == FoodGroup.Pratos) {
			result = randomFood (pratosList);
		} else if (foodType == FoodGroup.Bebidas) {
			result = randomFood (bebidasList);
		}
		result.Shuffle ();
		return result;
	}

	public FoodItem1_3B[] randomFood(FoodItem1_3B[] list){
		
		FoodItem1_3B[] result = new FoodItem1_3B[4];

		List<int> temp = UniqueRandom (0, list.Length).ToList ();
		for (int i = 0; i < 4; i++) {
			result [i] = list [i];
		}

		return result;
	}

	public enum FoodGroup {
		none = 0,
		Frutas,
		VerdurasVegetais,
		Pratos,
		Bebidas,
	}

	/// <summary>
	/// Returns all numbers, between min and max inclusive, once in a random sequence.
	/// </summary>
	IEnumerable<int> UniqueRandom(int minInclusive, int maxInclusive)
	{
		List<int> candidates = new List<int>();
		for (int i = minInclusive; i <= maxInclusive; i++)
		{
			candidates.Add(i);
		}
		System.Random rnd = new System.Random();
		while (candidates.Count > 0)
		{
			int index = rnd.Next(candidates.Count);
			yield return candidates[index];
			candidates.RemoveAt(index);
		}
	}

	public void OnBulletHit(BubbleFood1_3B bubble){
        soundM.startSoundFX(soundFXs[0]);
        CorrectionOfFood (bubble);
		canShoot = false;

	}

	public void StopLooping(){
		for (int i = 0; i < 4; i++) {
			bubbleSprites [i].isLooping = false;
		}
	}

	public void CorrectionOfFood(BubbleFood1_3B userChoiseFood){
		StopLooping ();
        animLevel.SetActive(true);
        anim_animLevel.enabled = true;



        if (userChoiseFood == rightBubbleFood) {
	        var currentScore = scoreAmount;
	        var newscore = scoreAmount + scoreAmountPerRight;
	        scoreText.DOTextInt(currentScore, newscore, 1f).OnComplete(() =>
	        {
		        scoreAmount = newscore;
		        Debug.Log($"score updated to {newscore}");

	        });
            Invoke("CorrectionSucess", 0.6f);
            _log.SaveEstatistica(3, 1, true);
            userChoiseFood.StartCorrectcenter (gotoCenterDelay);
			Timing.KillCoroutines ("IncreaseEffectScore");
//			Timing.RunCoroutine (scoreIncrease (scoreAmountPerRight), "IncreaseEffectScore");
            _log.AddPontosPedagogica(scoreAmountPerRight);

        } else {
            Invoke("CorrectionFail", 0.6f);
            _log.SaveEstatistica(3, 1, false);
            rightBubbleFood.StartCorrectcenter (gotoCenterDelay);
			userChoiseFood.MakeItRed ();
        }

        if (anoLetivo != 2)
        {
	        rightBubbleFood.textItem.DOFade(1f, .3f).SetDelay(gotoCenterDelay).OnComplete(() => Timing.RunCoroutine(EndQuestion()));
        }
        else
        {
	        Timing.RunCoroutine (EndQuestion());
        }

//		Timing.RunCoroutine (EndQuestion());
	}

    public void CorrectionSucess() {
        soundM.startSoundFX(soundFXs[1]);
        changeLevel2.startCerto("Parabéns! Você acertou!");
    }

    public void CorrectionFail() {
        soundM.startSoundFX(soundFXs[2]);
        changeLevel2.startErrado("Você errou! O certo era:");
    }

    IEnumerator<float> EndQuestion(){
		yield return Timing.WaitForSeconds (3f);
		//Timing.RunCoroutine (ReturnCannon ());
        canonTransform.DOMove(startPosCannon, delayReturnCannon, false);
        //Timing.RunCoroutine (FadeOuText ());
        textFoodName.DOFade(0f, textFoodFadeOuDelay);
        textFoodName.transform.DOScale(Vector3.zero, textFoodFadeOuDelay);
        FadeOutBubbles(delayFadeIn);
        yield return Timing.WaitForSeconds (delayFadeIn + 0.2f);
		NextFoodsQuestion ();
	}

    public void FadeOutBubbles(float _delay) {
        for (int i = 0; i < 4; i++) {
            bubbleSprites[i].StartFadeOut(_delay);
        }
    }



	IEnumerator<float> FadeOuText(){
		float times = 0.0f;

		Vector3 scaleStart = new Vector3 (1f, 1f, 1f);
		Vector3 scaleEnd = textFoodName.transform.localScale;

		var color = textFoodName.color;
		Color startColor = color;
		startColor.a = 1f;
		Color colorEnd = color;
		colorEnd.a = 0f;

		while (times < textFoodFadeOuDelay)
		{
			times += Time.deltaTime;
			float s = times / textFoodFadeOuDelay;

			textFoodName.transform.localScale = Vector3.Lerp (scaleStart, scaleEnd, fadeInCurveText.Evaluate (s));
			textFoodName.color = Color.Lerp (startColor, colorEnd, fadeInCurveText.Evaluate (s));

			yield return Timing.WaitForOneFrame;
        }
	}

	public IEnumerator<float> scoreIncrease(int increase){

		//Debug.Log ("Start Increase Points");
		float times = 0.0f;
		int startPoints = scoreAmount;
		int scoreT = scoreAmount + increase;
		scoreAmount += increase;
		while (times < scoreIncreaseDuration)
		{
			times += Time.deltaTime;
			float s = times / scoreIncreaseDuration;

			int scory = (int)Mathf.Lerp (startPoints, scoreT, fadeInCurveText.Evaluate (s));
			scoreText.text = scory.ToString ();
			yield return Timing.WaitForOneFrame;
        }	

	}

    public void RunDidatica() {
      //  tutorVacine2.gameObject.SetActive(true);
         ControlSomTutor2.numTutor = 6;
       // ControlSomTutor2.numTutor = 1;
        checkDitadita2 = true;

        checkDitadita2 = false;
        _log.StartTimerLudica(false);
        SequenceToChangeDidatica(false);
     
//        if (PlayerPrefs.HasKey("TutorV_IniD") == false || PlayerPrefs.HasKey("TutorV_IniD") == true) {
//            /*
//            if (Application.platform == RuntimePlatform.WindowsPlayer == true && (PlayerPrefs.HasKey("TutorV_IniD") == false)) {
//
//                checkTutorteclado = true;
//                PlayerPrefs.SetInt("TutorV_IniD", 1);
//                ControlTecladoTutor2.didatica = true;
//                for (int i = 0; i < bolhasDid.Length; i++) {
//                    bolhasDid[i].enabled = false;
//                }
//
//            }
//            */
//             if (Application.platform == RuntimePlatform.WindowsPlayer == false && (PlayerPrefs.HasKey("TutorV_IniD") == false)) {
//                oldManager.panelTeclado.SetActive(true);
//                // checkTutorteclado = true;
//                didadMovel = true;
//                PlayerPrefs.SetInt("TutorV_IniD", 1);
//
//                foreach (var t in ControlTecladoTutor2.tecla)
//                {
//	                t.gameObject.SetActive(false);
//                }
//
//            }
//            tutorVacine2.btTelao.SetActive(false);
//            tutorVacine2.pularTex.SetActive(false);
//            //PlayerPrefs.SetInt("TutorV_IniD", 1);
//            tutorG.SetActive(true);
//            tutorVacine2.textTutor.text = "Agora, veja o nome do alimento e atire com o canudo na imagem correta.";
//            //SomTutor(1);
//            tutorVacine2.Som1();
//            tutorVacine2.Onprof();
//            tutorialImageComponent.enabled = true;
//            tutorVacine2.iniciarG1.SetActive(true);
//            tutorVacine2.iniciarG2.SetActive(false);
//            _log.StartTimerLudica(false);
//            Timing.RunCoroutine(StartDidatica());
//			SequenceToChangeDidatica(true);
//            checkDitadita = true;
//
//        } else {
//
//            checkDitadita2 = false;
//            _log.StartTimerLudica(false);
//            SequenceToChangeDidatica(false);
//            //Timing.RunCoroutine(StartDidatica());
//
//
//        }

      
    }

    void SomTutor(int recNum) {
        ControlSomTutor2.numTutor = recNum;
        ControlSomTutor2.SomTutor();
        //y//ield return Timing.WaitForSeconds(0.2f);


    }

    public void teclaDidadica () {
        if (ControlTecladoTutor2.didatica == true) {
            painelTeclado2.SetActive(true);
        }

        if (didadMovel != true) return;
        canudo.SetActive(true);
        ControlTecladoTutor2.textTutor.SetActive(true);
        canudo.GetComponent<Animator>().enabled = true;
        canudo.GetComponent<Animator>().SetBool("Tutor", true);

    }

    public void iniDid() {
        checkDitadita2 = false;
        if (checkDitadita) {

            Timing.RunCoroutine(StartDidatica());

        }

    }

    public void ChangeToDidatica() {
	    Timing.RunCoroutine(StartDidatica());
    }

    public void EndChangeDidatica() {
        //fadeInOutImg.gameObject.SetActive(false);
        //fadeInOutImg.raycastTarget = false;
        if (!checkDitadita2) {
            StartGame();
        }
        _log.StartTimerDidatica(true);
    }

    public void OnBeforeStartChangeDidatica() {
        fadeInOutImg.gameObject.SetActive(true);
        fadeInOutImg.raycastTarget = true;
    }

    public void SequenceToChangeDidatica(bool tutorial) {

        oldManager.circleFader.maskValue = 0f;
        Sequence startDidaticaS = DOTween.Sequence();
        /*startDidaticaS.AppendCallback(OnBeforeStartChangeDidatica);
        if (tutorial == false) {
            startDidaticaS.Append(fadeInOutImg.DOFade(1f, fadeInDuration));
        }*/
        startDidaticaS.AppendCallback(() => this.ChangeToDidatica());
        startDidaticaS.Append(fadeInOutImg.DOFade(0f, fadeInDuration));
        startDidaticaS.AppendCallback(() => this.EndChangeDidatica());
        startDidaticaS.Play();
    }

    private IEnumerator<float> StartDidatica() {
        foreach (var t in desactiveObjects)
        {
	        t.SetActive(false);
        }

        foreach (var t in activeGameObjects)
        {
	        t.SetActive(true);
        }
        tutorialImageComponent.enabled = true;
        oldManager.enabled = false;
        //

        scoreAmount = oldManager.ScoreAmount;
        scoreText.text = scoreAmount.ToString();
       // yield return Timing.WaitForOneFrame;
        pausemanager.btPause = buttonPause.gameObject;

        yield return Timing.WaitForOneFrame;
        
        if (!checkDitadita2) {
            StartGame();
        }
        _log.StartTimerDidatica(true);

    }

    IEnumerator<float> ReturnCannon(){
		float times = 0.0f;

		Vector3 posEnd = startPosCannon;
		Vector3 posStart = canonTransform.position;	

		while (times < delayReturnCannon)
		{
			times += Time.deltaTime;
			float s = times / delayReturnCannon;

			canonTransform.position = Vector3.Lerp (posStart, posEnd, fadeInCurveText.Evaluate (s));

			yield return Timing.WaitForOneFrame;
		}
	}

    public void KillCoroutines() {
        Timing.KillCoroutines();
        StopAllCoroutines();
    }
}

[System.Serializable]
public class FoodItem1_3B{
    public string foodName;
    public Sprite spriteItem;	
}

[System.Serializable]
public class QuestionFoodGroup1_3B{
	public FoodItem1_3B[] foods = new FoodItem1_3B[4];
	//FoodGroup
}


