using System;
using System.Collections;
using System.Collections.Generic;
using MEC;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Sirenix.OdinInspector;
using UniRx;
using Random = UnityEngine.Random;

public class Manager_1_2B : OverridableMonoBehaviour
{
	[TabGroup("Geral")] public GameConfig config;
	[TabGroup("Geral")] public int anoLetivo;
	[TabGroup("1ª Ano")] public HabilidadeBNCCInfo Habilidade1;
	[TabGroup("1ª Ano")] public HabilidadeBNCCInfo Habilidade1_2;
	[TabGroup("2ª Ano")] public Dictionary<SpacialForms, List<Sprite>> spatialFormsSprites = new Dictionary<SpacialForms, List<Sprite>>();

	[TabGroup("2ª Ano")] public SpacialForms selectedForm = SpacialForms.None;
	[TabGroup("2ª Ano")] public HabilidadeBNCCInfo Habilidade2;
	[TabGroup("3ª Ano")] public PlaneFigures selectedPlaneFigure = PlaneFigures.None;
	[TabGroup("3ª Ano")] public List<Color> Colors;
	[TabGroup("3ª Ano")] public Dictionary<PlaneFigures, List<Sprite>> planeFiguresSprites = new Dictionary<PlaneFigures, List<Sprite>>();
	[TabGroup("3ª Ano")] public HabilidadeBNCCInfo Habilidade3;
	[HeaderAttribute("References")]
	public Manager_1_2A manager;
	public List<CirclesOnPanel_1_2B> panelCircles = new List<CirclesOnPanel_1_2B>();
	public Button rightBT;
	public Button leftBT;
	public RectTransform rectImage;
	public GameObject panel02;
	public List<GameObject> deactiveThis = new List<GameObject>();
	public GameObject nextButtonRef;
	public Image fadeImage;
	public Text comandText;
	public Image iconFromText;
	public FinalScore scoreEnd;
	public Canvas scoreEndCanvas;
	public Text scoreText;
	public Transform scoreWindowPos;
	public Text scoreWindowsText;
	public SoundManager soundManager;
	public changeLevel congratsPanel;
	public GameObject highlightCOmp;
	public LogSystem log;
	[HeaderAttribute("Variable")]
	public List<GeometryPlaces_1_2B> userPicks = new List<GeometryPlaces_1_2B>();
	public List<geometryForm> formList = new List<geometryForm>();
	public List<GeometryPlaces_1_2B> choosenPlaces = new List<GeometryPlaces_1_2B>();
	public List<GeometryPlaces_1_2B> AllPlaces = new List<GeometryPlaces_1_2B>();
	public float speedMovingImage;
	public bool isRight;
	public bool isLeft;
	public int needFind = 5;
	public int hasFound = 0;
	public bool canBePlayed = false;
	public Color rightColor;
	public Color wrongColor;
	public Sprite rightCheckSprite;
	public Sprite wrongCheckSprite;
	[RangeAttribute(0.0001f,2f)]
	public float waitTimeCorrection = 0.8f;
	public Sprite[] spritesIcon;
	public int dificult = 0;
	public Vector3 textScoreOriginalPos;
	public Color originalColor;
	public int scoreAmount;
	public int maxRandom = 8;
	public int minRandom = 3;
	public Transform circleParent;
	public GameObject circlePrefab;
	public List<GameObject> circleInstances = new List<GameObject>();
	public List<AudioClip> audios = new List<AudioClip>();
	public int minOtherSprites = 4;
	public int maxOtherSprites = 6;
	[HeaderAttribute("DB Of Itens")]
	public List<Sprite> squareSpritesList = new List<Sprite>();
	public List<Sprite> circleSpritesList = new List<Sprite>();
	public List<Sprite> triangleSpritesList = new List<Sprite>();
	public List<Sprite> rectangleSpritesList = new List<Sprite>();

	[HeaderAttribute("Animation")]
	public float fadeInDuration = 1.0f;
	public AnimationCurve fadeInCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
	public float fadeOutDuration = 1.0f;
	public AnimationCurve fadeOutCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
	public float fadeinScoreDuration = 1.0f;
	public AnimationCurve fadeinScoreCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
	public float scoreMoveDuration = 1.0f;
	public AnimationCurve scoreMoveCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
	public float increaseScoreDuration = 1.0f;
	public AnimationCurve increaseScoreCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
	public float scoreTextFadeDuration = 1.0f;
	public float scoreTextMoveDuration = 1.0f;
	public AnimationCurve scoreTextFadeCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
	public GameObject panelDesafio;
	public Animator panelDesafioAnimator;
	public PanelDesafioControl panelDesafioController;
	bool checkpanelDes;
	public GameObject panelDidatica;
	public GameObject panelOld;
	[HeaderAttribute("DB Of Itens")]
	public AudioClip[] clipsAudio;

