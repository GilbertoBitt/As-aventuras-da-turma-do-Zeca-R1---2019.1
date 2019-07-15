using System.Collections;
using System.Collections.Generic;
using com.csutil;
using MiniGames.Memory.Scripts;
using UnityEngine;
using UnityEngine.UI;
using MEC;
using Sirenix.OdinInspector;
using TMPro;
using UniRx;
using UnityEditor;
using UnityEngine.Serialization;

public class Manager_1_1B : MonoBehaviour
{
	[TabGroup("Geral")]
	public TextMeshProUGUI textTutorialComponent;
	[TabGroup("Geral")] 
	public Transform bottomCardsParent;
	[TabGroup("Geral")] 
	public Transform topCardsParent;
	
	public int level = 0;
	public SpriteRenderer[] cardMaos;

	public SpriteRenderer cardMao;
	public GameObject[] partsCards;
	public GameObject partCards;

	public GameObject[] partsCards2;
	public GameObject partCards2;
	public MemoryGameManager cardManager;
	[TabGroup("1ª Ano")]
	public CardRandomizationGroup cardGroup;
	[TabGroup("1ª Ano")]
	[LabelText("Name Cards Prefab")]
	[SuffixLabel("1ª Ano")]
	public GameObject bottomCardsPrefab1;
	[TabGroup("1ª Ano")]
	[LabelText("Image Cards Prefab")]
	[SuffixLabel("1ª Ano")]
	public GameObject topCardsPrefab1;
	[LabelText("Cell Size 1ª Ano")]
	[TabGroup("1ª Ano")] 
	public Vector2 cellsize_1ano;
	[LabelText("Cell Size Drag 1ª Ano")]
	[TabGroup("1ª Ano")] 
	public Vector2 cellsizeDrag_1ano;
	[LabelText("Texto Acerto")]
	[TabGroup("1ª Ano")]
	public TextMeshProUGUI textRight1;
	[LabelText("Texto Erro")]
	[TabGroup("1ª Ano")]
	public TextMeshProUGUI textWrong1;

	[LabelText("Pontuação por acerto")] [TabGroup("1ª Ano")]
	public int scorePerRight1;
	[LabelText("Habilidade")][TabGroup("1ª Ano")] public HabilidadeBNCCInfo Habilidade1;
	public DragCard_1_1B[] dragCards = new DragCard_1_1B[5];
	public CanvasGroup[] dragCardsCanvasGroup = new CanvasGroup[5];
	[TabGroup("Geral")] 
	public GridLayoutGroup groupLayout;
	[TabGroup("Geral")] 
	public GridLayoutGroup groupLayoutDrop;
	public GameObject panelDitatica;
	public Transform parentDrag;
	public Transform parentDrop;
	public Animator parentDropAnimator;
	public Transform parentMiddle;
	GridLayoutGroup parentMiddleGridLayout;
	public GameObject prefabMiddle;
	public List<GameObject> middleCardsPositions = new List<GameObject>();
	public DropCard_1_1B[] 	dropCards = new DropCard_1_1B[5];
	[TabGroup("1ª Ano")]
	[LabelText("Sprites Escolhidos")]
	[SuffixLabel("1ª Ano")]
	public List<CardItem> spritesChoosen = new List<CardItem>();
	public GameObject[] deactiveThisPanels;
	public GameObject nextButton;
	public TextMeshProUGUI scoreTextAnimComponent;
	public AudioClip[] clips = new AudioClip[13];
    public AudioClip[] fxsClips = new AudioClip[3];
	public SoundManager sound;

	public bool isPlayTime = false;
	public int scoreMade = 0;
	public GameObject panelFinal;
//	public GameObject panelFinalC;
	public FinalScore finalScore;
	public Color RedColor;
	public Color GreenColor;
	public Color YellowColor;
	public TextMeshProUGUI textRight;
	public TextMeshProUGUI textWrong;

	[HeaderAttribute("Animation Settings")]
	public float repositionDuration;
	public AnimationCurve repositionCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
	public float WaitScoreToShow;
	public float showScoreDuration;
	public AnimationCurve showScoreCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
	public Vector3 makeDeckScaleSize;
	public float makeDeckDuration = 1.0f;
	public AnimationCurve makeDeckCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
	public float fadeInButtonDuration = 1.0f;
	public AnimationCurve fadeInButtonCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
	public Vector3 posOffsetText;
	public float wrongTextDuration = 1.0f;
	public AnimationCurve wrongTextCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
	public GameObject particulasCartsDita;
 
