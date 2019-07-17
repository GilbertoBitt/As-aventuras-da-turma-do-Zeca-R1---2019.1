using System;
using DG.Tweening;
using MEC;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using com.csutil;
using Controllers;
using MiniGames.Scripts;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using TMPro;
using TutorialSystem.Scripts;
using UniRx;
using UniRx.Toolkit;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

public class Manager1_4B : OverridableMonoBehaviour
{

    [BoxGroup("Tutorial Config")] public DialogComponent dialogComponent;
    [BoxGroup("Tutorial Config")]
    [TabGroup("1ª Ano")]
    public DialogInfo DialogInfoYear1;

    [BoxGroup("Tutorial Config")]
    [TabGroup("2ª Ano")]
    public DialogInfo DialogInfoYear2;

    [BoxGroup("Tutorial Config")]
    [TabGroup("3ª Ano")]
    public DialogInfo DialogInfoYear3;

    [TabGroup("1ª Ano")] public HabilidadeBNCCInfo Skill1Year;
    [TabGroup("2ª Ano")] public HabilidadeBNCCInfo Skill2Year;
    [TabGroup("3ª Ano")] public HabilidadeBNCCInfo Skill3Year;
    [FoldoutGroup("Geral")] public InfoSkillWindow infoSKill;

    public GridLayoutGroup gridLayoutGroup;
    public RectTransform gridLayoutGroupRectTransform;

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
    [FoldoutGroup("2ª ano")] public TextMeshProUGUI correctWordsTextComponent;
    [FoldoutGroup("2ª ano")] public IntReactiveProperty correctWordsValue;

    [FoldoutGroup("2ª ano")] public TextMeshProUGUI wrongWordsTextComponent;
    [FoldoutGroup("2ª ano")] public IntReactiveProperty wrongWordsValue;
    [FoldoutGroup("2ª ano")] public int amountPerCorrectWord;

    [FoldoutGroup("3ª ano")] public WordItem[] wordsList;
    [LabelText("Prefab Buttons")]
    [FoldoutGroup("3ª ano")] public ButtonTMPComponent buttonPrefab3;

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
    [FoldoutGroup("2ª ano")]
    public List<string> triedWordWrongOnes = new List<string>();

    public ReactiveProperty<bool> isNextRoutine;

    private bool _isNextRound => currentRound >= TotalRounds / 2;

    public void UpdatedGridLayout()
    {
        gridLayoutGroup.CalculateLayoutInputHorizontal();
        gridLayoutGroup.CalculateLayoutInputVertical();
        gridLayoutGroup.SetLayoutHorizontal();
        gridLayoutGroup.SetLayoutVertical();
    }