	[HeaderAttribute("On Begin")]
	public UnityEvent OnGameStart;

	[HeaderAttribute("LOG")]
	public int idHabilidade;
	public int idDificuldade;

#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
    [SeparatorAttribute("Increase Text Effect")]
#endif
    public AnimationCurve increaseScoreCurves = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
    public float increaseDurationScoreEffect = 0.3f;
    StringFast _string = new StringFast();

#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
    [SeparatorAttribute("Increase Text Effect")]
#endif
    public AnimationCurve textFormCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
    public float textFormCurveDuration = 1f;
    public TextMeshProUGUI textForm;
    public Color defaultFormTextColor;
    static readonly int emCenaHash = Animator.StringToHash("emCena");
    public Vector2 LimitSlider;
    public Image fundoProf;
    public Sprite ImageFundo;
    private InfoSkillWindow _infoSkillInfo;
    [TabGroup("Geral")]
    public Button infoSkillButton;

    void Start()
    {
	    config = GameConfig.Instance;
	    
//	    anoLetivo = GameConfig.Instance.GetAnoLetivo();
		panelDesafioAnimator = panelDesafio.GetComponent<Animator>();

		if (_infoSkillInfo == null)
		{
			_infoSkillInfo = FindObjectOfType<InfoSkillWindow>();
		}

		infoSkillButton.onClick.AsObservable().Subscribe(unit =>
		{
			switch (anoLetivo)
			{
				case 1:
					_infoSkillInfo.ShowWindowInfo(Habilidade1, Habilidade1_2);
					break;
				case 2:
					_infoSkillInfo.ShowWindowInfo(Habilidade2);
					break;
				case 3:
				default:
					_infoSkillInfo.ShowWindowInfo(Habilidade3);
					break;
			}
		});
	}