	public ExpressaoCorporal zecaCard;


	public GameObject panelDesafio;
	Animator panelDesafioAnimator;
	public int contCart;

	bool checkpanelDesafio;
	private MemoryGameManager managerMemory;
	public PanelDesafioControl panelDesafioController;
	public GameObject interagindoG;

	[HeaderAttribute("LOG")]
	public int idHabilidade;
	public int idDificuldade;
	public LogSystem LOG;
	
	[TabGroup("2ª Ano")]
	[LabelText("Card Group 2ª Ano")]
	public CardRandomizationGroup cardGroup2Ano;
	[FormerlySerializedAs("DragCardPrefab2ano")]
	[TabGroup("2ª Ano")]
	[LabelText("Drag (Bottom) Cards Prefab")]
	[SuffixLabel("2ª Ano")]
	public GameObject dragCardPrefab2Ano;
	[FormerlySerializedAs("DropCardPrefab2ano")]
	[TabGroup("2ª Ano")]
	[LabelText("Drop (Top) Cards Prefab")]
	[SuffixLabel("2ª Ano")]
	public GameObject dropCardPrefab2Ano;
	[LabelText("Cell Size 2ª Ano")]
	[TabGroup("2ª Ano")] 
	public Vector2 cellsize_2ano;
	[LabelText("Cell Size Drag 2ª Ano")]
	[TabGroup("2ª Ano")] 
	public Vector2 cellsizeDrag_2ano;
	[LabelText("Texto Acerto")]
	[TabGroup("2ª Ano")]
	public TextMeshProUGUI textRight2;
	[LabelText("Texto Erro")]
	[TabGroup("2ª Ano")]
	public TextMeshProUGUI textWrong2;
	[LabelText("Pontuação por acerto")] [TabGroup("2ª Ano")]
	public int scorePerRight2;
	[LabelText("Habilidade")][TabGroup("2ª Ano")] public HabilidadeBNCCInfo Habilidade2;
	
	[TabGroup("3ª Ano")]
	[LabelText("Card Group 3ª Ano")]
	public CardRandomizationGroupAlternative cardGroup3Ano;
	[TabGroup("3ª Ano")]
	[LabelText("Drag (Bottom) Cards Prefab")]
	[SuffixLabel("3ª Ano")]
	public GameObject dragCardPrefab3Ano;
	[FormerlySerializedAs("DropCardPrefab3ano")]
	[TabGroup("3ª Ano")]
	[LabelText("Drop (Top) Cards Prefab")]
	[SuffixLabel("3ª Ano")]
	public GameObject dropCardPrefab3Ano;
	[LabelText("Cell Size 3ª Ano")]
	[TabGroup("3ª Ano")] 
	public Vector2 cellsize_3ano;
	[LabelText("Cell Size Drag 3ª Ano")]
	[TabGroup("3ª Ano")] 
	public Vector2 cellsizeDrag_3ano;
	[LabelText("Texto Acerto")]
	[TabGroup("3ª Ano")]
	public TextMeshProUGUI textRight3;
	[LabelText("Texto Erro")]
	[TabGroup("3ª Ano")]
	public TextMeshProUGUI textWrong3;
	[LabelText("Pontuação por acerto")] [TabGroup("3ª Ano")]
	public int scorePerRight3;

	[LabelText("Habilidade")][TabGroup("3ª Ano")] public HabilidadeBNCCInfo Habilidade3;

	[TabGroup("Geral")] public int anoLetivo;
	[TabGroup("Geral")] public Vector3 personPosition;
	[TabGroup("Geral")] public Vector3 personScale;
	[TabGroup("Geral")] public Transform personTransform;
	[TabGroup("Geral")] public RectTransform referenceDinamicPosition;
	[TabGroup("Geral")] public InfoSkillWindow infoSkillInfo;
	[TabGroup("Geral")] public Button infoButtonSkill;

	private GridLayoutGroup _mMiddleCanvasGroup;

	private static readonly int NextCanbeStarted = Animator.StringToHash("nextCanbeStarted");


