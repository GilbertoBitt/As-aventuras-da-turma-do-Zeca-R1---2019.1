using System;
using DG.Tweening;
using MEC;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Controllers;
using MiniGames.Scripts;
using Sirenix.OdinInspector;
using TMPro;
using UniRx;
using UniRx.Toolkit;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

public class Manager1_4B : OverridableMonoBehaviour
{

    [FoldoutGroup("Geral")]
    public GameStateReactiveProperty currentGameState = new GameStateReactiveProperty(GameState.MainState);

    [FoldoutGroup("Geral")]
    public AnoLetivoStateReactiveProperty anoLetivo = new AnoLetivoStateReactiveProperty(AnoLetivoState.Ano1);

    [Serializable]
    public class GameStateReactiveProperty : ReactiveProperty<GameState>
    {
        public GameStateReactiveProperty()
        {
        }

        public GameStateReactiveProperty(GameState initialValue)
            :base(initialValue)
        {
        }
    }

    [Serializable]
    public class AnoLetivoStateReactiveProperty : ReactiveProperty<AnoLetivoState>
    {
        public AnoLetivoStateReactiveProperty()
        {
        }

        public AnoLetivoStateReactiveProperty(AnoLetivoState initialValue)
            :base(initialValue)
        {
        }
    }



    public enum GameState
    {
        MainState,
        MainStateAlternate,
        SecondState,
        SecondStateAlternate
    }

    public enum AnoLetivoState
    {
        Ano1 = 1,
        Ano2 = 2,
        Ano3 = 3
    }


    [FoldoutGroup("Geral")]
    public Button confirmButton;
    [FoldoutGroup("Geral")]
    public ScoreController scoreController;
    [FoldoutGroup("Geral")]
    public StarController starController;
    [FoldoutGroup("Geral")]
    public TextMeshProUGUI textEnunciadoComponent;
    [FoldoutGroup("Geral")]
    public Button showInformationButton;

    [FoldoutGroup("Geral")] public TextMeshProUGUI completeWordTextComponent;
    [FoldoutGroup("Geral")] public GameObject dragDropPanelContent;
    [FoldoutGroup("Geral")] public GameObject buttonsPanel;
    [AssetsOnly]
    [FoldoutGroup("Geral")] public ButtonTMPComponent buttonPrefab;
    [FoldoutGroup("Geral")] public Transform buttonParentTransform;
    [FoldoutGroup("Geral")] [ReadOnly] public List<ButtonTMPComponent> buttonInstances;
    public ButtonTMPComponentPool buttonPool;
    [FoldoutGroup("Geral")] public Image itemIconImage;
    [AssetList(Path = "MiniGames/1-4[A montanha de brinquedos]/Scripts/1-4B/Words/2ª Ano", AutoPopulate = true)]
    [FoldoutGroup("2ª ano")] public WordItem[] randomWordArray = new WordItem[1];
    [FoldoutGroup("2ª ano")] public int currentGameRound = 0;
    [FoldoutGroup("2ª ano")] public int maxRound = 3;

    public ReactiveCommand startGame;
    public ReactiveCommand updatedGame = new ReactiveCommand();

    public Manager1_4A previousManager;
    public GameObject panelInteragindo;
    public GameObject oldPanelDisable;
    public GameObject activateThisPanel;
    //public VerticalLayoutGroup verticalLayoutGroup;
    public GameObject panelCountSyllable;
#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
    [SeparatorAttribute("Controles")]
#endif
    public bool isPlaying = false;
    public bool isDragging = false;
    public int wordsDone = 0;
    public int wordsNeed = 3;
    public WordItem currentWord;
    public bool hasWordComplete = false;
    public bool isCountSyllables = false;
#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
    [SeparatorAttribute("Palavras")]
#endif
//    public Text entireWordTextcomp;
    public Transform syllablesParent;
    public GameObject syllablePrefab;
    public GridLayoutGroup syllableGrid;
    public List<WordItem> words = new List<WordItem>();
#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
    [SeparatorAttribute("Espaços Brancos")]
#endif
    public Transform blankSpaceParent;
    public GameObject blankSpacePrefab;
#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
    [SeparatorAttribute("Arraste")]
#endif
    public Transform dragParent;
#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
    [SeparatorAttribute("Pontuação")]
#endif
    public int amountScorePerCorrect = 0;
    public int amountScorePerCorrectNumber = 0;
    public int amountScorePerCorrectSyllabes = 0;
    public float scoreIncreaseDuration = 1.0f;
    public AnimationCurve ScoreIncreaseCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);

#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
    [SeparatorAttribute("Scroll(Numero de Letras)")]
#endif
    public Text scrollsnap;
    public int scrollNumber;
    public int playerNumberSelect = 0;
#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
    [SeparatorAttribute("Animação FadeIn")]
#endif
    public float fadeInTextDuration = 1.0f;
    public AnimationCurve fadeInTextCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
    [SeparatorAttribute("Instancias")]
#endif
    public List<SyllableHandler1_4B> syllablesInstance = new List<SyllableHandler1_4B>();
    public List<BlankSpace1_4B> blankSpaceInstance = new List<BlankSpace1_4B>();
#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
    [SeparatorAttribute("Audio")]
#endif
    public SoundManager _soundManager;
    public AudioClip[] audios;
#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
    [SeparatorAttribute("Painel de Vitoria")]
#endif
    public changeLevel highlight;
    public PanelDesafioControl painelDesafio;
    public int tutorIniMD_0;
    public GameObject tutorial;

#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
    [SeparatorAttribute("LOG")]
#endif
    public LogSystem log;
    public int idDificuldade;
    public int idHabilidade;