	// Use this for initialization
	IEnumerator<float> StartGame () {
		soundManager.hasAmbientFX=false;
		checkpanelDes=false;
		fadeImage.gameObject.SetActive(true);
		fadeImage.DOFade(1f, fadeInDuration).SetEase(fadeInCurve);
//		float times = 0.0f;
//		if(fadeImage.color == Color.black){
//			while (times < fadeInDuration)
//			{
//				times += Time.deltaTime;
//				float s = times / fadeInDuration;
//
//				fadeImage.color = Color.Lerp (new Color (1f, 1f, 1f, 0f), Color.black, fadeInCurve.Evaluate (s));
//
//				yield return Timing.WaitForOneFrame;
//
//			}
//		}

		int temp = Random.Range(minRandom,maxRandom);

		nextButtonRef.SetActive(false);
		canBePlayed = false;
		needFind = temp;
		hasFound = 0;
		choosenPlaces.Clear();
		//choosenPlaces = AllPlaces;

		int amountInstance = circleInstances.Count;
		for (int i = 0; i < amountInstance; i++){
			Destroy(circleInstances[i]);
		}

		panelCircles.Clear();
		circleInstances.Clear();

		for (int i = 0; i < needFind; i++){
			GameObject circlePanel = Instantiate(circlePrefab,circleParent);
			circlePanel.transform.localScale = new Vector3(1,1,1);
			circleInstances.Add(circlePanel);
			panelCircles.Add(circlePanel.GetComponent<CirclesOnPanel_1_2B>());
		}


		for (int i = 0; i < AllPlaces.Count; i++){
			choosenPlaces.Add(AllPlaces[i]);
		}



		for (int i = 0; i < panelCircles.Count; i++){
			panelCircles[i].outlineImage.effectColor = Color.black;
			panelCircles[i].correctCompImage.color = Color.clear;
		}

		userPicks.Clear();
		//reset all places

		updateCircles();

		foreach (var places in AllPlaces)
		{
			places.resetConfig();
			places.form = geometryForm.none;
			places.spacialForm = SpacialForms.None;
			places.planeFigures = PlaneFigures.None;
		}

		
		AllPlaces.Suffle();

		switch (anoLetivo)
		{
			case 2:
				do
				{
					selectedForm = EnumExtensions.RandomEnumValue<SpacialForms>();
				} while (selectedForm == SpacialForms.None);

				foreach (var item in spatialFormsSprites)
				{
					item.Value.Suffle();
				}
				break;
			case 3:
				do
				{
					selectedPlaneFigure = EnumExtensions.RandomEnumValue<PlaneFigures>();
				} while (selectedPlaneFigure == PlaneFigures.None);

				break;
		}

		for (int i = 0; i < temp; i++){
			switch (anoLetivo)
			{
				case 1:
					AllPlaces[i].form = formList[dificult];
					AllPlaces[i].updateImage(returnList(formList[dificult])[i]);
					break;
				case 2:
					do
					{
						AllPlaces[i].spacialForm = selectedForm;
					} while (AllPlaces[i].spacialForm == SpacialForms.None);
					AllPlaces[i].updateImage(spatialFormsSprites[selectedForm][i]);
					break;
				case 3:
					do {
						AllPlaces[i].planeFigures = selectedPlaneFigure;
					} while (AllPlaces[i].planeFigures == PlaneFigures.None);

					var alreadySelected = new List<(Sprite, Color)>();
					(Sprite, Color) result;
					do
					{
						result = (planeFiguresSprites[selectedPlaneFigure].GetRandomValue(), Colors.GetRandomValue());
					} while (alreadySelected.Contains(result));
					alreadySelected.Add(result);
					AllPlaces[i].updateImage(result.Item1, result.Item2);
					break;
			}

			choosenPlaces.Remove(AllPlaces[i]);
		}

		choosenPlaces.Suffle();

		List<Sprite> otherSprites = new List<Sprite>();
		var otherSpatialForms = new List<(Sprite sprite, SpacialForms spacialForms)>();
		var otherFacialPlanes = new List<(Sprite sprite, PlaneFigures facialFigures)>();

		switch (anoLetivo)
		{
			case 1:
				for (int i = 1; i < 4; i++){
					for (int j = 0; j < returnList(formList[i]).Count; j++){
						if(formList[i] != formList[dificult]){
							otherSprites.Add(returnList(formList[i])[j]);
						}
					}
				}
				break;
			case 2:
				foreach (var item in spatialFormsSprites)
				{
					if (item.Key == selectedForm) continue;
					foreach (var sprite in item.Value)
					{
						otherSpatialForms.Add((sprite, item.Key));
					}
				}
				break;
			case 3:
				foreach (var item in planeFiguresSprites)
				{
					if (item.Key == selectedPlaneFigure) continue;
					foreach (var sprite in item.Value)
					{
						otherFacialPlanes.Add((sprite, item.Key));
					}
				}
				break;
		}


		otherSprites.Suffle();
		otherSpatialForms.Suffle();
		otherFacialPlanes.Suffle();


		int randomize = Random.Range(minOtherSprites,maxOtherSprites);
       

		for (int i = 0; i < randomize; i++){
			switch (anoLetivo)
			{
				case 1:
					choosenPlaces[i].updateImage(otherSprites[i]);
					choosenPlaces[i].form = VerifyImageForm(otherSprites[i]);
					break;
				case 2:
					choosenPlaces[i].updateImage(otherSpatialForms[i].sprite);
					choosenPlaces[i].spacialForm = otherSpatialForms[i].spacialForms;
					break;
				case 3:
					choosenPlaces[i].updateImage(otherFacialPlanes[i].sprite, Colors.GetRandomValue());
					choosenPlaces[i].planeFigures = otherFacialPlanes[i].facialFigures;
					break;
			}

        }



		canBePlayed = true;

		switch (anoLetivo)
        {
        	case 1:
	            _string.Clear();
	            _string.Append("Encontre ").Append(needFind).Append(" ").Append(returnName(formList[dificult]));
	            comandText.text = _string.ToString();
	            iconFromText.sprite = returnIcon(formList[dificult]);
        		break;
        	case 2:
	            _string.Clear();
	            _string.Append("Encontre ").Append(needFind).Append(" ").Append(GetSpacialFormName(selectedForm));
	            comandText.text = _string.ToString();
//	            iconFromText.sprite = returnIcon(formList[dificult]);
	            iconFromText.enabled = false;
        		break;
        	case 3:
	            _string.Clear();
	            _string.Append("Encontre ").Append(needFind).Append(" ").Append(GetSpacialFormName(selectedPlaneFigure));
	            comandText.text = _string.ToString();
//	            iconFromText.sprite = returnIcon(formList[dificult]);
	            iconFromText.enabled = false;
        		break;
        }

        fadeImage.DOFade(0f, fadeOutDuration).SetEase(fadeOutCurve);
        fadeImage.gameObject.SetActive(false);
		yield return Timing.WaitForOneFrame;
	}