	// Use this for initialization
	void Start ()
	{

		Resources.UnloadUnusedAssets();
        parentMiddleGridLayout = parentMiddle.GetComponent<GridLayoutGroup>();
		parentDropAnimator = parentDrop.GetComponent<Animator>();
		panelDesafioAnimator = panelDesafio.GetComponent<Animator>();
		managerMemory = GetComponent<MemoryGameManager>();
		managerMemory.config = GameConfig.Instance;
		//TODO variação do texto do erro e acerto.
		Log.d($"Ano letivo Information {JsonWriter.GetWriter().Write(GameConfig.Instance.currentYear)}");
		anoLetivo = GameConfig.Instance.GetAnoLetivo();
		_mMiddleCanvasGroup = parentMiddle.GetComponent(typeof(GridLayoutGroup)) as GridLayoutGroup;
		if (anoLetivo == 1)
		{
			groupLayout.cellSize = cellsizeDrag_1ano;
			groupLayoutDrop.cellSize = cellsize_1ano;
			if (_mMiddleCanvasGroup != null) _mMiddleCanvasGroup.cellSize = cellsize_1ano;
			InstantiateDragCards(5, bottomCardsPrefab1);
			InstantiateDropCards(5, topCardsPrefab1);
			textRight = textRight1;
			textWrong = textWrong1;
		} else if (anoLetivo == 2)
		{
			groupLayout.cellSize = cellsizeDrag_2ano;
			groupLayoutDrop.cellSize = cellsize_2ano;
			if (_mMiddleCanvasGroup != null) _mMiddleCanvasGroup.cellSize = cellsize_2ano;

			cardGroup2Ano.GroupItemList.Shuffle();
			InstantiateDragCards(5, dragCardPrefab2Ano);
			InstantiateDropCards(5, dropCardPrefab2Ano);
			textRight = textRight2;
			textWrong = textWrong2;
		}
		else
		{
			groupLayout.cellSize = cellsizeDrag_3ano;
			groupLayoutDrop.cellSize = cellsize_3ano;
			if (_mMiddleCanvasGroup != null) _mMiddleCanvasGroup.cellSize = cellsize_3ano;
			cardGroup3Ano.ShuffleLists();
			InstantiateDragCards(4, dragCardPrefab3Ano);
			InstantiateDropCards(4, dropCardPrefab3Ano);
			textRight = textRight3;
			textWrong = textWrong3;
		}

		if (infoSkillInfo == null)
		{
			infoSkillInfo = FindObjectOfType<InfoSkillWindow>();
		}

		infoButtonSkill.onClick.AsObservable().Subscribe(unit =>
		{
			switch (anoLetivo)
			{
				case 1:
					infoSkillInfo.ShowWindowInfo(Habilidade1);
					break;
				case 2:
					infoSkillInfo.ShowWindowInfo(Habilidade2);
					break;
				case 3:
				default:
					infoSkillInfo.ShowWindowInfo(Habilidade3);
					break;
			}
		});
		
		
		zecaCard = managerMemory.personReation;
//particulasCartsDita.SetActive(false);
		isPlayTime = false;
		level = 0;
		//cardManager.spriteCards.Suffle();
		var lenght = dragCards.Length;
		for (int i = 0; i < lenght; i++){
			dragCards[i].gameObject.SetActive(false);
		}
		//cardMao.enabled=false;
		partCards.SetActive(false);
		partCards2.SetActive(false);
		cardMao.enabled=false;
		//panelDesafio.SetActive(checkpanelDesafio);
	}

	public void InstantiateDragCards(int amount, GameObject prefabTarget)
	{
		dragCards = new DragCard_1_1B[amount];
		dragCardsCanvasGroup = new CanvasGroup[amount];
		for (int i = 0; i < amount; i++)
		{
			var cardInstance = Instantiate(prefabTarget, bottomCardsParent);
			var cardManagerInstance = cardInstance.GetComponent(typeof(DragCard_1_1B)) as DragCard_1_1B;
			cardManagerInstance.OriginalParent = bottomCardsParent;
			if (cardManagerInstance == null) continue;
			dragCards[i] = cardManagerInstance;
			if (cardManagerInstance != null) dragCardsCanvasGroup[i] = cardManagerInstance.thisCanvasGroup;
		}
	}