    public void InitGameConfig()
    {

        //StartConfiguration
        startGame = new ReactiveCommand();
        updatedGame = new ReactiveCommand();
        
        switch (anoLetivo.Value)
        {
            case AnoLetivoState.Ano1:
                showInformationButton.OnClickAsObservable().Subscribe(_ => infoSKill.ShowWindowInfo(Skill1Year));
                buttonPool = new ButtonTMPComponentPool(buttonPrefab, buttonParentTransform);
                #region SetupPool
                //Preload and config pool of boxes
                
                blueBoxInstancesOnScene = new List<SyllableHandler1_4B>();
                blankInstancesOnScene = new List<BlankSpace1_4B>();

                poolBlueBox = new BlueBoxPool(prefabBlueBoxInstance, blueBoxParent, this);

                poolBlankBox = new BlankBoxPool(prefabEmptyBoxInstance, emptyBoxParent, this);

                //boxes preload ended
                #endregion
                
                SuffleAndGenerateFirstRoutine();
                startGame.Subscribe(unit => { this.ExecuteDelayed(OnGameStart, 2f); });
                updatedGame.Subscribe(updated =>
                {
                    if (!isPlaying) return;
                    if (blankInstancesOnScene.All(x => x.hasDrop == true) && !hasWordComplete) {
                        hasWordComplete = true;
                        confirmButton.interactable = true;
                    } else if (blankInstancesOnScene.Any(x => x.hasDrop != true) && hasWordComplete) {
                        hasWordComplete = false;
                        confirmButton.interactable = false;
                    }
                });

                this.UpdateAsObservable().Subscribe(unit => { updatedGame?.Execute(); });
                break;
            case AnoLetivoState.Ano2:
                showInformationButton.OnClickAsObservable().Subscribe(_ => infoSKill.ShowWindowInfo(Skill2Year));
                buttonPool = new ButtonTMPComponentPool(buttonPrefab, buttonParentTransform);
                blueBoxInstancesOnScene = new List<SyllableHandler1_4B>();
                blankInstancesOnScene = new List<BlankSpace1_4B>();

                #region SetupPool
                //Preload and config pool of boxes

                poolBlueBox = new BlueBoxPool(prefabBlueBoxInstance, blueBoxParent, this);

                poolBlankBox = new BlankBoxPool(prefabEmptyBoxInstance, emptyBoxParent, this);

                //boxes preload ended
                #endregion

                randomWordArray.Shuffle();
                confirmButton.onClick.AsObservable().Subscribe(_ => CorrectCurrentWord());
                startGame.Subscribe(unit => { this.ExecuteDelayed(StartLoopSecondYear, 2f); });

                dragDropPanelContent.SetActive(true);

                correctWordsValue.Subscribe(_ => UpdateTextRight());
                wrongWordsValue.Subscribe(_ => UpdateTextWrong());

                break;
            case AnoLetivoState.Ano3:
                showInformationButton.OnClickAsObservable().Subscribe(_ => infoSKill.ShowWindowInfo(Skill3Year));
                buttonPool = new ButtonTMPComponentPool(buttonPrefab3, buttonParentTransform);
                blueBoxInstancesOnScene = new List<SyllableHandler1_4B>();
                blankInstancesOnScene = new List<BlankSpace1_4B>();

                #region SetupPool
                //Preload and config pool of boxes

                poolBlueBox = new BlueBoxPool(prefabBlueBoxInstance, blueBoxParent, this);

                poolBlankBox = new BlankBoxPool(prefabEmptyBoxInstance, emptyBoxParent, this);

                //boxes preload ended
                #endregion

                wordsList.Shuffle();
                confirmButton.onClick.AsObservable().Subscribe(_ => CorrectionOfSyllable());
                startGame.Subscribe(unit => { this.ExecuteDelayed(ThirdYearLoop, 2f); });
                dragDropPanelContent.SetActive(true);
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
        Debug.Log("Game inicialization Complete");

    }



    public void ThirdYearLoop()
    {
        if (currentGameRound > maxRound)
        {
            endDitatica();
        }
        else
        {

            blueBoxInstancesOnScene.ForEach(box =>
            {
                poolBlueBox.Return(box);
            });
            blueBoxInstancesOnScene.Clear();

            blankInstancesOnScene.ForEach(box =>
            {
                poolBlankBox.Return(box);
            });
            blankInstancesOnScene.Clear();

            currentWord = wordsList[currentRound];

            var syllables = currentWord.syllables;

            syllables.ForEach((item, itemIndex) =>
            {
                var blueBoxInstance = poolBlueBox.Rent();
                blueBoxInstance.UpdateTextContent(item.ToUpper());
                blueBoxInstance.transform.SetSiblingIndex(itemIndex);
                blueBoxInstance.DoFade(1f);
                blueBoxInstancesOnScene.Insert(itemIndex, blueBoxInstance);
            });

            var emptySpace = poolBlankBox.Rent();
            emptySpace.DoFade(1f);
            blankInstancesOnScene.Add(emptySpace);

            this.UpdateAsObservable().Where(unit =>blankInstancesOnScene.TrueForAll(box => box.hasDrop && box.thisSyllable != null))
                .Subscribe(unit => { confirmButton.interactable = true; });

            this.UpdateAsObservable().Where(unit => blankInstancesOnScene.Any(box => !box.hasDrop || box.thisSyllable == null))
                .Subscribe(unit => { confirmButton.interactable = false; });

        }
    }

    private void StartLoopSecondYear()
    {

        if (currentGameRound > maxRound)
        {
            highlight.startCerto("Parabéns! Você acertou todas as palavras!");
            endDitatica();
        }
        else
        {
            triedWordWrongOnes.Clear();
            wordsToFind.Clear();

            correctWordsValue.Value = 0;
            wrongWordsValue.Value = 0;

            if (wordsToFind == null)
            {
                wordsToFind = new ReactiveDictionary<string, bool>();
            }
            wordsToFind.Clear();
            //Return boxes used to pool.
            blueBoxInstancesOnScene.ForEach(box =>
            {
                poolBlueBox.Return(box);
            });
            blueBoxInstancesOnScene.Clear();

            blankInstancesOnScene.ForEach(box =>
            {
                poolBlankBox.Return(box);
            });
            blankInstancesOnScene.Clear();

            if (currentGameRound <= maxRound)
            {
                currentWord = randomWordArray[currentGameRound];
            }

            foreach (var word in currentWord.alternativeWordsContent.alternativeWords)
            {
                wordsToFind.Add(word.ToLower(), false);
            }

            var alternativeWords = currentWord.alternativeWordsContent.alternativeWords.Length;

            //instance blue box for syllable

            var shuffleSyllables = currentWord.syllables;

            shuffleSyllables.ForEach((item, itemIndex) =>
            {
                var blueBoxInstance = poolBlueBox.Rent();
                blueBoxInstance.UpdateTextContent(item.ToUpper());
                blueBoxInstance.transform.SetSiblingIndex(itemIndex);
                blueBoxInstance.DoFade(1f);
                blueBoxInstancesOnScene.Insert(itemIndex, blueBoxInstance);

                if (itemIndex == 0) return;
                var emptySpace = poolBlankBox.Rent();
                emptySpace.transform.SetSiblingIndex(itemIndex-1);
                emptySpace.DoFade(1f);
                blankInstancesOnScene.Insert(itemIndex-1, emptySpace);
            });

            //Set configs for onClickConfirmButton

            this.UpdateAsObservable()
                .Where(unit => isPlaying && blankInstancesOnScene.TrueForAll(box => box.hasDrop && box.thisSyllable != null))
                .Subscribe(unit => { confirmButton.interactable = true; });

            this.UpdateAsObservable().Where(unit =>
                    isPlaying && !blankInstancesOnScene.TrueForAll(box => box.hasDrop && box.thisSyllable != null))
                .Subscribe(unit => { confirmButton.interactable = false; });
        }
    }

    public void UpdateTextRight()
    {
        var sb = new StringBuilder();
        sb
            .Append("Acertos (<color=#008000ff>")
            .Append(correctWordsValue.Value)
            .Append("</color>) | ");

        wordsToFind.ForEach((pair, i) =>
        {
            if (!pair.Value) return;
            sb.Append("<color=#008000ff>");
            sb.Append(pair.Key);
            sb.Append("</color>");
            sb.Append(i == wordsToFind.Count - 1 ? "." : ", ");


        });

        correctWordsTextComponent.SetText(sb);

    }

    public void UpdateTextWrong()
    {
        var sb = new StringBuilder();
        sb
            .Append("Erros (<color=#ff0000ff>")
            .Append(wrongWordsValue.Value)
            .Append("</color>) | ");

        triedWordWrongOnes.ForEach((s, i) =>
        {

            sb.Append(" <color=#ff0000ff>");
            sb.Append(s);
            sb.Append("</color>");
            sb.Append(i == triedWordWrongOnes.Count - 1 ? "." : ", ");
        });

        wrongWordsTextComponent.SetText(sb);

    }

    private void CorrectionOfSyllable()
    {
        var syllable = blankInstancesOnScene[0].thisSyllable.textComp.text.ToLower();

        var reversed = currentWord.syllables.Reverse().ToList();

        var correctSyllable = reversed[(int) currentWord.tonicSyllables.classification].ToLower();

        if (string.Equals(correctSyllable, syllable, StringComparison.InvariantCultureIgnoreCase))
        {
            Debug.Log($"Current Word [{currentWord.word.ToLower()}] | Tonic Syllable [{correctSyllable}] | Selected By Player [{syllable}] | Word Classeification [{currentWord.tonicSyllables.classification}]");

            scoreController.amountValue.Value += amountPerCorrectWord;

            _soundManager.startSoundFX(audios[0]);
            highlight.startCerto("Silába tônica correta!");
            currentGameState.Value = GameState.SecondState;
        }
        else
        {
            _soundManager.startSoundFX(audios[1]);
            highlight.startErrado("Você errou... que pena!");
            currentRound++;
            currentGameState.SetValueAndForceNotify(GameState.MainState);
            startGame.Execute();
        }

    }

    private void CorrectCurrentWord()
    {
        //correction method to completed word

        //Get value guessed
        var playerGuessWord = blankInstancesOnScene.Where(item => item.thisSyllable != null).Aggregate(string.Empty, (current, item) => current + item.thisSyllable.textComp.text).ToLower();

        //confirm if word has or not this alternative word.
        var hasWord = wordsToFind.Keys.FirstOrDefault(s => s == playerGuessWord);

        if (!string.IsNullOrEmpty(hasWord) && !wordsToFind[hasWord])
        {
            wordsToFind[hasWord] = true;
            scoreController.amountValue.Value += amountPerCorrectWord;
            correctWordsValue.Value++;
            _soundManager.startSoundFX(audios[0]);

            if (wordsToFind.Values.All(x => x) && currentGameRound <= maxRound)
            {
                //Mostrar tela de parabéns com quantidade de palavras acertadas e erradas.
                highlight.startCerto("Parabéns! Você acertou todas as palavras");
                currentGameRound++;
                startGame.Execute();
            }
            else
            {
                highlight.startCerto("Palavra correta!");
                //Efeito de acerto e texto
                ReturnToInitConfiguration();
            }

            Debug.Log($"Palavra encontrada [{hasWord}]");

        }
        else
        {
            if (!string.IsNullOrEmpty(hasWord) && wordsToFind[hasWord])
            {
                wrongWordsValue.Value++;
                _soundManager.startSoundFX(audios[1]);
                highlight.startErrado("Palavra já encontrada");
                Debug.Log($"Palavra já encontrada [{hasWord}]");
            }
            else
            {
                if (!triedWordWrongOnes.Contains(playerGuessWord))
                {
                    triedWordWrongOnes.Add(playerGuessWord);
                }
                wrongWordsValue.Value++;
                _soundManager.startSoundFX(audios[1]);
                highlight.startErrado("Palavra Errada!");
                Debug.Log($"Palavra invalida [{playerGuessWord}]");
            }
            ReturnToInitConfiguration();
        }

    }

    private void ReturnToInitConfiguration(string[] strings = null)
    {
        blueBoxInstancesOnScene.ForEach(box =>
        {
            poolBlueBox.Return(box);
        });
        blueBoxInstancesOnScene.Clear();

        blankInstancesOnScene.ForEach(box =>
        {
            poolBlankBox.Return(box);
        });
        blankInstancesOnScene.Clear();

        var shuffleSyllables = strings ?? currentWord.syllables;

        shuffleSyllables.ForEach((item, itemIndex) =>
        {
            var blueBoxInstance = poolBlueBox.Rent();
            blueBoxInstance.UpdateTextContent(item.ToUpper());
            blueBoxInstance.transform.SetSiblingIndex(itemIndex);
            blueBoxInstance.DoFade(1f);
            blueBoxInstancesOnScene.Insert(itemIndex, blueBoxInstance);

            if (itemIndex == 0) return;

            if (itemIndex == 0) return;
            var emptySpace = poolBlankBox.Rent();
            emptySpace.transform.SetSiblingIndex(itemIndex-1);
            emptySpace.DoFade(1f);
            blankInstancesOnScene.Insert(itemIndex-1, emptySpace);
        });

    }

    private void OnGameStart()
    {
        switch (anoLetivo.Value)
        {
            case AnoLetivoState.Ano1:
            {
                dragDropPanelContent.SetActive(true);
                isPlaying = false;
                //Reconfigurar Pool.
                int temp = blueBoxInstances.Length;

                if (blueBoxInstancesOnScene.Count >= 1)
                {
                    blueBoxInstancesOnScene.ForEach(box =>
                    {
                        poolBlueBox.Return(box);
                    }); 
                    blueBoxInstancesOnScene.Clear();
                }

                if (blankInstancesOnScene.Count >= 1)
                {
                    blankInstancesOnScene.ForEach(box =>
                    {
                        poolBlankBox.Return(box);
                    });
                    blankInstancesOnScene.Clear();
                }

                blueBoxInstances = new SyllableHandler1_4B[0];
                emptyBoxInstances = new BlankSpace1_4B[0];

                if (currentRound > TotalRounds)
                {
                    endDitatica();
                }
                else
                {
                    switch (currentGameState.Value)
                    {
                        case GameState.MainState:
                            currentWord = routineOneList[currentRound];
                            //Iniciar Rotina de Letras.


                            //Configurando Didatica com a palavra escolhida.
                            blueBoxInstances = new SyllableHandler1_4B[currentWord.CountLetters];
                            emptyBoxInstances = new BlankSpace1_4B[currentWord.CountLetters];
                            currentWord.letters.Shuffle();

                            currentWord.letters.ForEach((itemString, indexItem) =>
                            {
                                var blueBoxInstance = poolBlueBox.Rent();
                                blueBoxInstance.UpdateTextContent(itemString.ToUpper());
                                blueBoxInstance.DoFade(1f);
                                blueBoxInstance.transform.SetSiblingIndex(indexItem);
                                blueBoxInstancesOnScene.Insert(indexItem, blueBoxInstance);

                                var emptySpace = poolBlankBox.Rent();
                                emptySpace.DoFade(1f);
                                emptySpace.transform.SetSiblingIndex(indexItem);
                                blankInstancesOnScene.Insert(indexItem, emptySpace);
                            });

                            itemIconImage.sprite = currentWord.itemSprite;
                            itemIconImage.DOFade(1f, 0.5f);
                            isPlaying = true;
                            confirmButton.interactable = false;
                            break;
                        case GameState.MainStateAlternate:
                            hasWordComplete = false;
                            Sequence TextFadeChange = DOTween.Sequence();

                            currentWord = routineTwoList[currentRound];
                            blueBoxInstances = new SyllableHandler1_4B[currentWord.CountSyllables];
                            emptyBoxInstances = new BlankSpace1_4B[currentWord.CountSyllables];
                            currentWord.syllables.Shuffle();

                            currentWord.syllables.ForEach((itemString, indexItem) =>
                            {
                                var blueBoxInstance = poolBlueBox.Rent();
                                blueBoxInstance.UpdateTextContent(itemString.ToUpper());
                                blueBoxInstance.DoFade(1f);
                                blueBoxInstance.transform.SetSiblingIndex(indexItem);
                                blueBoxInstancesOnScene.Insert(indexItem, blueBoxInstance);

                                var emptySpace = poolBlankBox.Rent();
                                emptySpace.DoFade(1f);
                                emptySpace.transform.SetSiblingIndex(indexItem);
                                blankInstancesOnScene.Insert(indexItem, emptySpace);
                            });


//                            for (int i = 0; i < currentWord.CountSyllables; i++) {
//                                var blueBoxInstance = poolBlueBox.Rent();
//                                blueBoxInstance.UpdateTextContent(currentWord.syllables[i].ToUpper());
//                                blueBoxInstance.DoFade(1f);
//                                blueBoxInstancesOnScene.Add(blueBoxInstance);
//
//                                var emptySpace = poolBlankBox.Rent();
//                                emptySpace.DoFade(1f);
//                                blankInstancesOnScene.Add(emptySpace);
//                            }

                            itemIconImage.sprite = currentWord.itemSprite;
                            itemIconImage.DOFade(1f, 0.5f);
                            isPlaying = true;
                            confirmButton.interactable = false;
                            break;
                        case GameState.SecondState:
                            break;
                        case GameState.SecondStateAlternate:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
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
                        CorrectionAmountSyllables();
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
                        UpdateEnunciado("Quantas letras a palavra abaixo tem?");
                        for (int i = 0; i <= 10; i++)
                        {
                            var instance = buttonPool.Rent();
                            instance.textComponent.text = i.ToString();
                            var indexValue = i;
                            instance.buttonComponent.OnPointerClickAsObservable().Subscribe(data =>
                            {
                                scrollNumber = indexValue;
                                CorrectionAmountLetters();
                            });
                            buttonInstances.Add(instance);
                        }
                        break;
                    case AnoLetivoState.Ano2:
                        break;
                    case AnoLetivoState.Ano3:
                        confirmButton.gameObject.SetActive(false);
                        blueBoxInstancesOnScene.ForEach(box =>
                        {
                            poolBlueBox.Return(box);
                        });
                        blueBoxInstancesOnScene.Clear();

                        blankInstancesOnScene.ForEach(box =>
                        {
                            poolBlankBox.Return(box);
                        });
                        blankInstancesOnScene.Clear();

                        string GetTextFromTonicSyllable(int indexEnum)
                        {
                            var classification = (WordItem.TonicSyllablesClassification) indexEnum;
                            switch (classification)
                            {
                                case WordItem.TonicSyllablesClassification.Oxitona:
                                    return "Oxítona";
                                case WordItem.TonicSyllablesClassification.Paroxitona:
                                    return "Paroxítona";
                                case WordItem.TonicSyllablesClassification.Proparoxitona:
                                    return "Proparoxítona";
                                default:
                                    throw new ArgumentOutOfRangeException(nameof(classification), classification, null);
                            }
                        }

                        UpdateEnunciado("Classifique a palavra abaixo.");
                        for (int i = 0; i < 3; i++)
                        {
                            var instance = buttonPool.Rent();
                            instance.textComponent.text =
                                GetTextFromTonicSyllable(i);
                            var indexValue = i;
                            instance.buttonComponent.OnPointerClickAsObservable().Subscribe(data =>
                                {
                                    scrollNumber = indexValue;
                                    CorrectTonicClassification(indexValue);
                                });
                            buttonInstances.Add(instance);
                        }
                        break;
                    default:
                        Debug.Break();
                        throw new ArgumentOutOfRangeException();
                }
    }

    private void CorrectTonicClassification(int indexValue)
    {
        var correctIndex = (int) currentWord.tonicSyllables.classification;

        if (correctIndex == indexValue)
        {
            scoreController.amountValue.Value += amountPerCorrectWord;

            _soundManager.startSoundFX(audios[0]);
            highlight.startCerto("Silába tônica correta!");
        }
        else
        {
            _soundManager.startSoundFX(audios[1]);
            highlight.startErrado("Você errou... que pena!");
        }

        currentRound++;
        currentGameState.SetValueAndForceNotify(GameState.MainState);
        startGame.Execute();

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
                confirmButton.gameObject.SetActive(true);
                UpdateEnunciado("Qual a sílaba tônica?");
                buttonInstances.ForEach(component =>
                {
                    buttonPool.Return(component);
                });
                disposables.Dispose();
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



    public void InitGame()
    {
        anoLetivo.Value = (AnoLetivoState) GameConfig.Instance.GetAnoLetivo();
        this.enabled = true;
//        dialogComponent = FindObjectOfType((typeof(DialogComponent))) as DialogComponent;

        if (dialogComponent == null) return;
        dialogComponent.transform.parent.gameObject.SetActive(true);
        dialogComponent.endTutorial = () =>
        {
            Debug.Log("Call Twice!");
            activateThisPanel.SetActive(true);
            nexPanelGroupComp.blocksRaycasts = false;
            nexPanelGroupComp.interactable = false;
            nexPanelGroupComp.alpha = 0f;
            scoreController.amountValue.Value = previousManager.scoreAmount;
            starController.amountValue.Value = previousManager.starAmount;
            InitGameConfig();
        };

        switch (anoLetivo.Value)
        {
            case AnoLetivoState.Ano1:
                dialogComponent.StartDialogSystem(DialogInfoYear1);
                break;
            case AnoLetivoState.Ano2:
                dialogComponent.StartDialogSystem(DialogInfoYear2);
                break;
            case AnoLetivoState.Ano3:
                dialogComponent.StartDialogSystem(DialogInfoYear3);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        activateThisPanel.SetActive(true);
        oldPanelDisable.SetActive(false);
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

   private void WordSillablesCorrection()
    {
        //correção de palavras.
        string fullPlayerWordGuess = blankInstancesOnScene.Where(item => item.thisSyllable != null).Aggregate(string.Empty, (current, item) => current + item.thisSyllable.textComp.text);

        fullPlayerWordGuess = ToTitleCase(fullPlayerWordGuess);
        currentWord.word = ToTitleCase(currentWord.word);

        Debug.Log("Respostas do Jogador: [" + fullPlayerWordGuess + "] \nResposta correta: [" + currentWord.word + "]");

        if (string.Equals(fullPlayerWordGuess, currentWord.word, StringComparison.CurrentCultureIgnoreCase))
        {
            //Acertou a palavra.
            log.SaveEstatistica(9, 1, true);
            _soundManager.startSoundFX(audios[0]);
            highlight.startCerto("Parabéns! Você acertou!");
            scoreController.amountValue.Value += amountScorePerCorrectSyllabes;
            //Iniciar Contagem de Letras.
            currentGameState.Value = GameState.SecondStateAlternate;
            StartRoutineTwo();
        }
        else
        {
            //Errou a palavra.
            log.SaveEstatistica(9, 1, false);
            _soundManager.startSoundFX(audios[1]);
            
            highlight.startErrado("Você errou! O correto era: " + ToTitleCase(currentWord.word) + string.Empty);
            currentGameState.Value = GameState.MainStateAlternate;
            //Iniciar novo Round.
            startGame.Execute();
        }
    }

    private void WordDraggedCorretion()
    {
        string fullPlayerWordGuess = blankInstancesOnScene.Where(item => item.thisSyllable != null).Aggregate(string.Empty, (current, item) => current + item.thisSyllable.textComp.text);

        Debug.Log("Respostas do Jogador: [" + fullPlayerWordGuess.ToLower() + "] \nResposta correta: [" + currentWord.word.ToLower() + "]");

        if (string.Equals(fullPlayerWordGuess, currentWord.word, StringComparison.CurrentCultureIgnoreCase))
        {
            //Acertou a palavra.
            log.SaveEstatistica(9, 1, true);
            _soundManager.startSoundFX(audios[0]);
            highlight.startCerto("Parabéns! Você acertou!");
//                textScoreComp.DOTextInt(scoreAmount, (scoreAmount + amountScorePerCorrectSyllabes), 0.5f);
            scoreController.amountValue.Value += amountScorePerCorrectSyllabes;
            //Iniciar Contagem de Letras.
            currentGameState.Value = _isNextRound ? GameState.MainStateAlternate : GameState.SecondState;
            StartRoutineTwo();
        }
        else
        {
            //Errou a palavra.
            log.SaveEstatistica(9, 1, false);
            _soundManager.startSoundFX(audios[1]);
            highlight.startErrado("Você errou! O correto era: " + ToTitleCase(currentWord.word) + string.Empty);
            currentRound++;
            currentGameState.Value = _isNextRound ? GameState.MainStateAlternate : GameState.MainState;
            startGame.Execute();
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
            currentGameState.Value = GameState.MainStateAlternate;
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
            currentGameState.Value = _isNextRound ? GameState.MainStateAlternate : GameState.MainState;
            EndRoutineTwo();
        }
        else
        {
            //errou o numero de letras.
            _soundManager.startSoundFX(audios[1]);
            log.SaveEstatistica(10, 1, true);
            highlight.startErrado("Você errou o certo era: " + currentWord.CountLetters);
            currentGameState.Value = _isNextRound ? GameState.MainStateAlternate : GameState.MainState;
            EndRoutineTwo();
        }
    }

    public void EndRoutineTwo() {
        //completeWordTextComp.text = currentWord.completeWord.ToUpper();        
//        confirmButton.interactable = true;
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
            blueBoxInstancesOnScene[i].DoFade(0f);
            blankInstancesOnScene[i].DoFade(0f);
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
            buttonInstance.ResetToDefault();
            buttonInstance.thisCanvasGroup.alpha = 0f;
            buttonInstance.gameObject.SetActive(false);
            return buttonInstance;
        }

        protected override void OnBeforeReturn(SyllableHandler1_4B instance)
        {
            instance.gameObject.SetActive(false);
            instance.ResetToDefault();
        }

        protected override void OnBeforeRent(SyllableHandler1_4B instance)
        {

            instance.ResetToDefault();
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
            buttonInstance.ResetToDefault();
            var color = Color.white;
            color.a = 0f;
            buttonInstance.thisImageComp.color = color;
            return buttonInstance;
        }

        protected override void OnBeforeReturn(BlankSpace1_4B instance)
        {
            instance.ResetToDefault();
            instance.gameObject.SetActive(false);
        }

        protected override void OnBeforeRent(BlankSpace1_4B instance)
        {
            instance.gameObject.SetActive(true);
            instance.ResetToDefault();
        }
    }

}