    // Update is called once per frame

    public override void UpdateMe() {
	    if (!canBePlayed || !manager.canBeStarted) return;
	    if (isRight && rectImage.offsetMin.x > -LimitSlider.y) {
			isLeft = false;
			rectImage.transform.position -= speedMovingImage * Time.deltaTime * Vector3.right;
		}

		if(rectImage.offsetMin.x <= -LimitSlider.y && rightBT.interactable){
			rightBT.interactable = false;
		} else if (rightBT.interactable == false && rectImage.offsetMin.x > -LimitSlider.y){
			rightBT.interactable = true;
		}

		if (isLeft && rectImage.offsetMin.x < LimitSlider.x) {
			isRight = false;
			rectImage.transform.position += speedMovingImage * Time.deltaTime * Vector3.right;
		}

		if(rectImage.offsetMin.x >= LimitSlider.x && leftBT.interactable){
			leftBT.interactable = false;
		} else if (leftBT.interactable == false && rectImage.offsetMin.x < LimitSlider.x) {
			leftBT.interactable = true;
		}

		if(hasFound >= needFind ){
			Timing.RunCoroutine(startCorrection());
		}

    }

	public void StartThis(){
		//panelDidatica.SetActive(true);
		//panelOld.SetActive(false);
		manager.enabled = false;
		//Timing.RunCoroutine(lateStart());
	}

    public IEnumerator<float> scoreIncrease(int increase) {

        //Debug.Log ("Start Increase Points");
        float times = 0.0f;
        int startPoints = scoreAmount;
        int scoreT = scoreAmount + increase;
        scoreAmount += increase;
        while (times < increaseDurationScoreEffect) {
            times += Time.deltaTime;
            float s = times / increaseDurationScoreEffect;

            int scory = (int)Mathf.Lerp(startPoints, scoreT, increaseScoreCurves.Evaluate(s));
            scoreWindowsText.text = scory.ToString();
            yield return 0f;
        }

    }

    public void ShowObjectFormAnimation(object form)
    {
	    switch (form)
	    {
		  case geometryForm planeForm:
			  _string.Clear();
			  _string.Append("Isto tem forma de ").Append(returnNameSingular(planeForm)).Append("!");
			  break;
		  case SpacialForms spacialForm:
			  _string.Clear();
			  _string.Append("Isto tem forma ").Append(GetSpacialFormName(spacialForm, true)).Append("!");
			  break;
		  case PlaneFigures planeFigures:
			  _string.Clear();
			  _string.Append("Isto tem forma ").Append(GetSpacialFormName(planeFigures, true)).Append("!");
			  break;
	    }
	    textForm.text = _string.ToString();
	    textForm.DOFade(1f, textFormCurveDuration).OnComplete(() =>
	    {
		    textForm.DOFade(0f, textFormCurveDuration);
	    });
    }



    public IEnumerator<float> ShowObjectForm(geometryForm form) {
        float times = 0.0f;

        Color startColor = textForm.color;
        startColor.a = 0f;
        Color endColor = textForm.color;
        endColor.a = 1f;


        textForm.text = _string.ToString();

        while (times < textFormCurveDuration) {
            times += Time.deltaTime;
            float s = times / textFormCurveDuration;

            textForm.color = Color.Lerp(startColor, endColor, textFormCurve.Evaluate(s));

            yield return Timing.WaitForOneFrame;
        }
    }