	public void InstantiateDropCards(int amount, GameObject prefabTarget)
	{
		dropCards = new DropCard_1_1B[amount];
		for (int i = 0; i < amount; i++)
		{
			var dropInstance = Instantiate(prefabTarget, topCardsParent);
			var dropManagerInstance = dropInstance.GetComponent(typeof(DropCard_1_1B)) as DropCard_1_1B;
			if (dropManagerInstance == null) continue;
			dropManagerInstance.manager = this;
			dropCards[i] = dropManagerInstance;
		}
	}
	
	// Update is called once per frame
	
 	 IEnumerator TemPartc()	{

	yield return Yielders.Get(2f);
	partCards.SetActive(false);
	partCards2.SetActive(false);
		cardMao.enabled=false;
		
	}
	public void verifyCards(){

		int dropCardsLength = dropCards.Length;
		int temp = 0;
		for (int i = 0; i < dropCardsLength; i++){
			if(dropCards[i].cardDraged != null){
				temp++;
			}
		}

		if(temp >= dropCards.Length){
			//verify Answers;
				scoreMade = 0;
			for (int i = 0; i < dropCardsLength; i++){
					dropCards[i].cardDraged.ParticleItem.SetActive(false);
					if(dropCards[i].characterSprite.idItem == dropCards[i].cardDraged.characterSprite.idItem){
						scoreMade++;
					}
			}
			isPlayTime = false;
			//Debug.Log("Check if its Correct");
			StartCoroutine(correction());
		}

	}