#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
    [SeparatorAttribute("Buttons")]
#endif
    public AudioClip buttomConfirmClipAudio;
    public Button buttonIncrease;
    public Button buttonDecrease;
    bool checkpassAudio;
    //public TutorMontanha TutorMontanha2;

#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
    [SeparatorAttribute("-- new")]
#endif
//    public Text comandTextComp;
//    public Text completeWordTextComp;
//    public Text TextEnumComp;
    public WordItem[] routineOneList = new WordItem[10];
    public WordItem[] routineTwoList = new WordItem[10];
    public int TotalRounds = 10;
    public int currentRound = 0;
    public Queue<SyllableHandler1_4B> blueBoxPool = new Queue<SyllableHandler1_4B>();
    public SyllableHandler1_4B[] blueBoxInstances = new SyllableHandler1_4B[0];
    public Queue<BlankSpace1_4B> emptyBoxPool = new Queue<BlankSpace1_4B>();
    public BlankSpace1_4B[] emptyBoxInstances = new BlankSpace1_4B[0];
    public BlueBoxPool poolBlueBox;
    public List<SyllableHandler1_4B> blueBoxInstancesOnScene;
    public BlankBoxPool poolBlankBox;
    public List<BlankSpace1_4B> blankInstancesOnScene;

    public int poolSize = 12;
    public GameObject prefabBlueBox;
    public SyllableHandler1_4B prefabBlueBoxInstance;
    public GameObject prefabEmptyBox;
    public BlankSpace1_4B prefabEmptyBoxInstance;
    public Transform poolParent;
    public Transform blueBoxParent;
    public Transform emptyBoxParent;

    public Button confirmButtonRoutine2;
    public CanvasGroup nexPanelGroupComp;

    [FoldoutGroup("2ª ano")]
    public ReactiveDictionary<string, bool> wordsToFind = new ReactiveDictionary<string, bool>();

    public void InitGameConfig()
    {
        buttonPool = new ButtonTMPComponentPool(buttonPrefab, buttonParentTransform);
        //StartConfiguration
        startGame = new ReactiveCommand();
        updatedGame = new ReactiveCommand();
        switch (anoLetivo.Value)
        {
            case AnoLetivoState.Ano1:
                SuffleAndGenerateFirstRoutine();
                SetupPools();
                startGame.Subscribe(OnGameStart);
                updatedGame.Subscribe(updated =>
                {
                    if (!isPlaying) return;
                    if (emptyBoxInstances.All(x => x.hasDrop == true) && !hasWordComplete) {
                        hasWordComplete = true;
                        confirmButton.interactable = true;
                    } else if (emptyBoxInstances.Any(x => x.hasDrop != true) && hasWordComplete) {
                        hasWordComplete = false;
                        confirmButton.interactable = false;
                    }
                });

                this.UpdateAsObservable().Subscribe(unit => { updatedGame?.Execute(); });
                break;
            case AnoLetivoState.Ano2:

                blueBoxInstancesOnScene = new List<SyllableHandler1_4B>();
                blankInstancesOnScene = new List<BlankSpace1_4B>();

                #region SetupPool
                //Preload and config pool of boxes

                poolBlueBox = new BlueBoxPool(prefabBlueBoxInstance, blueBoxParent, this);

                poolBlankBox = new BlankBoxPool(prefabEmptyBoxInstance, emptyBoxParent, this);

                //boxes preload ended
                #endregion

                randomWordArray.Shuffle();

                startGame.Subscribe(StartLoopSecondYear);

                dragDropPanelContent.SetActive(true);



                break;
            case AnoLetivoState.Ano3:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        //Bind to UniRX
        currentGameState.Where(state => state == GameState.MainState).Subscribe(OnMainState);
        currentGameState.Where(state => state == GameState.MainStateAlternate).Subscribe(OnMainStateAlternate);
        currentGameState.Where(state => state == GameState.SecondState).Subscribe(OnSecondState);
        currentGameState.Where(state => state == GameState.SecondStateAlternate).Subscribe(OnSecondStateAlternate);

        startGame.Execute();


    }

    private void StartLoopSecondYear(Unit obj)
    {
        if (wordsToFind == null)
        {
            wordsToFind = new ReactiveDictionary<string, bool>();
        }
        wordsToFind.Clear();
        //Return boxes used to pool.
        blueBoxInstancesOnScene.ForEach(box =>
        {
            poolBlueBox.Return(box);
            blueBoxInstancesOnScene.Remove(box);
        });

        blankInstancesOnScene.ForEach(box =>
        {
            poolBlankBox.Return(box);
            blankInstancesOnScene.Remove(box);
        });

        currentGameRound++;

        if (currentGameRound <= maxRound)
        {
            currentWord = randomWordArray[currentGameRound];
        }

        foreach (var word in currentWord.alternativeWordsContent.alternativeWords)
        {
            wordsToFind.Add(word.ToLower(), false);
        }

        var alternativeWords = currentWord.alternativeWordsContent.alternativeWords.Length;
        textEnunciadoComponent.DOText($"Forme {alternativeWords} {(alternativeWords == 1 ? "nova palavra" : "novas palavras")}", 1f);

        //instance blue box for syllable

        var shuffleSyllables = currentWord.syllables.Shuffle();

        foreach (var syllable in shuffleSyllables)
        {
            var blueBoxInstance = poolBlueBox.Rent();
            blueBoxInstance.UpdateTextContent(syllable.ToUpper());
            blueBoxInstance.DoFade(1f);
            blueBoxInstancesOnScene.Add(blueBoxInstance);

            var emptySpace = poolBlankBox.Rent();
            emptySpace.DoFade(1f);
            blankInstancesOnScene.Add(emptySpace);
        }

        //Set configs for onClickConfirmButton

        this.UpdateAsObservable()
            .Where(unit => isPlaying && blankInstancesOnScene.TrueForAll(box => box.hasDrop && box.thisSyllable != null))
            .Subscribe(unit => { confirmButton.interactable = true; });

        this.UpdateAsObservable().Where(unit =>
                isPlaying && !blankInstancesOnScene.TrueForAll(box => box.hasDrop && box.thisSyllable != null))
            .Subscribe(unit => { confirmButton.interactable = false; });

        confirmButton.OnClickAsObservable().Subscribe(CorrectCurrentWord);

    }

    private void CorrectCurrentWord(Unit unit)
    {
        //correction method to completed word

        //Get value guessed
        var playerGuessWord = blankInstancesOnScene.Where(item => item.thisSyllable != null).Aggregate(string.Empty, (current, item) => current + item.thisSyllable.textComp.text).ToLower();

        //confirm if word has or not this alternative word.
        var hasWord = wordsToFind.Keys.Contains(playerGuessWord);

        if (hasWord)
        {
            Debug.Log("palavra encontrada");
        }
        else
        {
            Debug.Log("failed to find");
        }

    }

    private void OnGameStart(Unit unit)
    {
        switch (anoLetivo.Value)
        {
            case AnoLetivoState.Ano1:
            {
                dragDropPanelContent.SetActive(true);
                isPlaying = false;
                //Reconfigurar Pool.
                int temp = blueBoxInstances.Length;
                for (int i = 0; i < temp; i++) {
                    blueBoxInstances[i].ResetToDefault();
                    blueBoxPool.Enqueue(blueBoxInstances[i]);
                    blueBoxInstances[i] = null;
                    emptyBoxInstances[i].ResetToDefault();
                    emptyBoxPool.Enqueue(emptyBoxInstances[i]);
                    emptyBoxInstances[i] = null;
                }

                blueBoxInstances = new SyllableHandler1_4B[0];
                emptyBoxInstances = new BlankSpace1_4B[0];

                if (currentRound < 10) {
                    if (currentRound < 5) {

                        //Iniciar Rotina de Letras.

                        //Iniciando Sequencia de animação para mudança do texto do comando(Titulo).
                        Sequence textFadeChange = DOTween.Sequence();
                        if (textEnunciadoComponent.color.a > 0f) {
                            textFadeChange.Append(textEnunciadoComponent.DOFade(0f, .3f));
                        }
                        textFadeChange.AppendCallback(() => ChangeTextTitle("Forme a Palavra"));
                        textFadeChange.Append(textEnunciadoComponent.DOFade(1f, .3f));

                        //Configurando Didatica com a palavra escolhida.
                        currentWord = routineOneList[currentRound];
                        blueBoxInstances = new SyllableHandler1_4B[currentWord.CountLetters];
                        emptyBoxInstances = new BlankSpace1_4B[currentWord.CountLetters];
                        currentWord.letters.Shuffle();
                        for (int i = 0; i < currentWord.CountLetters; i++) {
                            blueBoxInstances[i] = blueBoxPool.Dequeue();
                            blueBoxInstances[i].syllable = currentWord.letters[i].ToUpper();
                            blueBoxInstances[i].UpdateTextContent();
                            blueBoxInstances[i].transform.SetParent(blueBoxParent);
                            blueBoxInstances[i].DoFade(1f);
                            emptyBoxInstances[i] = emptyBoxPool.Dequeue();
                            emptyBoxInstances[i].transform.SetParent(emptyBoxParent);
                            emptyBoxInstances[i].DoFade(1f);
                        }

                        itemIconImage.sprite = currentWord.itemSprite;
                        itemIconImage.DOFade(1f, 0.5f);

                        //Iniciando Rotina 1.

                        currentGameState.Value = GameState.MainState;
                        isPlaying = true;
                        confirmButton.interactable = false;

                    } else {
                        //Iniciar Rotina de Silabas.
                        hasWordComplete = false;
                        Sequence TextFadeChange = DOTween.Sequence();
                        if (textEnunciadoComponent.color.a > 0f) {
                            TextFadeChange.Append(textEnunciadoComponent.DOFade(0f, .3f));
                        }
                        TextFadeChange.AppendCallback(() => ChangeTextTitle("Organize as Sílabas"));
                        TextFadeChange.Append(textEnunciadoComponent.DOFade(1f, .3f));

                        currentWord = routineTwoList[currentRound];
                        blueBoxInstances = new SyllableHandler1_4B[currentWord.CountSyllables];
                        emptyBoxInstances = new BlankSpace1_4B[currentWord.CountSyllables];
                        currentWord.syllables.Shuffle();
                        for (int i = 0; i < currentWord.CountSyllables; i++) {
                            blueBoxInstances[i] = blueBoxPool.Dequeue();
                            blueBoxInstances[i].syllable = currentWord.syllables[i].ToUpper();
                            blueBoxInstances[i].UpdateTextContent();
                            blueBoxInstances[i].transform.SetParent(blueBoxParent);
                            blueBoxInstances[i].DoFade(1f);
                            emptyBoxInstances[i] = emptyBoxPool.Dequeue();
                            emptyBoxInstances[i].transform.SetParent(emptyBoxParent);
                            emptyBoxInstances[i].DoFade(1f);

                        }
                        itemIconImage.sprite = currentWord.itemSprite;
                        itemIconImage.DOFade(1f, 0.5f);
                        //Iniciando Rotina 1.
                        currentGameState.Value = GameState.MainStateAlternate;
                        isPlaying = true;
                        confirmButton.interactable = false;
                    }
                } else {
                    //Iniciar Interagindo.
                    endDitatica();
                }
                break;
            }

            case AnoLetivoState.Ano2:
                break;
            case AnoLetivoState.Ano3:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void OnSecondStateAlternate(GameState obj)
    {
        switch (anoLetivo.Value)
        {
            case AnoLetivoState.Ano1:
                UpdateEnunciado("Quantas sílabas a palavra abaixo tem?");
                for (int i = 0; i <= 10; i++)
                {
                    var instance = buttonPool.Rent();
                    instance.textComponent.text = i.ToString();
                    var indexValue = i;
                    instance.buttonComponent.OnPointerClickAsObservable().Subscribe(data =>
                    {
                        scrollNumber = indexValue;
                        ConfirmButtonRoutineTwo(Unit.Default);
                    });
                    buttonInstances.Add(instance);
                }
                break;
            case AnoLetivoState.Ano2:
                break;
            case AnoLetivoState.Ano3:
                break;
            default:
                Debug.Break();
                throw new ArgumentOutOfRangeException();
        }
    }

    private void OnSecondState(GameState obj)
    {
        switch (anoLetivo.Value)
                {
                    case AnoLetivoState.Ano1:
                        UpdateEnunciado("Forme a palavra de acordo com o item abaixo.");
                        for (int i = 0; i <= 10; i++)
                        {
                            var instance = buttonPool.Rent();
                            instance.textComponent.text = i.ToString();
                            var indexValue = i;
                            instance.buttonComponent.OnPointerClickAsObservable().Subscribe(data =>
                            {
                                scrollNumber = indexValue;
                                ConfirmButtonRoutineTwo(Unit.Default);
                            });
                            buttonInstances.Add(instance);
                        }
                        break;
                    case AnoLetivoState.Ano2:
                        break;
                    case AnoLetivoState.Ano3:
                        break;
                    default:
                        Debug.Break();
                        throw new ArgumentOutOfRangeException();
                }
    }

    CompositeDisposable disposables = new CompositeDisposable();
    private void OnMainStateAlternate(GameState state)
    {
        switch (anoLetivo.Value)
        {
            case AnoLetivoState.Ano1:
                UpdateEnunciado("Forme a palavra de acordo com o item abaixo.");
                buttonInstances.ForEach(component =>
                {
                    buttonPool.Return(component);
                });
                disposables.Dispose();
                confirmButton.OnClickAsObservable().Subscribe(unit => { WordSillablesCorrection(); });
                break;
            case AnoLetivoState.Ano2:
                break;
            case AnoLetivoState.Ano3:
                break;
            default:
                Debug.Break();
                throw new ArgumentOutOfRangeException();
        }
    }

    private void OnMainState(GameState state)
    {
        switch (anoLetivo.Value)
        {
            case AnoLetivoState.Ano1:
                UpdateEnunciado("Forme a palavra de acordo com o item abaixo.");
                buttonInstances.ForEach(component =>
                {
                    buttonPool.Return(component);
                });
                disposables.Dispose();
                confirmButton.OnClickAsObservable().Subscribe(unit => { WordDraggedCorretion(); });
                break;
            case AnoLetivoState.Ano2:
                break;
            case AnoLetivoState.Ano3:
                break;
            default:
                Debug.Break();
                throw new ArgumentOutOfRangeException();
        }
    }

    public void UpdateEnunciado(string textTo)
    {
        textEnunciadoComponent.DOFade(0f, 0.3f).OnComplete(() =>
        {
            textEnunciadoComponent.SetText(textTo);
            textEnunciadoComponent.DOFade(1f, 0.3f);
        });
    }


    public override void UpdateMe() {

    }


    public void InitGame()
    {
//        if (PlayerPrefs.HasKey("TutorMD_0") == false)
//        {
//            oldPanelDisable.GetComponent<GraphicRaycaster>().enabled = false;
//            activateThisPanel.GetComponent<GraphicRaycaster>().enabled = false;
//            PlayerPrefs.SetInt("TutorMD_0", 0);
//        }
//        else
//        {
//            PlayerPrefs.SetInt("TutorMD_0", 1);
//            //tutorIniMD_0 = PlayerPrefs.GetInt("TutorMD_0",1);
//        }
//
//        log.StartTimerDidatica(true);
//        oldPanelDisable.SetActive(false);
//        activateThisPanel.SetActive(true);
//        if (PlayerPrefs.HasKey("TutorM_Didatica") == false)
//        {
//            oldPanelDisable.GetComponent<GraphicRaycaster>().enabled = false;
//            activateThisPanel.GetComponent<GraphicRaycaster>().enabled = false;
//            PlayerPrefs.SetInt("TutorM_Didatica", 0);
//            this.tutorial.GetComponent<TutorMontanha>().btPulartext.text = "Iniciar";
//            this.tutorial.GetComponent<TutorMontanha>().tutorNumber = 7;
//            foreach (var item in tutorial.GetComponent<TutorMontanha>().gTutor)
//            {
//                item.SetActive(true);
//            }
//
//            this.tutorial.GetComponent<TutorMontanha>().gTutor[6].SetActive(false);
//            this.tutorial.GetComponent<GraphicRaycaster>().enabled = true;
//            this.tutorial.SetActive(true);
//            Time.timeScale = 0f;
//        }
//        else
//        {
//            PlayerPrefs.SetInt("TutorM_Didatica", 1);
//        }

        oldPanelDisable.GetComponent<GraphicRaycaster>().enabled = false;
        oldPanelDisable.SetActive(false);
        activateThisPanel.SetActive(true);
        nexPanelGroupComp.blocksRaycasts = false;
        nexPanelGroupComp.interactable = false;
        nexPanelGroupComp.alpha = 0f;
        scoreController.amountValue.Value = previousManager.scoreAmount;
        starController.amountValue.Value = previousManager.starAmount;
        InitGameConfig();
    }

    public void ChangeTextTitle(string _text) {
        textEnunciadoComponent.text = _text;
    }

    public void SuffleAndGenerateFirstRoutine() {
        routineOneList = words.Where(x =>
            x.buildWithLetters).ToArray();
        routineOneList.Shuffle();
        int amountWords = routineOneList.Length;


        routineTwoList = words.Where(x =>
            x.wordClassification.classification == WordItem.ClassificationBySyllables.Dissilabas ||
            x.wordClassification.classification == WordItem.ClassificationBySyllables.Trissilabas ||
            x.wordClassification.classification == WordItem.ClassificationBySyllables.Polissilaba).ToArray();
        routineTwoList.Shuffle();
        amountWords = routineTwoList.Length;
    }

    public void SetupPools() {
        //Blue box pool Setup;
        for (int i = 0; i < poolSize; i++) {
            GameObject temp = Instantiate(prefabBlueBox, new Vector3(1000, 1000, poolParent.position.z), Quaternion.identity, poolParent);
            SyllableHandler1_4B tempHandler = temp.GetComponent<SyllableHandler1_4B>();
            tempHandler.thisCanvasGroup.alpha = 0f;
            tempHandler.manager = this;
            blueBoxPool.Enqueue(tempHandler);
        }

        for (int i = 0; i < poolSize; i++) {
            GameObject temp = Instantiate(prefabEmptyBox, new Vector3(1000, 1000, poolParent.position.z), Quaternion.identity, poolParent);
            BlankSpace1_4B tempHandler = temp.GetComponent<BlankSpace1_4B>();
            tempHandler.manager = this;
            tempHandler.thisImageComp.color = new Color(1f, 1f, 1f, 0f);
            emptyBoxPool.Enqueue(tempHandler);
        }

    }

//    public IEnumerator<float> StartDidatic() {
//
//
//    }

    public void ConfirmButtonRoutine(Unit unit) {
        _soundManager.startSoundFX(buttomConfirmClipAudio);
        confirmButton.interactable = false;
        isPlaying = false;

        if (currentRound < 5)
        {
            //correção de palavras.
            WordDraggedCorretion();
        } else
        {
            WordSillablesCorrection();
        }
    }

    private void WordSillablesCorrection()
    {
        //correção de palavras.
        string fullPlayerWordGuess = emptyBoxInstances.Where(item => item.thisSyllable != null).Aggregate(string.Empty, (current, item) => current + item.thisSyllable.textComp.text);

        fullPlayerWordGuess = ToTitleCase(fullPlayerWordGuess);
        currentWord.word = ToTitleCase(currentWord.word);

        Debug.Log("Respostas do Jogador: [" + fullPlayerWordGuess + "] \nResposta correta: [" + currentWord.word + "]");

        if (string.Equals(fullPlayerWordGuess, currentWord.word, StringComparison.CurrentCultureIgnoreCase))
        {
            //Acertou a palavra.
            log.SaveEstatistica(9, 1, true);
            _soundManager.startSoundFX(audios[0]);
            highlight.startCerto("Parabéns! Você acertou!");
//                textScoreComp.DOTextInt(scoreAmount, (scoreAmount + amountScorePerCorrectSyllabes), 0.5f);
            scoreController.amountValue.Value += amountScorePerCorrectSyllabes;
            //Iniciar Contagem de Letras.
            textEnunciadoComponent.text = "Quantas sílabas tem essa palavra?";
            currentGameState.Value = GameState.SecondStateAlternate;
            StartRoutineTwo();
        }
        else
        {
            //Errou a palavra.
            log.SaveEstatistica(9, 1, false);
            _soundManager.startSoundFX(audios[1]);
            currentRound++;
            highlight.startErrado("Você errou! O correto era: " + ToTitleCase(currentWord.word) + string.Empty);
            currentGameState.Value = GameState.MainStateAlternate;
            //Iniciar novo Round.
//                Timing.RunCoroutine(StartDidatic());
            startGame.Execute();
        }
    }

    private void WordDraggedCorretion()
    {
        string fullPlayerWordGuess = emptyBoxInstances.Where(item => item.thisSyllable != null).Aggregate(string.Empty, (current, item) => current + item.thisSyllable.textComp.text);

        fullPlayerWordGuess = ToTitleCase(fullPlayerWordGuess);
        currentWord.word = ToTitleCase(currentWord.word);

        Debug.Log("Respostas do Jogador: [" + fullPlayerWordGuess + "] \nResposta correta: [" + currentWord.word + "]");

        if (string.Equals(fullPlayerWordGuess, currentWord.word, StringComparison.CurrentCultureIgnoreCase))
        {
            //Acertou a palavra.
            log.SaveEstatistica(9, 1, true);
            _soundManager.startSoundFX(audios[0]);
            highlight.startCerto("Parabéns! Você acertou!");
//                textScoreComp.DOTextInt(scoreAmount, (scoreAmount + amountScorePerCorrectSyllabes), 0.5f);
            scoreController.amountValue.Value += amountScorePerCorrectSyllabes;
            //Iniciar Contagem de Letras.
            textEnunciadoComponent.text = "Quantas letras tem essa palavra?";
            currentGameState.Value = GameState.SecondState;
            StartRoutineTwo();
        }
        else
        {
            //Errou a palavra.
            log.SaveEstatistica(9, 1, false);
            _soundManager.startSoundFX(audios[1]);
            currentRound++;
            highlight.startErrado("Você errou! O correto era: " + ToTitleCase(currentWord.word) + string.Empty);
            currentGameState.Value = GameState.MainState;
            //Iniciar novo Round.
//                Timing.RunCoroutine(StartDidatic());
            startGame.Execute();
        }
    }

    public void ConfirmButtonRoutineTwo(Unit unit) {
        _soundManager.startSoundFX(buttomConfirmClipAudio);
        //scrollNumber
        if (currentRound < 5)
        {
            //correção numero de letras.
            CorrectionAmountLetters();
        } else
        {
            //correção numero de silabas.
            CorrectionAmountSyllables();
        }
    }

    private void CorrectionAmountSyllables()
    {
        if (scrollNumber == currentWord.CountSyllables)
        {
            //acertou o numero de letras.
            _soundManager.startSoundFX(audios[0]);
//                textScoreComp.DOTextInt(scoreAmount, (scoreAmount + amountScorePerCorrectSyllabes), 0.5f);
            scoreController.amountValue.Value += amountScorePerCorrectSyllabes;
            log.SaveEstatistica(10, 1, true);
            highlight.startCerto("Parabéns! Você acertou!");
            currentGameState.Value = GameState.SecondStateAlternate;
            EndRoutineTwo();
        }
        else
        {
            //errou o numero de letras.
            _soundManager.startSoundFX(audios[1]);
            log.SaveEstatistica(10, 1, true);
            highlight.startErrado("Você errou o certo era: " + currentWord.CountSyllables);
            currentGameState.Value = GameState.MainStateAlternate;
            EndRoutineTwo();
        }
    }

    private void CorrectionAmountLetters()
    {
        if (scrollNumber == currentWord.CountLetters)
        {
            //acertou o numero de letras.
            _soundManager.startSoundFX(audios[0]);
//                textScoreComp.DOTextInt(scoreAmount, (scoreAmount + amountScorePerCorrectSyllabes), 0.5f);
            scoreController.amountValue.Value += amountScorePerCorrectSyllabes;
            log.SaveEstatistica(10, 1, true);
            highlight.startCerto("Parabéns! Você acertou!");
            currentGameState.Value = GameState.SecondState;
            EndRoutineTwo();
        }
        else
        {
            //errou o numero de letras.
            _soundManager.startSoundFX(audios[1]);
            log.SaveEstatistica(10, 1, true);
            highlight.startErrado("Você errou o certo era: " + currentWord.CountLetters);
            currentGameState.Value = GameState.MainState;
            EndRoutineTwo();
        }
    }

    public void EndRoutineTwo() {
        //completeWordTextComp.text = currentWord.completeWord.ToUpper();        
        confirmButton.interactable = true;
        confirmButton.gameObject.SetActive(true);
        completeWordTextComponent.DOFade(0f, 0.3f);
        nexPanelGroupComp.blocksRaycasts = false;
        nexPanelGroupComp.interactable = false;
        nexPanelGroupComp.DOFade(0f, 0.3f);
        currentRound++;
//        Timing.RunCoroutine(StartDidatic());
        startGame.Execute();
    }

    public void StartRoutineTwo() {
        completeWordTextComponent.text = currentWord.word.ToUpper();
        scrollNumber = 0;
        scrollsnap.text = "0";
        confirmButton.interactable = false;
        confirmButton.gameObject.SetActive(false);
        Sequence routine2Start = DOTween.Sequence();
        routine2Start.AppendCallback(() => FadesSecondRoutine());
        routine2Start.AppendInterval(0.5f);
        routine2Start.Append(completeWordTextComponent.DOFade(1f, 1f));
        routine2Start.Play();
        nexPanelGroupComp.blocksRaycasts = true;
        nexPanelGroupComp.interactable = true;

    }

    public void FadesSecondRoutine() {
        nexPanelGroupComp.DOFade(1f, 0.3f);
        int temp = blueBoxInstances.Length;
        for (int i = 0; i < temp; i++) {
            blueBoxInstances[i].DoFade(0f);
            emptyBoxInstances[i].DoFade(0f);
        }
        itemIconImage.DOFade(0f, 0.5f);
    }

    public IEnumerator StartThis() {
        isPlaying = false;

        if (wordsDone >= wordsNeed) {
            endDitatica();
        }

        CleanWords();

        currentWord = words[wordsDone];

        List<string> syllablesTemp = currentWord.syllables.ToList();
        syllablesTemp.Shuffle();

        completeWordTextComponent.text = currentWord.word.ToUpper();

        Color colorTemp = completeWordTextComponent.color;
        colorTemp.a = 0f;
        completeWordTextComponent.color = colorTemp;

        for (int i = 0; i < currentWord.CountSyllables; i++) {
            GameObject syllable = Instantiate(syllablePrefab, Vector3.zero, Quaternion.identity, syllablesParent);
            syllable.transform.localScale = Vector3.one;
            SyllableHandler1_4B handler = syllable.GetComponent<SyllableHandler1_4B>();
            handler.manager = this;
            handler.syllable = syllablesTemp[i].ToUpper();
            handler.UpdateTextContent();
            syllablesInstance.Add(handler);
        }

        for (int i = 0; i < currentWord.CountSyllables; i++) {
            GameObject blankSpace = Instantiate(blankSpacePrefab, Vector3.zero, Quaternion.identity, blankSpaceParent);
            blankSpace.transform.localScale = Vector3.one;
            BlankSpace1_4B handler = blankSpace.GetComponent<BlankSpace1_4B>();
            handler.manager = this;
            blankSpaceInstance.Add(handler);
        }

        yield return new WaitForSeconds(.5f);
        isPlaying = true;
        hasWordComplete = false;
        isCountSyllables = false;

    }

    public void onValueScrollChange() {
        playerNumberSelect = scrollNumber;
    }

    /*public IEnumerator<float> scoreIncrease(int increase) {

        //Debug.Log ("Start Increase Points");
        float times = 0.0f;
        int startPoints = scoreAmount;
        int scoreT = scoreAmount + increase;
        scoreAmount += increase;

        log.AddPontosPedagogica(increase);
        while (times < scoreIncreaseDuration) {
            times += Time.deltaTime;
            float s = times / scoreIncreaseDuration;

            int scory = (int)Mathf.Lerp(startPoints, scoreT, ScoreIncreaseCurve.Evaluate(s));
            textScoreComp.text = scory.ToString();
            yield return Timing.WaitForOneFrame;
        }

    }*/

    /*void updateStarAmount() {

        if (starsAmount < 3) {
            for (int i = 0; i < 3; i++) {
                starsParent.GetChild(i).GetComponent<Image>().sprite = previousManager.spriteEmptyStar;
            }

            for (int i = 0; i < starsAmount; i++) {
                starsParent.GetChild(i).GetComponent<Image>().sprite = previousManager.spriteFullStar;
            }
        }

    }*/

    public void confirmButton02() {
        if (scrollNumber == currentWord.CountSyllables) {
            //Debug.Log("Correto o numero de silabas!");
            //scoreAmount += amountScorePerCorrectSyllabes;
            _soundManager.startSoundFX(audios[0]);
//            StartCoroutine(scoreIncrease(amountScorePerCorrectSyllabes));
            scoreController.amountValue.Value += amountScorePerCorrectSyllabes;
            log.SaveEstatistica(10, 1, true);
            highlight.startCerto("Parabéns! Você acertou!");
        } else {
            //Debug.Log("Errado o numero de silabas!");
            _soundManager.startSoundFX(audios[1]);
            log.SaveEstatistica(10, 1, true);
            highlight.startErrado("Você errou o certo era: " + currentWord.CountSyllables.ToString());
        }
        StartCoroutine(GameCorrectionSyllable());
    }

    IEnumerator GameCorrectionSyllable() {
        yield return new WaitForSeconds(.5f);
        scrollNumber = 0;
        scrollsnap.text = "0";

        panelCountSyllable.SetActive(false);
        wordsDone++;
        yield return new WaitForSeconds(.5f);
        StartCoroutine(StartThis());
    }

    public void confirmButton01() {
        if ((blankSpaceInstance.Any(x => x.hasDrop != true))) {
            //Debug.Log("Defina o numero de silabas!");
        } else {
            //Debug.Log("Corrigir!");

            StartCoroutine(GameCorrectionWord());
        }
    }

    IEnumerator GameCorrectionWord() {
        List<SyllableHandler1_4B> syllabeTemp = new List<SyllableHandler1_4B>();
        int inscreaseamount = 0;
        bool isWordWrong = false;
        int blankSpaceInstanceCount = blankSpaceInstance.Count;

        string correctWords = string.Empty;
        string currentWords = currentWord.word.ToUpper();

        for (int i = 0; i < blankSpaceInstanceCount; i++) {
            correctWords += blankSpaceInstance[i].thisSyllable.textComp.text;
        }

        correctWords = correctWords.ToUpper();
        if (correctWords == currentWords) {
            isWordWrong = false;
            log.SaveEstatistica(9, 1, true);
        } else {
            isWordWrong = true;
            log.SaveEstatistica(9, 1, false);
        }


        for (int i = 0; i < blankSpaceInstanceCount; i++) {
            syllabeTemp.Add(blankSpaceInstance[i].thisSyllable);
        }

        if (syllabeTemp.Count >= currentWord.syllables.Length) {
            for (int i = 0; i < syllabeTemp.Count; i++) {
                if (syllabeTemp[i].syllable != currentWord.syllables[i].ToUpper()) {
                    break;
                }
            }
        }
        Debug.Log("THIs");
        if (isWordWrong) {
            //Debug.Log("Palavra errada!");
            _soundManager.startSoundFX(audios[1]);
            wordsDone++;
            isPlaying = false;
            StartCoroutine(StartThis());
            highlight.startErrado("Você errou! O correto era: " + currentWords + string.Empty);
            //Debug.Log ("THISE");
        }

        if (isWordWrong == false) {
            panelCountSyllable.SetActive(true);
            //Debug.Log("Palavra Certa!");			
            //StartCoroutine(scoreIncrease(amountScorePerCorrect));
            inscreaseamount += amountScorePerCorrect;
            _soundManager.startSoundFX(audios[0]);
            isCountSyllables = true;
            isPlaying = true;
            Debug.Log("THISE");
            StartCoroutine(AskAmountSyllables());
            highlight.startCerto("Parabéns! Você acertou!");
        }

        if (inscreaseamount > 0) {
//            StartCoroutine(scoreIncrease(inscreaseamount));
            scoreController.amountValue.Value += inscreaseamount;
        }

        yield return new WaitForSeconds(.5f);
    }

    IEnumerator AskAmountSyllables() {
        isPlaying = true;
        //buttonIncrease.interactable = true;
        // buttonDecrease.interactable = true;
        //entireWordTextcomp.color = Color.black;

        Color _color = Color.black;
        _color.a = 0f;
        Color blankStartColor = blankSpaceInstance[0].thisImageComp.color;
        float times = 0.0f;

        Color colorTemp = completeWordTextComponent.color;
        colorTemp.a = 1f;
        Color startColor = completeWordTextComponent.color;

        panelCountSyllable.SetActive(true);

        while (times < fadeInTextDuration) {
            times += Time.deltaTime;
            float s = times / fadeInTextDuration;

            //int scory = (int)Mathf.Lerp (startPoints, scoreT, fadeInTextCurve.Evaluate (s));
            int syllablesInstanceCount = syllablesInstance.Count;
            for (int i = 0; i < syllablesInstanceCount; i++) {
                syllablesInstance[i].imageComp.color = Color.Lerp(Color.black, _color, fadeInTextCurve.Evaluate(s));
                syllablesInstance[i].textComp.color = Color.Lerp(Color.black, _color, fadeInTextCurve.Evaluate(s));
                blankSpaceInstance[i].thisImageComp.color = Color.Lerp(blankStartColor, Color.clear, fadeInTextCurve.Evaluate(s));
                blankSpaceInstance[i].traceImageComp.color = Color.Lerp(blankStartColor, Color.clear, fadeInTextCurve.Evaluate(s));
            }

            completeWordTextComponent.color = Color.Lerp(startColor, colorTemp, fadeInTextCurve.Evaluate(s));

            yield return new WaitForEndOfFrame();
        }

        panelCountSyllable.SetActive(true);


    }

    public void CleanWords() {
        int syllablesInstanceCount = syllablesInstance.Count;
        for (int i = 0; i < syllablesInstanceCount; i++) {
            Destroy(syllablesInstance[i].gameObject);
        }
        int blankSpaceInstanceCount = blankSpaceInstance.Count;
        for (int i = 0; i < blankSpaceInstanceCount; i++) {
            Destroy(blankSpaceInstance[i].gameObject);
        }

        syllablesInstance.Clear();
        blankSpaceInstance.Clear();
    }

    public void endDitatica() {
        StopAllCoroutines();
        //TODO delay aqui
        painelDesafio.scoreAmount = scoreController.amountValue.Value;
        painelDesafio.starAmount = starController.amountValue.Value;
        panelInteragindo.SetActive(true);
        //		panelInteragindo.GetComponent<Animator>().SetInteger("panelDesafioNumber",1);
    }

    public void ButtonIncrease() {
        _soundManager.startSoundFX(buttomConfirmClipAudio);
        confirmButtonRoutine2.interactable = true;
        if (scrollNumber >= 9) {
            scrollNumber = 0;
        } else {
            scrollNumber++;
        }

        scrollsnap.text = scrollNumber.ToString();
    }

    public void ButtonDecrease() {
        _soundManager.startSoundFX(buttomConfirmClipAudio);
        confirmButtonRoutine2.interactable = true;
        if (scrollNumber <= 0) {
            scrollNumber = 9;
        } else {
            scrollNumber--;
        }

        scrollsnap.text = scrollNumber.ToString();
    }

    public string ToTitleCase(string str) {
        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
    }

    public void DropCardSound() {
        _soundManager.startSoundFX(audios[2]);
    }

    public class BlueBoxPool : ObjectPool<SyllableHandler1_4B>
    {
        readonly SyllableHandler1_4B _prefab;
        readonly Transform _hierarchyParent;
        private readonly Manager1_4B _manager;

        public BlueBoxPool(SyllableHandler1_4B prefab, Transform hierarchyParent, Manager1_4B manager)
        {
            this._prefab = prefab;
            this._hierarchyParent = hierarchyParent;
            this._manager = manager;
        }

        protected override SyllableHandler1_4B CreateInstance()
        {
            var buttonInstance = UnityEngine.Object.Instantiate(_prefab, _hierarchyParent);
            buttonInstance.manager = this._manager;
            buttonInstance.ResetToDefault(_hierarchyParent);
            buttonInstance.thisCanvasGroup.alpha = 0f;
            buttonInstance.gameObject.SetActive(false);
            return buttonInstance;
        }

        protected override void OnBeforeReturn(SyllableHandler1_4B instance)
        {
            instance.gameObject.SetActive(false);
            instance.ResetToDefault(_hierarchyParent);
        }

        protected override void OnBeforeRent(SyllableHandler1_4B instance)
        {

            instance.ResetToDefault(_hierarchyParent);
            instance.gameObject.SetActive(true);
        }
    }

    public class BlankBoxPool : ObjectPool<BlankSpace1_4B>
    {
        readonly BlankSpace1_4B _prefab;
        readonly Transform _hierarchyParent;
        private readonly Manager1_4B _manager14B;

        public BlankBoxPool(BlankSpace1_4B prefab, Transform hierarchyParent, Manager1_4B manager)
        {
            this._prefab = prefab;
            this._hierarchyParent = hierarchyParent;
            this._manager14B = manager;
        }

        protected override BlankSpace1_4B CreateInstance()
        {
            var buttonInstance = UnityEngine.Object.Instantiate<BlankSpace1_4B>(_prefab, _hierarchyParent);
            buttonInstance.manager = this._manager14B;
            buttonInstance.ResetToDefault(_hierarchyParent);
            var color = Color.white;
            color.a = 0f;
            buttonInstance.thisImageComp.color = color;
            return buttonInstance;
        }

        protected override void OnBeforeReturn(BlankSpace1_4B instance)
        {
            instance.ResetToDefault(_hierarchyParent);
            instance.gameObject.SetActive(false);
        }

        protected override void OnBeforeRent(BlankSpace1_4B instance)
        {
            instance.gameObject.SetActive(true);
            instance.ResetToDefault(_hierarchyParent);
        }
    }

}