    public IEnumerator<float> lateStart(){
        //yield return new WaitUntil(() => manager.canBeStarted == true);
     //  Debug.Log("Inicio da Didatica 1", this);
		OnGameStart.Invoke();

		yield return Timing.WaitForSeconds(0.3f);

		float times = 0.0f;
		fadeImage.gameObject.SetActive(true);
		scoreWindowsText.text = manager.scoreAmount.ToString();
		textScoreOriginalPos = scoreText.transform.position;
		scoreAmount = manager.scoreAmount;

		audios = manager.audios;

        /*while (times < fadeInDuration)
		{
			times += Time.deltaTime;
			float s = times / fadeInDuration;

			fadeImage.color = Color.Lerp (new Color (1f, 1f, 1f, 0f), Color.white, fadeInCurve.Evaluate (s));

			
			yield return Yielders.EndOfFrame;
		}*/

        if (log.faseLudica == 4) {
            //terminou tudo. todas as 3 fases e encontrou todos os personagens.
            manager.textFinalMessage.text = "Parabéns, Você encontrou toda a turma.";
            //yield return Timing.WaitForOneFrame;
        } else if (log.faseLudica < 4 && manager.amountsFinded > 0) {
            //ainda não terminou tudo mas encontrou algum dos personagens.
            manager.textFinalMessage.text = "Muito Bem! Tente encontrar todos na próxima.";
            //yield return Timing.WaitForOneFrame;
        } else {
            //não encontrou nada nem terminou nada. zerado burro cego.
            manager.textFinalMessage.text = "Ah que pena! procure melhor a turma da próxima vez.";
            //yield return Timing.WaitForOneFrame;
        }
        manager.textFinalMessage.gameObject.SetActive(true);
        fadeImage.DOFade(1f, fadeInDuration);
        manager.textFinalMessage.DOFade(1f, .3f);

        yield return Timing.WaitForSeconds(fadeInDuration);

        TutorialCheking();

        fadeImage.color = Color.clear;
		formList.Suffle();
		deactiveThis[0].SetActive(false);
		dificult = 0;
		panel02.SetActive(true);
        //Animation of Transition
        //Disable Panels
        //Begin Start;
        //manager.tutorPanelAnimator.SetInteger("emCena",0);
        manager.enabled = false;
        Timing.RunCoroutine(StartGame());
		log.StartTimerDidatica (true);
		
	}

    public void TutorialCheking() {
        if (manager.emCenaCheck == false) {
            manager.tutorPanelAnimator.SetInteger(emCenaHash, 0);
        }

        if (manager.tutorIni_1 == 0) {
           // manager.tutorIni_1 = PlayerPrefs.GetInt("TutorSP_1", 1);
            Timing.KillCoroutines();
            manager.tutorTex.text = manager.tutorTexC;
            fundoProf.enabled = true;
            fundoProf.sprite = ImageFundo;
            fundoProf.raycastTarget = true;
            StartCoroutine(GoForEducationTime());
        } else {
            Timing.KillCoroutines();
            CancelInvoke();
        }

        manager.config.Rank(log.idMinigame, scoreAmount, manager.starsAmount);
        //this.enabled = false;
    }

    public void fundoProfraycastTarget() {
        fundoProf.raycastTarget = false;

    }

    IEnumerator GoForEducationTime() {
        manager.ControlSomTutor2.numTutor = 1;
        manager.emCenaCheck = true;
        yield return Yielders.Get(1f);
        manager.tutorPanelAnimator.SetInteger(emCenaHash, 2);
       // StartCoroutine(lateStart());
        manager.btAvancar.SetActive(true);


    }

    public List<Sprite> returnList(geometryForm form){
		switch (form)
		{
			case geometryForm.square:
				return squareSpritesList;
				break;
			case geometryForm.circle:
				return circleSpritesList;
				break;
			case geometryForm.triangle:
				return triangleSpritesList;
				break;
			case geometryForm.retangle:
				return rectangleSpritesList;
				break;
			default:
				return new List<Sprite>();
				break;
		}
	}

	public string returnName(geometryForm form){
		switch (form)
		{
			case geometryForm.square:
				return "Quadrados";
				break;
			case geometryForm.circle:
				return "Círculos";
				break;
			case geometryForm.triangle:
				return "Triângulos";
				break;
			case geometryForm.retangle:
				return "Retângulos";
				break;
			default:
				return "";
				break;
		}
	}