	IEnumerator correction(){
        //parentDropAnimator.SetBool("nextCanbeStarted", false);
        parentDropAnimator.enabled = false;
        yield return Yielders.Get(0.2f);
		//parentDropAnimator.SetBool("nextCanbeStarted", false);
        parentDropAnimator.enabled=false;

        //TODO score based on ano letivo.
		int score = scoreMade;
		switch (anoLetivo)
		{
			case 1:
				score *= scorePerRight1;
				break;
			case 2:
				score *= scorePerRight2;
				break;
			case 3:
			default:
				score *= scorePerRight3;
				break;
		}
		int dropCardsLength = dropCards.Length;
		if (scoreMade > 0)
		{
			textRight.text = $"Parabéns, você acertou: +{score}";
		}

        LOG.AddPontosPedagogica(score);

        for (int i = 0; i < dropCardsLength; i++){
			if(dropCards[i].characterSprite.idItem != dropCards[i].cardDraged.characterSprite.idItem){
				dropCards[i].thisoutline.effectColor = RedColor;
				if (LOG != null) {
					LOG.SaveEstatistica (1, 1, false);
				}
			} else {
				dropCards[i].thisoutline.effectColor = GreenColor;

				if (LOG != null) {
					
					LOG.SaveEstatistica (1, 1, true);
				}
			}
			dropCards[i].thisoutline.enabled = true;
		}



		List<Transform> WrongCards = new List<Transform>();
		List<DropCard_1_1B> WrongCardComponent = new List<DropCard_1_1B>();

		int middleCardsPositionsCount = middleCardsPositions.Count;
		for (int i = 0; i < middleCardsPositionsCount; i++){			
			Destroy(middleCardsPositions[i]);
		}

		int wrongs = 0;
		for (int i = 0; i < dropCardsLength; i++){
			if(dropCards[i] != null && dropCards[i].characterSprite.idItem != dropCards[i].cardDraged.characterSprite.idItem){
				wrongs++;
				WrongCards.Add(dropCards[i].transform);
				WrongCardComponent.Add(dropCards[i]);

			}
			//zecaCard.ControlAnimCorpo.SetInteger ("posCorpoZeca",5);
		}

		parentMiddleGridLayout.enabled = false;
		
		groupLayout.enabled = true;
		middleCardsPositions.Clear();
		for (int i = 0; i < wrongs; i++){
			GameObject temp = Instantiate(prefabMiddle,parentMiddle) as GameObject;
			temp.transform.localScale = new Vector3(1f,1f,1f);
			middleCardsPositions.Add(temp);
		}
		
		//	particulasCartsDita.SetActive(true);
		parentDropAnimator.SetBool(NextCanbeStarted, false);

		parentDropAnimator.enabled=false;
		parentMiddleGridLayout.enabled = true;
		

		StartCoroutine(StartTotalPointsInGame(score));

		yield return Yielders.Get(WaitScoreToShow);

		parentMiddleGridLayout.enabled = false;
		
		//Debug.Log("desativa");

		List<Vector3> posFrom = new List<Vector3>();
		List<Vector3> posTo = new List<Vector3>();
		for (int i = 0; i < wrongs; i++){
			WrongCards[i].SetParent(parentMiddle);
			posFrom.Add(WrongCards[i].transform.position);
		}
		for (int i = 0; i < wrongs; i++){
			posTo.Add(middleCardsPositions[i].transform.position);
		}

		groupLayoutDrop.enabled = true;
		//parentDrop.GetComponent<Animator>().enabled=false;

		float times = 0.0f;
		while (times < repositionDuration)
		{
			times += Time.deltaTime;
			float s = times / repositionDuration;

			for (int i = 0; i < wrongs; i++){
				WrongCards[i].position = Vector3.Lerp(posFrom[i],posTo[i], repositionCurve.Evaluate (s));
			}

			yield return Yielders.EndOfFrame;
		}

		for (int i = 0; i < wrongs; i++){			
			Destroy(middleCardsPositions[i]);
		}

		/*times = 0.0f;
		while (times < showScoreDuration)
		{
			times += Time.deltaTime;
			float s = times / showScoreDuration;

			scoreTextAnimComponent.transform.localScale = Vector3.Lerp(new Vector3(0f,0f,0f), new Vector3(1f,1f,1f), showScoreCurve.Evaluate(s));
			scoreTextAnimComponent.color = Color.Lerp (new Color(1f,1f,1f,0f), Color.yellow, showScoreCurve.Evaluate (s));

			yield return Yielders.EndOfFrame;
		}*/

		//textHere
		textWrong.gameObject.SetActive(wrongs > 0);

		if (scoreMade > 0)
		{
			textRight.gameObject.SetActive(wrongs < 5);
		}

		//Debug.Log("Wrongs Amount" + wrongs.ToString(), this);
		
        if(scoreMade <= 0) {
            //acertar todos os nomes.
            sound.startSoundFX(fxsClips[2]);
        } else if (scoreMade > 0 && scoreMade < 5) {
            //acertar e errar ao mesmo tempo.
            sound.startSoundFX(fxsClips[1]);
        } else if (scoreMade >= 5) {
            //errar todos os nomes.
            sound.startSoundFX(fxsClips[0]);
        } 

		parentMiddleGridLayout.enabled = true;

		for (int i = 0; i < wrongs; i++){
			WrongCardComponent[i].updateRightText(anoLetivo);
		}

		times = 0.0f;
		while (times < wrongTextDuration)
		{
			times += Time.deltaTime;
			float s = times / wrongTextDuration;

			for (int i = 0; i < wrongs; i++){
				//WrongCards[i].position = Vector3.Lerp(posFrom[i],posTo[i], repositionCurve.Evaluate (s));
				WrongCardComponent[i].cardDraged.RightOneTextComponent.transform.localPosition = Vector3.Lerp(new Vector3(0f,-10f,0f),posOffsetText,wrongTextCurve.Evaluate(s));
				WrongCardComponent[i].cardDraged.RightOneTextComponent.color = Color.Lerp(new Color(1f,1f,1f,0f),YellowColor, wrongTextCurve.Evaluate(s));
				WrongCardComponent[i].cardDraged.TextComponent.color = Color.Lerp(YellowColor,RedColor, wrongTextCurve.Evaluate(s));
			}

			yield return Yielders.EndOfFrame;
		}

//		groupLayoutDrop.enabled = false;
		level++;
		nextButton.SetActive(true);


	}

	public void NextButton(){

		
		textRight.gameObject.SetActive(false);
		textWrong.gameObject.SetActive(false);
		if(level < 3){
            //RunStartB();
            Timing.RunCoroutine(makeDeck());
        } else {

			if(checkpanelDesafio==false){
				interagindoG.SetActive (true);
				checkpanelDesafio=true;
				interagindoG.GetComponent<GraphicRaycaster> ().enabled = true;
				panelDesafioController.starAmount = cardManager.starsAmount;
				panelDesafioController.scoreAmount = cardManager.pointsAmount;
				panelDesafio.SetActive(checkpanelDesafio);
				panelDesafioAnimator.SetInteger("panelDesafioNumber",1);


			}
	
		}
			
		
	}

