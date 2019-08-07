﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
 using com.csutil;
 using UnityEngine.Events;
using UnityEngine.EventSystems;
using MEC;
using Sirenix.OdinInspector;
using TMPro;
using TutorialSystem.Scripts;

public class Manager1_4A : OverridableMonoBehaviour {

#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
    [SeparatorAttribute("Referencias")]
#endif
    public List<GridLayoutGroup> Grids = new List<GridLayoutGroup>();
    public Transform dragParent;
    public Transform towerParent;
    public Manager1_4B nextManager;
    public changeLevel _highlight;
    public GameObject highlightOB;
    public Transform towerGrid;
    public Button pauseButton;
    //[Header("Prefabs")]
#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
    [SeparatorAttribute("Prefabs")]
#endif
    public GameObject circlesOnGrid;
    public GameObject imagesOfItem;

#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
    [SeparatorAttribute("Variáveis")]
#endif
    public bool isPlaying = false;
    public bool isDragging = false;
    [Range(0f, 100f)]
    public float chancesOfBadItem = 5f;
//    public bool isGameEnded = false;
    public bool hasEndedByTime = false;
    public Color gridColor;

#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
    [SeparatorAttribute("Dificuldade Atual")]
#endif
    public int currentDificult = 0;
    public int startDificult = 0;
    public Dificult1_4A dificult;
#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
    [SeparatorAttribute("Configuração Transparencia")]
#endif
    [Range(0f, 1f)]
    public List<float> transparentLevels = new List<float>();
#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
    [SeparatorAttribute("Listas de Dados")]
#endif
    public List<Item1_4A> itemsOfTower = new List<Item1_4A>();
    public List<Dificult1_4A> dificults = new List<Dificult1_4A>();
#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
    [SeparatorAttribute("Instancias")]
#endif
    public List<GameObject> circlesOnScene = new List<GameObject>();
    public List<GameObject> imagesLayerOnScene = new List<GameObject>();
    private List<ItemHandler1_4A> itemHandlers = new List<ItemHandler1_4A>();
#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
    [SeparatorAttribute("Configuração de Animações")]
#endif
    public float itemAlphaChangeDuration = 1.0f;
    public AnimationCurve itemAlphaChangeCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
    [Range(1f, 300f)]
    public float towerFallOffset = 1f;
    public float towerFallDuration = 1.0f;
    public AnimationCurve towerFallCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
    [SeparatorAttribute("Configuração do Raycast")]
#endif
    public Vector2 directionOfRayRight;
    public Color debugDrawRayColorRight;
    public Vector2 directionOfRayLeft;
    public Color debugDrawRayColorLeft;
#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
    [SeparatorAttribute("Configuração de Pontuação")]
#endif
    public int scoreAmount = 0;
    public int startScoreAmount = 0;
    public int amountScorePerCorrect = 0;
    public TextMeshProUGUI scoreTextComp;
    public float scoreIncreaseDuration = 1.0f;
    public AnimationCurve ScoreIncreaseCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
    public float scoreDecreaseDuration = 1.0f;
    public AnimationCurve ScoreDecreaseCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
    [SeparatorAttribute("Configuração de Estrelas")]
#endif
    public int starAmount = 0;
    public Sprite spriteFullStar;
    public Sprite spriteEmptyStar;
    public Transform starsParent;
#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
    [SeparatorAttribute("Configuração de Timer")]
#endif
    public float timerStartValue = 60f;
    public Slider timerSlider;
#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
    [SeparatorAttribute("Configuração de Combo")]
#endif
    public int combo = 0;
    public TextMeshProUGUI comboTextComp;
    public float showComboTextDuration = 1.0f;
    public AnimationCurve showComboTextCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);

    [HideInInspector]
    public bool isRemoving = false;
    private List<ItemHandler1_4A> ItemHandlerList = new List<ItemHandler1_4A>();
#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
    [SeparatorAttribute("Item Bonus!")]
#endif
    public int amountBonusItem = 0;
    public bool hasItemBonus = false;
    public float breakBetweenBonus;
    private ItemGroup1_4A itemWithBonusItem;
    [RangeAttribute(0f, 100f)]
    public float chancesOfBonus;
#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
    [SeparatorAttribute("Configurações dos Baús!")]
#endif
    public float timeToRandomizeChest;
    [Range(0f, 100f)]
    public float chanceToRandomChest;
    public List<ItemType1_4A> itemtypes = new List<ItemType1_4A>();
    public List<ChestHandler1_4A> chests = new List<ChestHandler1_4A>();
    private List<ChestHandler1_4A> chests2 = new List<ChestHandler1_4A>();
    public Sprite[] towerFloors = new Sprite[5];
#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
    [SeparatorAttribute("Baú Bonus!")]
#endif
    public bool hasChestBonus = false;
    public int bonusChestScore;
    public bool chooseChestBonus;
    [RangeAttribute(0f, 100f)]
    public float chancesOfChestBonus;
    public float timeToChestBonus;

#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
    [SeparatorAttribute("Baú Fechado")]
#endif

    [RangeAttribute(0f, 100f)]
    public float chancesOfChestClose;
    public float timeToChestClose;
    public bool hasChestClose = false;
    public bool isClosingChest = false;
    public bool isOpeningChest = false;
    private ChestHandler1_4A _closedChest;
    public Vector2 rangeTimeToOpen;
    public List<GameObject> chestsParent = new List<GameObject>();
    public List<GameObject> ChestOnRight = new List<GameObject>();

#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
    [SeparatorAttribute("Informações Iniciais[Historico]")]
#endif
    public Vector3 startTowerPos;

    public int valorTest = 10;
#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
    [SeparatorAttribute("Eventos")]
#endif
    public UnityEvent OnChestsChange = new UnityEvent();
    public UnityEvent OnBonusItemShow = new UnityEvent();
    public UnityEvent OnChestBlock = new UnityEvent();
    public UnityEvent OnChestOpens = new UnityEvent();