	public string GetSpacialFormName(SpacialForms form, bool isSingular = false)
	{
		switch (form)
		{
			case SpacialForms.None:
				throw new ArgumentOutOfRangeException(nameof(form), form, null);
				break;
			case SpacialForms.Cube:
				return isSingular ? "Cubo" : "Cubos";
				break;
			case SpacialForms.Retangular:
				return isSingular ? "Forma Retantular" : "Formas Retangulares";
				break;
			case SpacialForms.Pyramid:
				return isSingular ? "Pirâmide" : "Pirâmides";
				break;
			case SpacialForms.Cone:
				return isSingular ? "Cone" : "Cones";
				break;
			case SpacialForms.Cylinder:
				return isSingular ? "Cilindro" : "Cilindros";
				break;
			case SpacialForms.Sphere:
				return isSingular ? "Esfera" : "Esferas";
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(form), form, null);
		}
	}

	public string GetSpacialFormName(PlaneFigures form, bool isSingular = false)
	{
		switch (form)
		{
			case PlaneFigures.None:
				throw new ArgumentOutOfRangeException(nameof(form), form, null);
				break;
			case PlaneFigures.Square:
				return isSingular ? "Quadrado" : "Quadrados";
				break;
			case PlaneFigures.Rectangle:
				return isSingular ? "Retângulo" : "Retângulos";
				break;
			case PlaneFigures.Triangle:
				return isSingular ? "Triângulo" : "Triângulos";
				break;
			case PlaneFigures.Trapeze:
				return isSingular ? "Trapézio" : "Trapézios";
				break;
			case PlaneFigures.Parallelogram:
				return isSingular ? "Paralelogramo" : "Paralelogramos";
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(form), form, null);
		}
	}

    public string returnNameSingular(geometryForm form) {
        switch (form) {
            case geometryForm.square:
                return "Quadrado";
                break;
            case geometryForm.circle:
                return "Círculo";
                break;
            case geometryForm.triangle:
                return "Triângulo";
                break;
            case geometryForm.retangle:
                return "Retângulo";
                break;
            default:
                return "";
                break;
        }
    }

    public Sprite returnIcon(geometryForm form){
		switch (form)
		{
			case geometryForm.square:
				return spritesIcon[0];
				break;
			case geometryForm.circle:
				return spritesIcon[1];
				break;
			case geometryForm.triangle:
				return spritesIcon[2];
				break;
			case geometryForm.retangle:
				return spritesIcon[3];
				break;
			default:
				return spritesIcon[4];
				break;
		}
	}

	public enum geometryForm{
		square,
		circle,
		triangle,
		retangle,
		none
	}

	public void bt_rightDown(){
		isRight = true;
	}

	public void bt_rightUp(){
		isRight = false;
	}

	public void bt_leftDown(){
		isLeft = true;
	}

	public void bt_leftUp(){
		isLeft = false;
	}

	public void AddUserPick(GeometryPlaces_1_2B placeItem){
		userPicks.Add(placeItem);
		updateCircles();
	}

	public void RemoveUserPick(GeometryPlaces_1_2B placeItem){
		userPicks.Remove(placeItem);
		updateCircles();
	}

	public void updateCircles()
	{
		foreach (var circles in panelCircles)
		{
			circles.updateImage();
			//panelCircles[i].hideCloseButtton();
		}

		for (int i = 0; i < userPicks.Count; i++){
			panelCircles[i].updateImage(userPicks[i]);
            //panelCircles[i].showCloseButton();
            if (panelCircles[i].placeOfItem != null) {
                CorrectForm(panelCircles[i]);
            }
        }
	}

	public void hideAllCloseButtons(){
		for (int i = 0; i < panelCircles.Count; i++){
			panelCircles[i].hideCloseButtton();
		}
	}
    
    public void CorrectForm(CirclesOnPanel_1_2B panelCircle) {

	    switch (anoLetivo)
	    {
		    case 1:
			    CorrectCircle(panelCircle, panelCircle.placeOfItem.form == formList[dificult]);
			    break;
		    case 2:
			    CorrectCircle(panelCircle, panelCircle.placeOfItem.spacialForm == selectedForm);
			    break;
		    case 3:
			    CorrectCircle(panelCircle, panelCircle.placeOfItem.planeFigures == selectedPlaneFigure);
			    break;
	    }


    }

    private void CorrectCircle(CirclesOnPanel_1_2B panelCircle, bool isRight = true)
    {
	    panelCircle.outlineImage.effectColor = isRight ? rightColor : wrongColor;
	    panelCircle.correctCompImage.sprite = isRight ? rightCheckSprite : wrongCheckSprite;
	    panelCircle.correctCompImage.color = Color.white;
    }

    public IEnumerator<float> startCorrection(){
		canBePlayed = false;
		hideAllCloseButtons();

		int tempCorrects = 0;

		int scoreTemp = 0;
		int scoreAmountStart = scoreAmount;
		yield return Timing.WaitForSeconds(1f);

		for (int i = 0; i < needFind; i++){
			switch (anoLetivo)
			{
				case 1:
					if(panelCircles[i].placeOfItem.form == formList[dificult]){
						//panelCircles[i].outlineImage.effectColor = rightColor;
						//panelCircles[i].correctCompImage.sprite = rightCheckSprite;
						//panelCircles[i].correctCompImage.color = Color.white;
						//scoreAmount += 10;
						scoreTemp += 10;
						tempCorrects++;
						log.SaveEstatistica (2, 1, true);
						//Debug.Log("Certo!!");
					} else {
						//panelCircles[i].outlineImage.effectColor =  wrongColor;
						//panelCircles[i].correctCompImage.sprite = wrongCheckSprite;
						//panelCircles[i].correctCompImage.color = Color.white;
						log.SaveEstatistica (2, 1, false);
						//Debug.Log("Errado!!");
					}
					break;
				case 2:
					if (panelCircles[i].placeOfItem.spacialForm == selectedForm)
					{
						scoreTemp += 10;
						tempCorrects++;
					}
					log.SaveEstatistica (2, 1, panelCircles[i].placeOfItem.spacialForm == selectedForm);
					break;
				case 3:
					if (panelCircles[i].placeOfItem.planeFigures == selectedPlaneFigure)
					{
						scoreTemp += 10;
						tempCorrects++;
					}
					log.SaveEstatistica (2, 1, panelCircles[i].placeOfItem.planeFigures == selectedPlaneFigure);
					break;
			}
		}

		log.AddPontosPedagogica (scoreTemp);

        Timing.RunCoroutine(scoreIncrease(scoreTemp));
        
		congratsPanel.customWaitTime = 1f;
		highlightCOmp.SetActive(true);
		if(tempCorrects > 0){
			if(tempCorrects > 1){
                _string.Clear();
                _string.Append("Você acertou ").Append(tempCorrects).Append(" itens! \n +").Append(scoreTemp).Append(" pontos.");

                 congratsPanel.startCerto(_string.ToString());
				 soundManager.startSoundFX(clipsAudio[2]);
			} else {
                _string.Clear();
                _string.Append("Você acertou ").Append(tempCorrects).Append(" iten! \n +").Append(scoreTemp).Append(" pontos.");
                congratsPanel.startCerto(_string.ToString());
				 soundManager.startSoundFX(clipsAudio[2]);
			}
		} else {
			congratsPanel.startErrado("Você não acertou nenhum item, tente novamente!");
			soundManager.startSoundFX(clipsAudio[3]);
		}
		
		//"+" + scoreTemp.ToString() + " Pontos."
		
		yield return Timing.WaitForSeconds(3f);
		nextButtonRef.SetActive(true);
		//highlightCOmp.SetActive(false);
		dificult++;
	}

	public void nextButton(){
        soundManager.startSoundFX(clipsAudio[0]);
		nextButtonRef.SetActive(false);
		if(dificult < 3){
			Timing.RunCoroutine(StartGame());
			scoreText.transform.position = textScoreOriginalPos;
			scoreText.transform.localScale = Vector3.zero;
			scoreText.color =  new Color(originalColor.r,originalColor.g,originalColor.b,0f);
		} else {		
			
				panelDesafioController.scoreAmount = scoreAmount;
				panelDesafioController.starAmount = manager.starsAmount;
				
			panelDesafio.SetActive(true);
		}
	}

	public void endGameCall(){
		StopAllCoroutines();
		CancelInvoke();
		scoreEnd.scoreAmount = scoreAmount;
		scoreEnd.starsAmount = manager.starsAmount;
		scoreEndCanvas.gameObject.SetActive(true);		
		
	}

	public void EndAllCoroutines(){
		StopAllCoroutines();
		CancelInvoke();
	}

    public geometryForm VerifyImageForm(Sprite _spriteImage) {
        
        if (returnList(formList[0]).Contains(_spriteImage)) {
            return formList[0];
        } else if(returnList(formList[1]).Contains(_spriteImage)){
            return formList[1];
        } else if (returnList(formList[2]).Contains(_spriteImage)) {
            return formList[2];
        } else if (returnList(formList[3]).Contains(_spriteImage)) {
            return formList[3];
        } else {
            return geometryForm.none;
        }
    
    }




}