    public void ResetCardScale() {
        for (int i = 0; i < dropCards.Length; i++) {
            dropCards[i].transform.localScale = new Vector3(1f, 1f, 1f);
        }
       
    }
	
	public IEnumerator Begin11B()
	{
		personPosition = referenceDinamicPosition.position;
		personTransform.position = personPosition;
		personTransform.localScale = personScale;

		int dragCardsLength = dragCards.Length;
		
		for (int i = 0; i < dragCardsLength; i++){
			dragCards[i].RightOneTextComponent.color = new Color(1f,1f,1f,0f);
			dragCards[i].TextComponent.color = YellowColor;
			dragCards[i].RightOneTextComponent.transform.localPosition = new Vector3(0f,0f,0f);
		}

		for (int i = 0; i < dragCardsLength; i++){
           
            if (dropCards[i] != null && dropCards[i].thisoutline != null) {
                dropCards[i].thisoutline.enabled = false;
            }
		}

		if (anoLetivo == 1)
		{
			spritesChoosen.Clear();
			switch (level){
				case 0:
					for (int i = 0; i < 5; i++){
						spritesChoosen.Add(cardGroup.GroupItemList[i]);
					}
					break;	
				case 1:
					for (int i = 5; i < 10; i++){
						spritesChoosen.Add(cardGroup.GroupItemList[i]);
					}
					break;
				case 2:
					for (int i = 0; i < 11; i++){
						if(i%2==1){
							spritesChoosen.Add(cardGroup.GroupItemList[i]);
						}
					}
					break;
				default:
					break;
			}

			spritesChoosen.Shuffle();
		}
		else if (anoLetivo == 2)
		{
			spritesChoosen.Clear();
			for (int j = level * 5; j < (level*5)+5; j++)
			{
				spritesChoosen.Add(cardGroup2Ano.GroupItemList[j]);
				spritesChoosen.Shuffle();
			}
		}
		else
		{
			spritesChoosen.Clear();
			var result = cardGroup3Ano.GetByIndex(level);
			spritesChoosen.Add(result.monosillaba);
			spritesChoosen.Add(result.dissilabo);
			spritesChoosen.Add(result.trissilabo);
			spritesChoosen.Add(result.polissilabo);
			spritesChoosen.Shuffle();
		}
		spritesChoosen.Shuffle();
		var tempList = spritesChoosen;
		tempList.Shuffle();
		for (int i = 0; i < dragCards.Length; i++)
		{
			
			dragCards[i].transform.SetParent(parentDrag);
			
			dragCards[i].UpdateSprite(tempList[i], anoLetivo);
			dragCards[i].Clear();
			dragCards[i].thisCanvasGroup.blocksRaycasts = true;

		}

		if (anoLetivo == 1)
		{
			spritesChoosen.Shuffle();
		}

		spritesChoosen.Shuffle();
		for (int i = 0; i < dropCards.Length; i++){
			dropCards[i].transform.SetParent(parentDrop);
			if (anoLetivo == 1)
			{
				dropCards[i].UpdateSprite(spritesChoosen[i]);
			} else if (anoLetivo == 2) {
				dropCards[i].UpdateName(spritesChoosen[i]);
			}else if (anoLetivo == 3) {
				dropCards[i].UpdateName(spritesChoosen[i] as CardItemAlternative, anoLetivo);
			}
			
			dropCards[i].Clear();
		}

        yield return Yielders.Get(0.6f);
        zecaCard.posCorpoZeca = 5;
        zecaCard.ControlAnimCorpo.SetInteger("posCorpoZeca", 5);

        yield return Yielders.Get(0.5f);

    }

	public void buttonFadin(){
		cardManager.sound.startSoundFX(cardManager.audio[4]);
		StartCoroutine(beginButtons());
	}