#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
    [SeparatorAttribute("Tremedeira da Torre")]
#endif
    public bool shakeOn = false;
    public float shakePower = 0;
    // sprite original position
    public Vector3 originPosition;
#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
    [SeparatorAttribute("Animação do Item Errado")]
#endif
    public Vector3 posOffsetThrow;
    public float itemThrowAwayDuration = 1.0f;
    public AnimationCurve itemThrowAwayScaleCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
    public AnimationCurve itemThrowAwayPositionCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
    public GameObject[] partBauTransp;
    public GameObject[] bauGO;
    //public ParticleSystem[]  partBauTransp2;

#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
    [SeparatorAttribute("Animação do FadeInFadeOut")]
#endif
    public float fadeOutChestChangeDuration = 1.0f;
    public AnimationCurve fadeOutChestChangeCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
    public float fadeInChestChangeDuration = 1.0f;
    public AnimationCurve fadeInChestChangeCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);

    public bool checkTorreDestroy;

    public GameObject jucaBaufechar;
    int _chestIndex;
    public bool checkJucaBau = false;
    public bool checkZecaBau = false;

    public bool checkBauOpen = false;

    public IEnumerator randomchest;

#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
    [SeparatorAttribute("Animação do FadeInFadeOut")]
#endif
    public AudioClip[] audios;
    public SoundManager soundManager;
    bool checkBauIni;
  //  int tutorTroca;
  //  int tutorObjLock;
  //  int TutorBauBonus;
   // public GameObject tutorial;
    public GameObject montanha;
    public GameObject didatica;

#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
    [SeparatorAttribute("LOG")]
#endif
    public LogSystem log;
    public ChestHandler1_4A chestBonus;

    private StringFast stringFast = new StringFast();

    public LayerMask layerItemHandler = new LayerMask();
   // public TutorMontanha TutorMontanha2;

    public GameConfig config => GameConfig.Instance;
    public Minigames minigame;

    private CoroutineHandle chestBonusEnd;

    public GameObject painelMaeZeza;

    bool checkMaeZeca;

    public Text texProfessora;

    [TextArea()]
    public string[] textos;
    public ZecaBauMonanha[] ZecaBauMonanhaV;

    [BoxGroup("Lúdica Final")]
    public SpeechInfo speechInfoFinalLudica1;
    [BoxGroup("Lúdica Final")]
    public SpeechInfo speechInfoFinalLudica2;
    [BoxGroup("Lúdica Final")]
    public SpeechInfo speechInfoFinalLudica3;


    private static readonly int AbrirBau = Animator.StringToHash("AbrirBau");
    private static readonly int FecharBau = Animator.StringToHash("fecharBau");
    private static readonly int NumbBauSort = Animator.StringToHash("NumbBauSort");
    private static readonly int TransporTrue = Animator.StringToHash("TransporTrue");

    public DialogComponent dialogComponent;

    // Use this for initialization
    void Start()
    {
	    dialogComponent = FindObjectOfType(typeof(DialogComponent)) as DialogComponent;
	    if (dialogComponent == null) return;
	    dialogComponent.endTutorial = () =>
	    {
		    StartGame();
		    foreach (var item in chests)
		    {
			    item.enabled = true;
		    }

		    foreach (var item in ZecaBauMonanhaV)
		    {
			    item.enabled = true;
		    }
	    };
    }

    private void StartGame()
    {
	    StartCoroutine(beginGame());
	    startTowerPos = towerParent.position;
	    _chestIndex = Random.Range(0, chests.Count - 1);
	    log.StartTimerLudica(true);
	    Input.multiTouchEnabled = false;
    }


    // Update is called once per frame
    public override void UpdateMe() {

        if (isPlaying && timerSlider.value >= timerStartValue && hasEndedByTime == false) {
            timerSlider.value = 0;
            ////Debug.Log ("DONE");
            CancelInvoke();
            hasEndedByTime = true;
            var ItemHandlerListCount = ItemHandlerList.Count;
	        for (var i = 0; i < ItemHandlerListCount; i++)
		        ItemHandlerList[i].hasEnded = true;
        }
#if UNITY_EDITOR

        if (Input.GetKeyDown(KeyCode.J)) {
            timerSlider.value = 0;
            ////Debug.Log ("DONE");
            CancelInvoke();
            hasEndedByTime = true;

            int ItemHandlerListCount = ItemHandlerList.Count;
            for (int i = 0; i < ItemHandlerListCount; i++) {
                ItemHandlerList[i].hasEnded = true;
            }
        }
#endif

        if (Input.GetKeyDown(KeyCode.L)) {
            ShakeCameraOn(shakePower);
        }

        if (Input.GetKeyDown(KeyCode.O)) {
            ShakeCameraOff();
        }

        if (!shakeOn) return;
        // reset original position
        var transform1 = towerParent.transform;
        var localPosition = transform1.localPosition;
        localPosition = originPosition;

        // generate random position in a 1 unit circle and add power
        Vector2 ShakePos = Random.insideUnitCircle * shakePower;

        // transform to new position adding the new coordinates
        localPosition = new Vector3(localPosition.x + ShakePos.x,
	        localPosition.y + ShakePos.y, localPosition.z);
        transform1.localPosition = localPosition;
    }

    void updateStarAmount(int _amount) {
        starAmount = _amount;
        if (_amount >= 3) return;
        _amount++;

        for (int i = 0; i < 3; i++) {
	        starsParent.GetChild(i).GetComponent<Image>().sprite = spriteEmptyStar;
        }

        for (int i = 0; i < _amount; i++) {
	        starsParent.GetChild(i).GetComponent<Image>().sprite = spriteFullStar;
        }

    }
    /*void StartbeginGame(){
		StartCoroutine (beginGame ());
	}*/
    public IEnumerator beginGame() {

        isPlaying = false;
        hasEndedByTime = false;
        timerSlider.value = 0f;
        Debug.Log("Game Loop Started");

        dificult = dificults[currentDificult];
        timerStartValue = dificult.timeToComplete;
        timerSlider.maxValue = timerStartValue;
        startScoreAmount = scoreAmount;
        startDificult = currentDificult;
        chancesOfBadItem = dificult.chancesOfBadItems;
        combo = 0;
        int tempSizeOfTower = dificult.sizeOfTower;
        Grids.ReturnReverseList();
        for (int i = 0; i < dificult.sizeOfTower; i++) {

            List<ItemHandler1_4A> handlersOfThis = new List<ItemHandler1_4A>();
            Image img = Grids[i].GetComponent<Image>();
            Grids[i].GetComponent<Floor1_4A>().floorItems.Clear();
            for (int j = tempSizeOfTower - 1; j >= 0; j--) {
                GameObject circleOfItem = Instantiate(circlesOnGrid, Grids[i].gameObject.transform) as GameObject;
                ItemHandler1_4A itemHandler = circleOfItem.GetComponent<ItemHandler1_4A>();
                itemHandler.towerFloor = i;
                circleOfItem.transform.localScale = new Vector3(1f, 1f, 1f);
                itemHandler.gameManager = this;
                circlesOnScene.Add(circleOfItem);
                itemHandler.floorParent = Grids[i].GetComponent<Floor1_4A>();
                Grids[i].GetComponent<Floor1_4A>().floorItems.Add(itemHandler);
	            handlersOfThis.Add(itemHandler);
                itemHandler.sortingOrder = tempSizeOfTower;
            }
            setHandlerOnHandler(handlersOfThis);
            tempSizeOfTower--;
        }

        Grids.ReturnReverseList();
        int circlesOnSceneCount = circlesOnScene.Count;
        for (int i = 0; i < circlesOnSceneCount; i++) {
            int tempIndex = dificult.sizeOfLayers - 1;
            ItemHandler1_4A itemHandler = circlesOnScene[i].GetComponent<ItemHandler1_4A>();
            ItemHandlerList.Add(itemHandler);
            for (int j = 0; j < dificult.sizeOfLayers; j++) {
                GameObject imageItem = Instantiate(imagesOfItem, circlesOnScene[i].transform) as GameObject;
                itemsOfTower.Shuffle();
                imagesLayerOnScene.Add(imageItem);
                imageItem.transform.localScale = new Vector3(1f, 1f, 1f);
                itemHandler.itemsOnSlot.Add(imageItem);
                itemHandler.UpdateItemInfo(j, itemsOfTower[j], transparentLevels[tempIndex]);
                tempIndex--;
                imageItem.GetComponent<Canvas>().sortingOrder = itemHandler.sortingOrder;
            }

            itemHandler.itemsOnSlot.Reverse();

            itemHandler.floorParent.CheckFloorDone();
            itemHandler.UpdateItemHandlerOnChilds();
            itemHandler.ActiveCollider2D();
            itemHandler.UpdateStartList();
            //yield return new WaitForSeconds (.2f);
            itemHandler.UpdateBadItemsLimit();
            itemHandler.slotCount = itemHandler.itemsOnSlot.Count;
            itemHandler.UpdateItemHandlersHistory();
            itemHandler.StartMaxBadItemsUpdate();
            itemHandler.UpdateDragItem();
        }

        if (checkBauIni == false) {
            int chestsCount = chests.Count;
            checkBauIni = true;
            for (int i = 0; i < chestsCount; i++) {
                chests[i].isChestBonus = false;
                chests[i].isChestClose = false;
                chests[i].ToggleRaycastTarget(true);
                chests[i].chestGroup = itemtypes[i];
                chests[i].UpdateText(ChestNameByType(itemtypes[i]));
            }
        }
        else {
            int chestsCount = chests.Count;
            for (int i = 0; i < chestsCount; i++) {
                chests[i].isChestBonus = false;
                //chests [i].isChestClose = false;
                chests[i].chestGroup = itemtypes[i];
                chests[i].UpdateText(ChestNameByType(itemtypes[i]));

            }
            //if (chests [_chestIndex].ToggleRaycastTarget () == false) {
            //chests [_chestIndex].ToggleRaycastTarget (true);
            //chests[_chestIndex].zecaFecharBau.SetBool("AbrirBau",true);
            StartCoroutine(DesativBauAnim());
            //}

        }

        hasChestBonus = false;
        hasChestClose = false;

        updateItemHandlerList();

        StartCoroutine(endGame());
        InvokeRepeating(nameof(DecreaseTimeOverSeconds), 1f, 1f);
        InvokeRepeating(nameof(ItemBonus), breakBetweenBonus, breakBetweenBonus);
        Invoke(nameof(startRandomChest), timeToRandomizeChest);
        InvokeRepeating(nameof(ChooseChest), timeToChestBonus, timeToChestBonus);
        Invoke(nameof(randomChestToCLose), timeToChestClose);
        //StartCoroutine(RandomChestsTime());
        //yield return new WaitForSeconds (1f);
        yield return Yielders.Get(1f);
        itemHandlers.ForEach(a =>
        {
	        a.UpdateItemInfos();
	        a.UpdateDragCanvas();
	        a.ResetItemsPos();
	        a.UpdateDragItem();
	        a.OnBeginDrag(new PointerEventData(EventSystem.current));
	        a.OnEndDrag(new PointerEventData(EventSystem.current));
        });
        isPlaying = true;
    }
    IEnumerator DesativBauAnim() {
        yield return Yielders.Get(2f);
        chests[_chestIndex].zecaFecharBau.SetBool(AbrirBau, true);
        int chestsCount = chests.Count;
        for (int i = 0; i < chestsCount; i++) {
            chests[i].GetComponent<Animator>().enabled = false;
        }
    }



    public void setHandlerOnHandler(List<ItemHandler1_4A> _handlerList) {
        for (int i = 0; i < _handlerList.Count; i++) {
            for (int j = 0; j < _handlerList.Count; j++) {
                if (_handlerList[i] != _handlerList[j]) {
                    _handlerList[i].AddHandler(_handlerList[j]);
                }
            }
        }
    }

    /// <summary>
    /// Item group is correct.
    /// </summary>
    /// <param name="itemGroup">Item group.</param>
    public void ItemGroupIsCorrect(ItemHandler1_4A itemGroup, bool _isChestBonus) {
        ItemGroup1_4A item = itemGroup.itemToDrag.GetComponent<ItemGroup1_4A>();
        if (item.hasObjectOnLeft || item.hasObjectOnRight) {
            ItemGroupIsWrong2(itemGroup);
        } else {
            soundManager.startSoundFX(audios[0]);
            isRemoving = true;
            item.DisableBackgroundImage();
            bool IsBonusItem = item.isBonusItem;
            int increasePoints = scoreToIncrease(IsBonusItem);
            item.DisableBackgroundImage();


            if (_isChestBonus && !IsBonusItem) {
                comboTextComp.text = "Baú bônus";
                increasePoints *= bonusChestScore;
                Timing.RunCoroutine(ShowComboText(), "ComboEffector");
            } else if (!_isChestBonus && IsBonusItem) {
                comboTextComp.text = "Item bônus";
                Timing.RunCoroutine(ShowComboText(), "ComboEffector");
            } else if (_isChestBonus && IsBonusItem) {
                comboTextComp.text = "Item bônus + Baú Bônus";
                Timing.RunCoroutine(ShowComboText(), "ComboEffector");
            }

            StartCoroutine(scoreIncrease(increasePoints));
            //StartCoroutine(ShowComboText());
            if (item.hasObjectOnLeft || item.hasObjectOnRight) {
                ResetToyTower(item);
            } else {
                StartCoroutine(UpdateLayerOfItemsIE(itemGroup, true));
            }
            item.DisableBackgroundImage();
        }
    }

    /// <summary>
    /// Item group is wrong.
    /// </summary>
    /// <param name="itemGroup">Item group.</param>
    public void ItemGroupIsWrong(ItemHandler1_4A itemGroup) {
        soundManager.startSoundFX(audios[1]);
        isRemoving = true;
        ItemGroup1_4A item = itemGroup.itemToDrag.GetComponent<ItemGroup1_4A>();
        item.DisableBackgroundImage();
        if (item.hasObjectOnLeft || item.hasObjectOnRight) {
            ResetToyTower(item);
        } else {
            item.DisableBackgroundImage();
            StartCoroutine(UpdateLayerOfItemsIE(itemGroup, false));
        }
        item.DisableBackgroundImage();
    }

    public void ItemGroupIsWrong2(ItemHandler1_4A itemGroup) {
        soundManager.startSoundFX(audios[1]);
        isRemoving = true;
        ItemGroup1_4A item = itemGroup.itemToDrag.GetComponent<ItemGroup1_4A>();
        item.DisableBackgroundImage();
        if (item.hasObjectOnLeft || item.hasObjectOnRight) {
	        
            ResetToyTower(item);
        } else {
            item.DisableBackgroundImage();
            //StartCoroutine (UpdateLayerOfItemsIE (itemGroup, false));
        }
        item.DisableBackgroundImage();
    }

    public void ResetToyTower(ItemGroup1_4A item) {
        ////Debug.Log ("Torre será montada novamente!");
        isPlaying = false;
        Timing.RunCoroutine(fallOfTower(item));
    }

	/// <summary>
	/// Falls of The tower.
	/// </summary>
	/// <param name="itemGroup1_4A"></param>
	/// <returns>Animation and Effect of Tower Falling</returns>
	IEnumerator<float> fallOfTower(ItemGroup1_4A itemGroup) {
        checkTorreDestroy = true;
        StartCoroutine(scoreReset());
        //yield return new WaitForSeconds (0.1f);
        yield return Timing.WaitForSeconds(0.1f);

		itemGroup.thisItemHandler.enabled = false;

        Vector3 startPos = towerGrid.transform.position;
        Vector3 endPos = new Vector3(startPos.x, startPos.y - towerFallOffset, startPos.z);

        ShakeCameraOn(shakePower);
        float timer = 0.0f;
        while (timer < towerFallDuration)
        {
            timer += Time.deltaTime;
            float s = timer / towerFallDuration;

            //float scale = Mathf.Lerp (0f, 1f, towerFallCurve.Evaluate (s));
            towerGrid.transform.position = Vector3.Lerp(startPos, endPos, towerFallCurve.Evaluate(s));

            yield return Timing.WaitForOneFrame;
        }
        ShakeCameraOff();
        checkTorreDestroy = false;
        //yield return new WaitForSeconds (1f);
        yield return Timing.WaitForSeconds(1f);
        Timing.RunCoroutine(rebuildingTower(itemGroup.thisItemHandler));

    }
    public int scoreToIncrease(bool _hasBonusItem) {
        if (_hasBonusItem) {
            int score = amountBonusItem;
            return score;
        } else {
            int score = amountScorePerCorrect;
            return score;
        }
    }

	/// <summary>
	/// Rebuilding the tower.
	/// </summary>
	/// <param name="itemGroupThisItemHandler"></param>
	/// <returns>The tower.</returns>
	IEnumerator<float> rebuildingTower(ItemHandler1_4A HandleItem) {
        int ItemHandlerListCount = ItemHandlerList.Count;
        for (int i = 0; i < ItemHandlerListCount; i++) {
            ItemHandler1_4A itemHandler = ItemHandlerList[i];
	        itemHandler.enabled = true;
            itemHandler.ResetItemsPos();
            UpdateLayerOfItemsIEWhitoutRemove(itemHandler);
            itemHandler.itemToDrag.GetComponent<CanvasGroup>().blocksRaycasts = true;

        }
        towerGrid.position = startTowerPos;
        towerGrid.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        towerGrid.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        //yield return new WaitForSeconds (1f);
        yield return Timing.WaitForSeconds(1f);
        isPlaying = true;
    }


    IEnumerator UpdateLayerOfItemsIE(ItemHandler1_4A _itemHandler, bool _isRight) {

        if (_itemHandler != null)
            _itemHandler.removeItemInFront(_isRight);

        bool isBadItem = false;
        yield return new WaitForEndOfFrame();
        if (_itemHandler.slotCount > 0) {
            if (_itemHandler.amountBadItems < _itemHandler.maxBadItems && _itemHandler.slotCount >= 1) {
                float temp = Random.Range(0f, 100f);
                if (temp < chancesOfBadItem) {
                    // ("Bad Item");
                    _itemHandler.amountBadItems++;
                    _itemHandler.updateOtherHandlers();
                    _itemHandler.RemoveThisHandlerFromAll(_itemHandler);
                    _itemHandler.isRed = true;
                    _itemHandler.redItem();
                    isBadItem = true;
                } else {
                    //Debug.Log ("Normal Item Inside");
                    isBadItem = false;
                }
            } else {
                //Debug.Log ("normal Item");
                isBadItem = false;
            }
            float times = 0.0f;
            float duration = itemAlphaChangeDuration;
            AnimationCurve curve = itemAlphaChangeCurve;

            List<Color> startColors = new List<Color>();
            List<Color> endColors = new List<Color>();

            List<float> transparency = new List<float>();
            for (int i = 0; i < _itemHandler.slotCount; i++) {
                transparency.Add(transparentLevels[i]);
            }

            for (int i = 0; i < _itemHandler.slotCount; i++) {
                startColors.Add(_itemHandler.itemsOnSlot[i].GetComponent<ItemGroup1_4A>().GetColor());
                endColors.Add(new Vector4(1f, 1f, 1f, transparency[i]));
            }

            _itemHandler.itensGroup.Clear();

            List<GameObject> _list = _itemHandler.itemsOnSlot.ReturnReverseList();

            for (int i = 0; i < _itemHandler.slotCount; i++) {
                _itemHandler.itensGroup.Add(_list[i].GetComponent<ItemGroup1_4A>());
            }

            while (times < duration) {
                times += Time.deltaTime;
                float s = times / duration;

                for (int i = 0; i < _itemHandler.slotCount; i++) {

                    if (i == 0 && _itemHandler.isRed) {
                        Color _color = Color.Lerp(startColors[i], Color.gray, curve.Evaluate(s));
                        _itemHandler.itemsOnSlot[i].GetComponent<ItemGroup1_4A>().UpdateImage(_color);
                        _itemHandler.itemsOnSlot[i].GetComponent<ItemGroup1_4A>().toggleLock(true);
                    } else {
                        Color _color = Color.Lerp(startColors[i], endColors[i], curve.Evaluate(s));
                        _itemHandler.itemsOnSlot[i].GetComponent<ItemGroup1_4A>().UpdateImage(_color);
                    }

                }

                yield return Yielders.EndOfFrame;
            }

            List<GameObject> _list2 = _itemHandler.itemsOnSlot.ReturnReverseList();

            _itemHandler.UpdateDragItem();
            _itemHandler.blockDrag = false;

            _itemHandler.ActiveCollider2D();

            _itemHandler.floorParent.CheckFloorDone();
            isRemoving = false;
        } else {
            _itemHandler.hasEnded = true;
            _itemHandler.maxBadItems--;
            _itemHandler.updateOtherHandlers();
            _itemHandler.UpdateDragItem();
            //_itemHandler.blockDrag = false;
            _itemHandler.ActiveCollider2D();
            _itemHandler.floorParent.CheckFloorDone();
            isRemoving = false;
            //Debug.Log ("Sem item");
        }
    }


    public void UpdateLayerOfItemsIEWhitoutRemove(ItemHandler1_4A itemGroup) {

        int tempCount = itemGroup.slotCount;

        List<Color> endColors = new List<Color>();
        Color[] endColorArray = new Color[tempCount];

        List<float> transparency = new List<float>();
        float[] transparencyArray = new float[tempCount];

        for (int i = 0; i < tempCount; i++) {
            //transparency.Add (transparentLevels [i]);
            //endColors.Add(new Vector4(1f, 1f, 1f, transparency[i]));
            transparencyArray[i] = transparentLevels[i];
            if (endColorArray.Length > i && transparency.Count > i) {
                endColorArray[i] = new Vector4(1f, 1f, 1f, transparency[i]);
            }
        }

        itemGroup.itensGroup.Clear();

        List<GameObject> _list = itemGroup.itemsOnSlot.ReturnReverseList();
        //GameObject[] _listArray = itemGroup.itemsOnSlot.ReturnReverseList().ToArray();

        tempCount = itemGroup.slotCount;

        for (int i = 0; i < tempCount; i++) {
            itemGroup.itensGroup.Add(_list[i].GetComponent<ItemGroup1_4A>());
            //itemGroup.itensGroup.Add(_listArray[i].GetComponent<ItemGroup1_4A>());
            if (itemGroup.itemsOnSlot.Count > i && endColors.Count > i) {
                itemGroup.itemsOnSlot[i].GetComponent<ItemGroup1_4A>().UpdateImage(endColors[i].a);
            }
        }

        List<GameObject> _list2 = itemGroup.itemsOnSlot.ReturnReverseList();

        itemGroup.UpdateDragItem();
        //itemGroup.blockDrag = false;
        itemGroup.ActiveCollider2D();
        itemGroup.floorParent.CheckFloorDone();
        isRemoving = false;
    }

    IEnumerator endGame() {
        yield return new WaitUntil(() => itemHandlers.All(x => x.hasEnded) || hasEndedByTime);
        Debug.Log("Game Loop Ended");
        isPlaying = false;
        pauseButton.interactable = false;
        CancelInvoke();
        isPlaying = false;
        currentDificult++;
        int circlesOnSceneCount = circlesOnScene.Count;
        for (int i = 0; i < circlesOnSceneCount; i++) {
            Destroy(circlesOnScene[i]);
        }

        circlesOnScene.Clear();
        imagesLayerOnScene.Clear();
        itemHandlers.Clear();
        ItemHandlerList.Clear();

        yield return Yielders.Get(1f);
        if (hasEndedByTime || (currentDificult > dificults.Count - 1)) {

            log.StartTimerLudica(true);
            log.pontosLudica = scoreAmount;
            log.faseLudica = hasEndedByTime ? currentDificult : 4;

            switch (currentDificult)
            {
	            case 1:
		            nextManager.DialogInfoYear1.speeches[0] = speechInfoFinalLudica1;
		            nextManager.DialogInfoYear2.speeches[0] = speechInfoFinalLudica1;
		            nextManager.DialogInfoYear3.speeches[0] = speechInfoFinalLudica1;
		            break;
	            case 2:
		            nextManager.DialogInfoYear1.speeches[0] = speechInfoFinalLudica2;
		            nextManager.DialogInfoYear2.speeches[0] = speechInfoFinalLudica2;
		            nextManager.DialogInfoYear3.speeches[0] = speechInfoFinalLudica2;
		            break;
	            case 3:
		            nextManager.DialogInfoYear1.speeches[0] = speechInfoFinalLudica3;
		            nextManager.DialogInfoYear2.speeches[0] = speechInfoFinalLudica3;
		            nextManager.DialogInfoYear3.speeches[0] = speechInfoFinalLudica3;
		            break;
            }
			Debug.Log("Start Didática Game Loop");
            Timing.RunCoroutine(nextManager.CallNextInitGame(this));

        } else {
            _highlight.startChangeLevelAnimation(currentDificult + 1);
            yield return new WaitForSeconds(3f);
            StartCoroutine(beginGame());
            //updateStarAmount ();
        }
        pauseButton.interactable = true;

    }


    

	void updateItemHandlerList(){
		int circlesOnSceneCount = circlesOnScene.Count;
		for (int i = 0; i < circlesOnSceneCount; i++) {
			itemHandlers.Add (circlesOnScene [i].GetComponent<ItemHandler1_4A> ());
		}	
	}

	public IEnumerator scoreIncrease(int increase){

		//Debug.Log ("Start Increase Points");
		float times = 0.0f;
		int startPoints = scoreAmount;
		int scoreT = scoreAmount + increase;
		scoreAmount += increase;
		while (times < scoreIncreaseDuration)
		{
			times += Time.deltaTime;
			float s = times / scoreIncreaseDuration;

			int scory = (int)Mathf.Lerp (startPoints, scoreT, ScoreIncreaseCurve.Evaluate (s));
			scoreTextComp.text = scory.ToString ();
			yield return Yielders.EndOfFrame;
		}

        UpdateStars(scoreT);


    }

	public IEnumerator scoreReset(){
		//Debug.Log ("Start Increase Points");
		float times = 0.0f;
		int startPoints = scoreAmount;
		int scoreT = startScoreAmount;
		scoreAmount = startScoreAmount;
		while (times < scoreDecreaseDuration)
		{
			times += Time.deltaTime;
			float s = times / scoreDecreaseDuration;

			int scory = (int)Mathf.Lerp (startPoints, scoreT, ScoreDecreaseCurve.Evaluate (s));
			scoreTextComp.text = scory.ToString ();
			yield return Yielders.EndOfFrame;
		}

        UpdateStars(scoreT);

    }

	void DecreaseTimeOverSeconds(){
		if(isPlaying){
			timerSlider.value += 1;
			////Debug.Log("Time reduced");
		}
	}


	string ComboText(){
        stringFast.Clear();
        stringFast.Append("Combo ").Append(combo).Append("X");
        string newText = stringFast.ToString();
		return newText;
	}

	public IEnumerator<float> ShowComboText(){
			float times = 0.0f;
			while (times < showComboTextDuration) {
				times += Time.deltaTime;
				float s = times / showComboTextDuration;

				Vector3 newScale = Vector3.Lerp (Vector3.zero, Vector3.one, showComboTextCurve.Evaluate (s));
				comboTextComp.transform.localScale = newScale;

				yield return Timing.WaitForOneFrame;
			}
	}

	void ItemBonus(){
		StartCoroutine(CreateItemBonus());
	}

	IEnumerator CreateItemBonus(){
		if(!hasItemBonus && isPlaying){
			int ItemHandlerListCount = ItemHandlerList.Count;
			yield return new WaitUntil(() => isDragging == false);
			float _randomChances = Random.Range(0f,100f);
			if(_randomChances <= chancesOfBonus){
				int indexItem = Random.Range(0,ItemHandlerListCount);
				if (ItemHandlerList.Count > 0 && ItemHandlerList [indexItem] != null) {
					while (ItemHandlerList [indexItem].hasEnded == true && ItemHandlerList.Count > 0) {
						indexItem = Random.Range (0, ItemHandlerListCount - 1);
					}
				}
				if (ItemHandlerList.Count > 0 && ItemHandlerList [indexItem] != null) {
					itemWithBonusItem = ItemHandlerList [indexItem].ItemBonusHere ();
                    itemWithBonusItem.toggleBonusItem(true);
                    hasItemBonus = true;
					////Debug.Log("Item Bonus created!");
					OnBonusItemShow.Invoke ();
				}
			}
		}		

		Invoke("endItemBonus", 8f);
	}

	void endItemBonus(){
		if (itemWithBonusItem != null) {
			if (hasItemBonus && itemWithBonusItem.isActiveAndEnabled) {
				if (itemWithBonusItem != null) {
					itemWithBonusItem.isBonusItem = false;
                    itemWithBonusItem.toggleBonusItem(false);
                }
				//itemWithBonusItem.UpdateColor(Color.white);
				hasItemBonus = false;
			}
		}
	}

	void startRandomChest(){

		float randomTemp = Random.Range(0f, 100f);
		if (randomTemp <= chanceToRandomChest) {
			if(randomchest !=null){
				StopCoroutine(randomchest);
			}
			randomchest = RandomChests();
			StartCoroutine(randomchest);
		}

	}

	IEnumerator RandomChests(){
      
        //chests[_chestIndex].GetComponent<Animator> ().enabled = true;

        yield return new WaitUntil(predicate: () => isClosingChest == false);
		yield return new WaitUntil(() => isDragging == false);
		yield return new WaitUntil(() => checkBauOpen == false);
		yield return new WaitUntil(() => checkJucaBau == false);
		yield return new WaitUntil(() => checkZecaBau == false);
		yield return new WaitUntil(() => isOpeningChest == false);

		yield return new WaitUntil(() => chooseChestBonus == false);

		foreach (var item in partBauTransp)
		{
			item.SetActive(true);
			item.GetComponent<Animator>().SetBool(TransporTrue, true);

		}

		yield return Yielders.Get(0.8f);
	

		Color WhiteWithoutAlpha = new Vector4(1f,1f,1f,0f);

		float times = 0.0f;
		while (times < fadeInChestChangeDuration) {
			times += Time.deltaTime;
			float s = times / fadeInChestChangeDuration;
			int chestsCount = chests.Count;
			for (int i = 0; i < chestsCount; i++){
				chests[i].imageTamp.color = Color.Lerp(Color.white,WhiteWithoutAlpha, fadeOutChestChangeCurve.Evaluate(s));
				chests[i].imageComp.color = Color.Lerp(Color.white,WhiteWithoutAlpha, fadeOutChestChangeCurve.Evaluate(s));
				chests[i].textComp.color = Color.Lerp(Color.white,WhiteWithoutAlpha, fadeOutChestChangeCurve.Evaluate(s));
				chests[i].imageBau.color = Color.Lerp(Color.white,WhiteWithoutAlpha, fadeOutChestChangeCurve.Evaluate(s));
			}
		
			foreach (var item in partBauTransp)
			{
				item.GetComponent<Animator>().SetBool(TransporTrue,false);
			}

			yield return Yielders.EndOfFrame;
	
		}
		if(isPlaying){
			chestsParent.Shuffle();
			Vector3 _anchoredPosition = chests[0].GetComponent<RectTransform>().anchoredPosition;
			Vector2 _sizeDelta = chests[0].GetComponent<RectTransform>().sizeDelta;
			int chestsCount = chests.Count;
			for (int i = 0; i < chestsCount; i++){
				chests[i].transform.SetParent(chestsParent[i].transform);
				chests[i].rectComp.anchoredPosition = _anchoredPosition;
				chests[i].rectComp.sizeDelta = _sizeDelta;
				chests[i].transform.localScale = Vector3.one;
				chests[i].textComp.gameObject.transform.localScale = chestsParent[i].transform.localScale;
				chests[i].GetComponent<ChestHandler1_4A>().bauSort = chestsParent[i].GetComponent<BauNumber>().numbBau;
			}
			OnChestsChange.Invoke();
		}
			

		times = 0.0f;
		while (times < fadeInChestChangeDuration) {
			times += Time.deltaTime;
			float s = times / fadeInChestChangeDuration;
			int chestsCount = chests.Count;
			for (int i = 0; i < chestsCount; i++){
				chests[i].imageTamp.color = Color.Lerp(WhiteWithoutAlpha,Color.white, fadeOutChestChangeCurve.Evaluate(s));
				chests[i].imageComp.color = Color.Lerp(WhiteWithoutAlpha,Color.white, fadeOutChestChangeCurve.Evaluate(s));
				chests[i].textComp.color = Color.Lerp(WhiteWithoutAlpha,Color.white, fadeOutChestChangeCurve.Evaluate(s));
				chests[i].imageBau.color = Color.Lerp(WhiteWithoutAlpha,Color.white, fadeOutChestChangeCurve.Evaluate(s));
				
			}

			yield return Yielders.EndOfFrame;
		}
			
		Invoke(nameof(startRandomChest), timeToRandomizeChest);

		foreach (var item in partBauTransp)
		{
			item.SetActive(false);
		}	
	}

	void randomChestToCLose(){

		
		isClosingChest = true;
		if (hasChestClose || !isPlaying) return;
		float randomTemp = Random.Range(0f,100f);
		if (chestBonus != null) {
			chestBonus.isChestBonus = false;
			chestBonus.ToggleBonusParticle(false);
		}
		if (randomTemp <= chancesOfChestClose)
		{
			_chestIndex = Random.Range(0,chests.Count-1);
			while (chests[_chestIndex].isChestClose == true || chests[_chestIndex].isChestBonus == true){
				_chestIndex = Random.Range(0,chests.Count-1);
			}
			chests[_chestIndex].jucaFecharBau.enabled=true;
			chests[_chestIndex].jucaFecharBau.SetBool(FecharBau,true);
			chests[_chestIndex].jucaFecharBau.SetInteger(NumbBauSort,chests[_chestIndex].GetComponent<ChestHandler1_4A>().bauSort);
			StartCoroutine (TimeEnableBau());
		}

		else {
			Invoke(nameof(randomChestToCLose), timeToChestClose);
			isClosingChest = false;
		}

		foreach (var t in chests)
		{
			if (t.isChestBonus) {
				chestBonus = t;
			}
		}
	}
	IEnumerator TimeEnableBau(){
		yield return Yielders.Get(10f);
		chests[_chestIndex].jucaFecharBau.SetBool(FecharBau,false);
	}


	//Chamar no final da animação de fechar o bau.
	public void closeChest(){		
		chests[_chestIndex].isChestClose = true;
		chests[_chestIndex].ToggleRaycastTarget(false);
		_closedChest = chests[_chestIndex];
		hasChestClose = true;
		isClosingChest = false;
		float temp = Random.Range(rangeTimeToOpen.x,rangeTimeToOpen.y);
		Invoke(nameof(OpenChestAnimation), temp);
	}

	
	void OpenChestAnimation(){
		isOpeningChest = true;
		chests[_chestIndex].zecaFecharBau.SetBool(AbrirBau,true);
		StartCoroutine (TimeEnableBau());
	}


	//Chamar no final da animação de abertura do báu.
	public void OpenChest(){
		if(hasChestClose && isPlaying){		
		 	checkZecaBau=true;
			_closedChest.isChestClose = false;
			chests[_chestIndex].zecaFecharBau.enabled=true;
			_closedChest.ToggleRaycastTarget(true);
			hasChestClose = false;		
		}
		isOpeningChest = false;
		Invoke(nameof(randomChestToCLose), timeToChestClose);
	}


	void ChooseChest(){
       Debug.Log("Bau Bonus Starting");
       if (hasChestBonus || !isPlaying) return;
       float randomTemp = Random.Range(0f,100f);
		if (!(randomTemp <= chancesOfChestBonus)) return;
		int temp = chests.Count;
		for (int i = 0; i < temp; i++) {
			chests[i].isChestBonus = false;
			chests[i].ToggleBonusParticle(false);
		}

		chests2 = chests.ToList();
		chests2.Shuffle();

		if (!chests2[0].isChestClose) {
			chestBonus = chests2[0];
		} else if (!chests2[1].isChestClose) {
			chestBonus = chests2[1];
		} else if(!chests2[2].isChestClose) {
			chestBonus = chests2[2];
		}
		if (chestBonus == null) {
			Debug.Log("Bau Bonus Nulled");
		}
		chestBonus.isChestBonus = true;
		chestBonus.ToggleBonusParticle(true);

		hasChestBonus = true;
		Debug.Log("Bau Bonus");
		Timing.KillCoroutines(chestBonusEnd);
		chestBonusEnd = Timing.RunCoroutine(endChestBonus(), "ChestBonusEnd");


	}
	IEnumerator<float> endChestBonus(){
        yield return Timing.WaitForOneFrame;
        hasChestBonus = true;
        chestBonus.isChestBonus = true;
        chestBonus.particleBonusChest.SetActive(true);

        yield return Timing.WaitForSeconds(8f);

        if (hasChestBonus && isPlaying){
			chestBonus.isChestBonus = false;
            chestBonus.ToggleBonusParticle(false);
			hasChestBonus = false;
			OnChestOpens.Invoke();
            yield return Timing.WaitForOneFrame;
		}
	}

	string ChestNameByType(ItemType1_4A _type){
		switch (_type){
			case ItemType1_4A.books:
				return "Livros";
				break;
			case ItemType1_4A.cars:
				return "Carros";
				break;
			case ItemType1_4A.dolls:
				return "Bonecos";
				break;
			case ItemType1_4A.sports:
				return "Esportes";
				break;
			default:
				return "None";
				break;
		}
	}

	public void ShakeCameraOn(float sPower){

		//save position before start shake, 
		//this it's really important otherwise 
		//the sprite can goes away and will not return 
		//in native position
		originPosition = towerParent.transform.localPosition;

		//enable shaking and setting power
		shakeOn = true;
		shakePower = sPower;
	}

	// shake off
	public void ShakeCameraOff(){

		// shake off
		shakeOn = false;

		// set original position after 
		towerParent.transform.localPosition = originPosition;
	}

	public void ThrowItemAway(Transform _transformItem){
		StartCoroutine(ThrowItemAwayIE(_transformItem));
	}

	IEnumerator ThrowItemAwayIE(Transform _transformItem){

		Vector3 startPos = _transformItem.position;
		Vector3 endPos = _transformItem.position + posOffsetThrow;
        ItemGroup1_4A item = _transformItem.GetComponent<ItemGroup1_4A>();

        item.DisableBackgroundImage();
		float timer = 0.0f;
		while (timer < itemThrowAwayDuration)
		{
			timer += Time.deltaTime;
			float s = timer / itemThrowAwayDuration;

			if(_transformItem != null){
				item.DisableBackgroundImage();
				_transformItem.position = Vector3.Lerp(startPos,endPos,itemThrowAwayPositionCurve.Evaluate(s));
				_transformItem.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * 5f, itemThrowAwayScaleCurve.Evaluate(s));
			} else {
				break;
			}

			yield return Yielders.EndOfFrame;
		}

		if (_transformItem == null) yield break;
		_transformItem.localScale = Vector3.one;
		_transformItem.gameObject.SetActive(false);
		////Debug.Log("Item Errado Jogado Fora!");

	}

	public void checkItensPos(){
		int itemHandlersCount = itemHandlers.Count;
		for (int i = 0; i < itemHandlersCount; i++) {
			itemHandlers[i].checkDragItem();
		}
	}

    public void UpdateStars(int _score) {

        if (_score >= minigame.limit.x && _score < minigame.limit.y) {
            updateStarAmount(1);
        } else if (_score >= minigame.limit.y && _score < minigame.limit.z) {
            updateStarAmount(2);
        } else if (_score >= minigame.limit.z) {
            updateStarAmount(3);
        } else {
            updateStarAmount(0);
        }
    }

}