	public IEnumerator beginButtons(){
		for (int i = 0; i < dragCards.Length; i++){
			dragCards[i].gameObject.SetActive(true);
		}

		groupLayout.enabled = true;

		float times = 0.0f;
		while (times < fadeInButtonDuration)
		{
			times += Time.deltaTime;
			float s = times / fadeInButtonDuration;

			//int scory = (int)Mathf.Lerp (startPoints, scoreT, cardManager.textPointsEffectCurve.Evaluate (s));

			float xScale = Mathf.Lerp(0f,1f, fadeInButtonCurve.Evaluate(s));
			float yScale = Mathf.Lerp(0f,1f, fadeInButtonCurve.Evaluate(s));

			for (int i = 0; i < dragCards.Length; i++){
				dragCards[i].transform.localScale = new Vector3(xScale,yScale,1f);
			}

			yield return Yielders.EndOfFrame;
		}

		var enabled1 = groupLayout.enabled;
		enabled1 = false;
		enabled1 = true;
		groupLayout.enabled = enabled1;


		yield return Yielders.Get(0.1f);

		
		//animação pra voltar pra tela.
		//ao final de animação desativar e ativar novamento groupLayout.
		isPlayTime = true;

	}


    public void RunStartB() {
        Timing.RunCoroutine(StartBGame());
    }

	public IEnumerator<float> StartBGame(){

        LOG.StartTimerDidatica(true);
        cardGroup.GroupItemList.Shuffle();

		
		contCart=contCart+1;
		StartCoroutine(Begin11B());
		
		panelDitatica.SetActive(true);
      
        yield return Timing.WaitForOneFrame;

		partCards.SetActive(true);
		cardMao.enabled=true;
		partCards2.SetActive(true);

        zecaCard.posCorpoZeca = 5;
        zecaCard.ControlAnimCorpo.SetInteger("posCorpoZeca", 5);

    }

	public IEnumerator StartTotalPointsInGame(int increase){
		float times = 0.0f;
		int startPoints = cardManager.pointsAmount;
		int scoreT = cardManager.pointsAmount + increase;
		cardManager.pointsAmount += increase;	
		while (times < cardManager.increasePointsEffect)
		{
			times += Time.deltaTime;
			float s = times / cardManager.increasePointsEffect;

			int scory = (int)Mathf.Lerp (startPoints, scoreT, cardManager.textPointsEffectCurve.Evaluate (s));
			cardManager.textPoints.text = scory.ToString ();
			yield return Yielders.EndOfFrame;
		}
	}


	public IEnumerator<float> makeDeck(){
parentDropAnimator.SetBool("nextCanbeStarted", false);
		groupLayout.enabled = false;

        managerMemory.checkdidatica = true;
        var cardsPos = new List<Vector3>();

        foreach (var t in dropCards)
        {
	        cardsPos.Add(t.transform.position);
        }
		
		float times = 0.0f;
		while (times < makeDeckDuration)
		{
			times += Time.deltaTime;
			float s = times / makeDeckDuration;

			//int scory = (int)Mathf.Lerp (startPoints, scoreT, cardManager.textPointsEffectCurve.Evaluate (s));

			float xScale = Mathf.Lerp(1f,0f, cardManager.makeDeckCurve.Evaluate(s));
			float yScale = Mathf.Lerp(1f, 0f, cardManager.makeDeckCurve.Evaluate(s));

			for (int i = 0; i < 5; i++){
//				dropCards[i].transform.localScale = new Vector3(xScale,yScale,1f);
//				dropCards[i].transform.position = Vector3.Lerp(cardsPos[i],cardManager.characterDeckLocation.position,cardManager.makeDeckCurve.Evaluate(s));
              
			}

			yield return Timing.WaitForOneFrame;
				
		}
	
		//parentDropAnimator.enabled=true;
        partCards.SetActive(true);
        cardMao.enabled = true;
        partCards2.SetActive(true);


        foreach (var card in dragCards) {
	        if(card != null)
		        card.gameObject.SetActive(false);
        }

        //ResetCardScale();
        //cardMao.enabled = true;
        //  yield return Timing.WaitForSeconds(1f);


        //retirar e chamar pelo animator.
        //	cardManager.cardsAnimation.SetBool("nextCanbeStarted", true);
        yield return Timing.WaitForSeconds(0.1f);
        StartCoroutine(Begin11B());	

        

        //cardManager.cardsAnimation.SetBool("nextCanbeStarted", true);	

    }

    public void JogarCartas() {
		//parentDropAnimator.SetInteger("NumbCheck", 1);
       // parentDropAnimator.enabled = true;
     
        partCards.SetActive(false);
        cardMao.enabled = false;
        partCards2.SetActive(false);      

    }
}
