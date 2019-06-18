﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using MEC;
using MiniGames.Rainbow.Scripts;
using MiniGames.Rainbow.Scripts._1ª_Ano;
using Sirenix.OdinInspector;
using TMPro;
using UniRx;
using Unity.Mathematics;
using Random = UnityEngine.Random;

public class managerAddRainbow : MonoBehaviour
{

	[FoldoutGroup("Geral")] public int anoLetivo;
	[FoldoutGroup("Geral")] public TextMeshProUGUI enumciadoTextComponent;
	[FoldoutGroup("1ª Ano")] public GameObject panelAnoLetivoOne;
	[FoldoutGroup("1ª Ano")] public GroupLayoutContentManagerComponent[] groupContent;
	[FoldoutGroup("1ª Ano")] public RainbowItem[] rainbowItems;
	[FoldoutGroup("1ª Ano")] public RainbowItem[] rainbowItemsSelected;
	[FoldoutGroup("1ª Ano")] public EquationContent[] equationContents;
	[Sirenix.OdinInspector.MinMaxSlider(2, 20)]
	[FoldoutGroup("1ª Ano")] public Vector2Int minMaxRandomValues;
	[FoldoutGroup("1ª Ano")] public TextMeshProUGUI firstValueTextComponent;
	[FoldoutGroup("1ª Ano")] public GroupLayoutContentManagerComponent firstValueGroupContentComponent;
	[FoldoutGroup("1ª Ano")] public TextMeshProUGUI secondValueTextComponent;
	[FoldoutGroup("1ª Ano")] public GroupLayoutContentManagerComponent secondValueGroupContentComponent;
	[FoldoutGroup("1ª Ano")] public TextMeshProUGUI equationTypeValueTextComponent;
	[FoldoutGroup("1ª Ano")] public TextMeshProUGUI equationTypeValueTextComponentSecond;
	[FoldoutGroup("1ª Ano")] public GroupLayoutContentManagerComponent resultValueGroupContentComponent;
	[FoldoutGroup("1ª Ano")] public TextMeshProUGUI resultValueTextComponent;
	[FoldoutGroup("1ª Ano")] public Button[] alternativeButtons;
	[FoldoutGroup("3ª Ano")] public MultiplyInAddition multiplyInAdditionComponent;
	[FoldoutGroup("3ª Ano")] public TextMeshProUGUI resultOfAdditionTextComponent;
	[FoldoutGroup("3ª Ano")] public TextMeshProUGUI resultOfMultiplyTextComponent;
	[FoldoutGroup("3ª Ano")] public TextMeshProUGUI multiplyEquationText;
	[FoldoutGroup("3ª Ano")] public GameObject panelThirdAnoLetivo;


	#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
	[SeparatorAttribute("Configurações")]
	#endif
	public int timesToAsk = 4;
	public int timesAsk = 0;
	public int scoreAmount = 0;
	public int starAmount;
	public int scorePerRight = 30;
	public int rightResult;
	#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
	[SeparatorAttribute("Adição e Subtração Painel")]
	#endif
	public GameObject mainPanel;
	public GameObject oldPanel;
	public RainbowController rainbowController;
	#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
	//[SeparatorAttribute("Painéis Laterais ")]
	#endif
	//public Text[] sidePanelTextsName = new Text[3];
	//public Text[] sidePanelTextsCount = new Text[3];
	#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
	[SeparatorAttribute("Botões de Opções!")]
	#endif
	public int[] alternatives = new int[4];
	public int alternativeSelected = -1;
	public TextMeshProUGUI[] alternativeTexts = new TextMeshProUGUI[4];
	public Button confirmButton;
	public Button[] buttonsAlternative;
	#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
	[SeparatorAttribute("Textos do Painel Principal")]
	#endif
	public Text firstNumberText;
	public Text secondNumberText;
	public Text sinalOfOperationText;
	public Text resultOfOperation;
	//public Image[] imagePanels;
	public Color selectionColor;
	public Color defaultColor;
	public ItemIndexed[] itensToCount;
	#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
	[SeparatorAttribute("Tela de Parabéns")]
	#endif
	public Animator congratsAnimator;
	public GameObject congratsPanel;
	public Text congratsText;
	private int acertoErrohash;
	#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
	[SeparatorAttribute("Desafio")]
	#endif
	public Animator chalengePanelAnimator;
	public GameObject chalengePanel;
	public PanelDesafioControl challengeController;
	public int panelDesafioNumberHash;
	#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
	[SeparatorAttribute("Painel Tutorial")]
	#endif
	public GameObject painelTutorial;
	public GameObject pointT;
	public int scoreDidatica = 0;
	public float timePedagogica = 0f;
	public bool timerStart = false;
    public Image characterImage;
    public Sprite[] characterImages;
	#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
	[SeparatorAttribute("Configuração Didatica")]
	#endif
	public int idHabilidade;
	public int idDificuldade;
	public LogSystem log;
	public Text facaConta;
	public string[] facaContaT;
	public Image[] buttons2;
	public Sprite[] buttonsSp;
    tutorPanelArcoirirs tutorPanelArcoirirs2;
    int contNunbAudioPos;
    int contNunbAudioNeg;
    public GameObject pauseBt;
    public SoundManager soundManager;
    public AudioClip acertoAudioClip;
    public AudioClip erroAudioClip;


    // Use this for initialization
    public void StartGame () {
		Timing.RunCoroutine(StartGameLate(), Segment.SlowUpdate);

	}

	IEnumerator<float> StartGameLate(){
        yield return Timing.WaitForSeconds(0.6f);
        pauseBt.SetActive(false);
        // Debug.Log("Soma e Subtração ativada! açlskdçlaksdçalksdçls");
        tutorPanelArcoirirs2 = painelTutorial.GetComponent<tutorPanelArcoirirs>();
        int person = PlayerPrefs.GetInt("characterSelected", 0);
        characterImage.sprite = characterImages[person];
        painelTutorial.SetActive(false);
		acertoErrohash = Animator.StringToHash("acertoErro");
		panelDesafioNumberHash = Animator.StringToHash("panelDesafioNumber");
		chalengePanel.SetActive(false);
		mainPanel.SetActive(true);
		oldPanel.SetActive(false);
		itensToCount = rainbowController.ReturnTopThree();
		scoreAmount = rainbowController.amountPoints;
		starAmount = rainbowController.amountStars;
		//rainbowController.enabled = false;
		scoreDidatica = rainbowController.scorePegagogica;
		timePedagogica = rainbowController.timerDidatica;
		timerStart = rainbowController.isTimerLogDidaticaStart;


		//Randomizar seleção.
		switch (anoLetivo)
		{
			case 1:
				rainbowItems.Shuffle();
				rainbowItemsSelected = rainbowItems.Take(4).ToArray();
				equationContents = new EquationContent[4];
				for (int i = 0; i < 4; i++)
				{
					equationContents[i] = new EquationContent();
					switch (i)
					{
						case 0:
							equationContents[i].groupType = GroupType.Juntar;
							equationContents[i].equationType = EquationType.Addition;
							break;
						case 1:
							equationContents[i].groupType = GroupType.Acrescentar;
							equationContents[i].equationType = EquationType.Addition;
							break;
						case 2:
							equationContents[i].groupType = GroupType.Retirar;
							equationContents[i].equationType = EquationType.Subtraction;
							break;
						default:
							equationContents[i].groupType = GroupType.Separar;
							equationContents[i].equationType = EquationType.Subtraction;
							break;
					}

					do
					{
						equationContents[i].firstValue = Random.Range(minMaxRandomValues.x, minMaxRandomValues.y);
                        equationContents[i].secondValue = Random.Range(minMaxRandomValues.x, minMaxRandomValues.y);
					} while (equationContents[i].ResultValue() > 20 || equationContents[i].ResultValue() < 0);
					equationContents[i].rainbowItem = rainbowItemsSelected[i];
					panelAnoLetivoOne.SetActive(true);
				}
				break;
			case 2:
				rainbowItems.Shuffle();
				rainbowItemsSelected = rainbowItems.Take(4).ToArray();
				equationContents = new EquationContent[4];
				for (int i = 0; i < 4; i++)
				{
					equationContents[i] = new EquationContent();
					switch (i)
					{
						case 0:
							equationContents[i].groupType = GroupType.Juntar;
							equationContents[i].equationType = EquationType.Addition;
							do
							{
								equationContents[i].firstValue = Random.Range(minMaxRandomValues.x, minMaxRandomValues.y);
								equationContents[i].secondValue = Random.Range(minMaxRandomValues.x, minMaxRandomValues.y);
							} while (equationContents[i].ResultValue() > 20
							         || equationContents[i].ResultValue() < 0 ||
							         equationContents[i].firstValue > 10);
							break;
						case 1:
							equationContents[i].groupType = GroupType.Acrescentar;
							equationContents[i].equationType = EquationType.Addition;
							do
							{
								equationContents[i].firstValue = Random.Range(minMaxRandomValues.x, minMaxRandomValues.y);
								equationContents[i].secondValue = Random.Range(minMaxRandomValues.x, minMaxRandomValues.y);
							} while (equationContents[i].ResultValue() > 20
							         || equationContents[i].ResultValue() < 0 ||
							         equationContents[i].firstValue < 10);
							break;
						case 2:
							equationContents[i].groupType = GroupType.Retirar;
							equationContents[i].equationType = EquationType.Subtraction;
							do
							{
								equationContents[i].firstValue = Random.Range(minMaxRandomValues.x, minMaxRandomValues.y);
								equationContents[i].secondValue = Random.Range(minMaxRandomValues.x, minMaxRandomValues.y);
							} while (equationContents[i].ResultValue() > 20
							         || equationContents[i].ResultValue() < 0 ||
							         equationContents[i].firstValue > 10);
							break;
						default:
							equationContents[i].groupType = GroupType.Separar;
							equationContents[i].equationType = EquationType.Subtraction;
							do
							{
								equationContents[i].firstValue = Random.Range(minMaxRandomValues.x, minMaxRandomValues.y);
								equationContents[i].secondValue = Random.Range(minMaxRandomValues.x, minMaxRandomValues.y);
							} while (equationContents[i].ResultValue() > 20
							         || equationContents[i].ResultValue() < 0 ||
							         equationContents[i].firstValue < 10);
							break;
					}


					equationContents[i].rainbowItem = rainbowItemsSelected[i];
					panelAnoLetivoOne.SetActive(true);
				}
				break;
			case 3:
				//TODO multiplicação por 2,3,4,5 e 10.
				rainbowItems.Shuffle();
				rainbowItemsSelected = rainbowItems.Take(4).ToArray();
				equationContents = new EquationContent[4];
				for (var index = 0; index < equationContents.Length; index++)
				{
					equationContents[index] = new EquationContent();
					var equationContent = equationContents[index];
					equationContent.equationType = EquationType.Multiplication;
					do
					{
						equationContent.firstValue = new List<int>() {2, 3, 4, 5, 10}.GetRandomValue();
					} while (equationContents.Any(x => x.firstValue == equationContent.firstValue));

					equationContent.secondValue = Random.Range(1, 11);
				}

				panelThirdAnoLetivo.SetActive(true);
				break;
		}

        NextQuestion();
        Timing.RunCoroutine(timingTODisable());
        confirmButton.interactable = false;
        yield return Timing.WaitForSeconds(2f);
        congratsPanel.SetActive(true);
        congratsAnimator.enabled = true;
    }

    IEnumerator<float> timingTODisable() {
        yield return Timing.WaitForSeconds(5f);
        
        rainbowController.enabled = false;
    }

	public void buttonAnswer01(){
		alternativeSelected = alternatives[0];
		buttons2[0].sprite = buttonsSp[1];
		buttons2[1].sprite = buttonsSp[0];
		buttons2[2].sprite = buttonsSp[0];
		buttons2[3].sprite = buttonsSp[0];
		resultValueTextComponent.text = alternativeSelected.ToString();
        confirmButton.interactable = true;

    }

	public void buttonAnswer02(){
		alternativeSelected = alternatives[1];
		buttons2[0].sprite = buttonsSp[0];
		buttons2[1].sprite = buttonsSp[1];
		buttons2[2].sprite = buttonsSp[0];
		buttons2[3].sprite = buttonsSp[0];
		resultValueTextComponent.text = alternativeSelected.ToString();
        confirmButton.interactable = true;
    }

	public void buttonAnswer03(){
		alternativeSelected = alternatives[2];
		buttons2[0].sprite = buttonsSp[0];
		buttons2[1].sprite = buttonsSp[0];
		buttons2[2].sprite = buttonsSp[1];
		buttons2[3].sprite = buttonsSp[0];
		resultValueTextComponent.text = alternativeSelected.ToString();
        confirmButton.interactable = true;
    }

	public void buttonAnswer04(){
		alternativeSelected = alternatives[3];
		buttons2[0].sprite = buttonsSp[0];
		buttons2[1].sprite = buttonsSp[0];
		buttons2[2].sprite = buttonsSp[0];
		buttons2[3].sprite = buttonsSp[1];
		resultValueTextComponent.text = alternativeSelected.ToString();
        confirmButton.interactable = true;
    }

	public void buttonConfirm(){
		confirmButton.interactable = false;
		buttons2[0].sprite = buttonsSp[0];
		buttons2[1].sprite = buttonsSp[0];
		buttons2[2].sprite = buttonsSp[0];
		buttons2[3].sprite = buttonsSp[0];
		QuestionCorrection();
	}

	public void QuestionCorrection(){
		timesAsk++;
		bool isRight = alternativeSelected == rightResult;
		ButtonsEnable(false);
		Timing.RunCoroutine(CoroutineCorrection(isRight));
	}

	IEnumerator<float> CoroutineCorrection(bool isRight){
		//TODO Mostrar Parabéns aqui.
		//TODO adicionar pontos.
		congratsPanel.SetActive(true);
        congratsAnimator.enabled = true;
        if (isRight) {
			congratsText.text = "Acertou! \b +30 Pontos";
			//rainbowController.GameConfig.didaticaStatic(true, idHabilidade, idDificuldade, idMinigame);
			log.SaveEstatistica (idHabilidade, idDificuldade, true);
			log.AddPontosPedagogica (30);
            scoreAmount += 30;
            scoreDidatica += 30;           
            congratsAnimator.SetInteger (acertoErrohash,1);
            soundManager.startSoundFX(acertoAudioClip);
           //Debug.Log("1ssr");
		} else {
			congratsText.text = "Errou! \n O certo era " +  rightResult.ToString();
			//rainbowController.GameConfig.didaticaStatic(false, idHabilidade, idDificuldade, idMinigame);
			log.SaveEstatistica (idHabilidade, idDificuldade, false);
            congratsAnimator.SetInteger (acertoErrohash,2);
            soundManager.startSoundFX(erroAudioClip);
            //Debug.Log("2dt");
        }
		yield return Timing.WaitForSeconds(1.5f);
		congratsAnimator.SetInteger (acertoErrohash,0);
		congratsAnimator.SetInteger (acertoErrohash,0);
        confirmButton.interactable = true;
        //congratsAnimator.enabled = false;
        // congratsPanel.SetActive(false);
        NextQuestion();
	}

	public void NextQuestion(){
		if (timesAsk < timesToAsk) {

			switch (anoLetivo)
			{
				case 1:
					var equation = equationContents[timesAsk];
					firstValueGroupContentComponent.SetupItemGroup();
					secondValueGroupContentComponent.SetupItemGroup();
					switch (equation.groupType)
					{
						case GroupType.Juntar:
							enumciadoTextComponent.text =
								$"Manu pegou {equation.firstValue} <sprite name=\"{equation.rainbowItem.name}\">, Bia pegou {equation.secondValue}. " +
								$"Quantos <sprite name=\"{equation.rainbowItem.name}\"> elas pegaram juntas?";
							equationTypeValueTextComponent.text = "+";
							equationTypeValueTextComponentSecond.text = "+";
							firstValueGroupContentComponent.imageComponent.enabled = true;
							secondValueGroupContentComponent.imageComponent.enabled = true;
							break;
						case GroupType.Acrescentar:
							enumciadoTextComponent.text =
								$"Paulo tinha {equation.firstValue} <sprite name=\"{equation.rainbowItem.name}\">, Pegou mais {equation.secondValue}. " +
								$"Com quantos <sprite name=\"{equation.rainbowItem.name}\"> ele ficou?";
							equationTypeValueTextComponent.text = "+";
							equationTypeValueTextComponentSecond.text = "+";
							firstValueGroupContentComponent.imageComponent.enabled = true;
							secondValueGroupContentComponent.imageComponent.enabled = false;
							break;
						case GroupType.Separar:
							enumciadoTextComponent.text =
								$"João e Paulo tinham juntos {equation.firstValue} <sprite name=\"{equation.rainbowItem.name}\">, Separando {equation.secondValue} para Paulo. " +
								$"Com quantos <sprite name=\"{equation.rainbowItem.name}\"> João ficou?";
							equationTypeValueTextComponent.text = "-";
							equationTypeValueTextComponentSecond.text = "-";
							firstValueGroupContentComponent.imageComponent.enabled = true;
							secondValueGroupContentComponent.imageComponent.enabled = true;
							break;
						case GroupType.Retirar:
							enumciadoTextComponent.text =
								$"Zeca conseguiu pegar {equation.firstValue} <sprite name=\"{equation.rainbowItem.name}\">, Mas deu {equation.secondValue} para Tati. " +
								$"Com quantos <sprite name=\"{equation.rainbowItem.name}\"> ele ficou?";
							equationTypeValueTextComponent.text = "-";
							equationTypeValueTextComponentSecond.text = "-";
							firstValueGroupContentComponent.imageComponent.enabled = true;
							secondValueGroupContentComponent.imageComponent.enabled = false;
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}
					//Update Group and bottom value.
					firstValueTextComponent.text = $"<u>{equation.firstValue}</u>";

					firstValueGroupContentComponent.itemSprite.Value = equation.rainbowItem;
					firstValueGroupContentComponent.itemAmount.Value = equation.firstValue;

					secondValueTextComponent.text = $"<u>{equation.secondValue}</u>";

					secondValueGroupContentComponent.itemSprite.Value = equation.rainbowItem;
					secondValueGroupContentComponent.itemAmount.Value = equation.secondValue;

					confirmButton.onClick.RemoveAllListeners();
					resultValueGroupContentComponent.SetupItemGroup();
					resultValueGroupContentComponent.itemAmount.Value = 0;
					confirmButton.OnClickAsObservable().Subscribe(unit =>
					{
						resultValueTextComponent.text = equation.ResultValue().ToString();
						resultValueGroupContentComponent.SetupItemGroup();
						resultValueGroupContentComponent.itemSprite.Value = equation.rainbowItem;
						resultValueGroupContentComponent.itemAmount.Value = equation.ResultValue();
					});

					int correctResult = equation.ResultValue();
					int tempMin;
					if (correctResult - 10 <= 0) {
						tempMin = correctResult - (correctResult - 1);
					} else {
						tempMin = correctResult - 10;
					}

					int tempMax;
					tempMax = correctResult + 10;

					List<int> alternativesTemp = new List<int>();

					alternativesTemp.Add(correctResult);

					for (int i = 0; i < 3; i++) {
						int temp = 0;

						temp = Random.Range(tempMin,tempMax);
						while (alternativesTemp.Contains (temp)) {
							temp = Random.Range(tempMin,tempMax);
						}

						bool isOnList = alternativesTemp.Contains (temp);

						alternativesTemp.Add (temp);

					}

					alternatives = alternativesTemp.OrderBy (x => x).ToArray();

					for (int i = 0; i < 4; i++) {
						alternativeTexts[i].text = alternatives [i].ToString ();
					}

					for (int i = 0; i < alternativeButtons.Length; i++)
					{
						var buttonIndex = alternativeButtons[i];
//						buttonIndex.onClick.RemoveAllListeners();
						var valueIndex = alternatives[i];
						buttonIndex.OnClickAsObservable().Subscribe(unit =>
						{
							alternativeSelected = valueIndex;
							foreach (var button in alternativeButtons)
							{
								button.image.sprite = button == buttonIndex ? buttonsSp[1] : buttonsSp[0];
							}
							resultValueTextComponent.text = alternativeSelected.ToString();
							resultValueGroupContentComponent.SetupItemGroup();
							resultValueGroupContentComponent.itemSprite.Value = equation.rainbowItem;
							resultValueGroupContentComponent.itemAmount.Value = alternativeSelected;
							confirmButton.interactable = true;
						});
					}
					rightResult = correctResult;
					break;
				case 2:
					var equationContent = equationContents[timesAsk];
					firstValueGroupContentComponent.SetupItemGroup();
					secondValueGroupContentComponent.SetupItemGroup();
					firstValueGroupContentComponent.imageComponent.enabled = false;
					secondValueGroupContentComponent.imageComponent.enabled = false;
					resultValueGroupContentComponent.SetupItemGroup();
					resultValueGroupContentComponent.imageComponent.enabled = false;
					switch (equationContent.groupType)
					{
						case GroupType.Juntar:
							enumciadoTextComponent.text = $"Qual o resultado da adição abaixo?";
							equationTypeValueTextComponent.text = "+";
							equationTypeValueTextComponentSecond.text = "+";
							break;
						case GroupType.Acrescentar:
							enumciadoTextComponent.text = $"Qual o resultado da adição abaixo?";
							equationTypeValueTextComponent.text = "+";
							equationTypeValueTextComponentSecond.text = "+";
							break;
						case GroupType.Separar:
							enumciadoTextComponent.text = $"Qual o resultado da subtração abaixo?";
							equationTypeValueTextComponent.text = "-";
							equationTypeValueTextComponentSecond.text = "-";
							break;
						case GroupType.Retirar:
							enumciadoTextComponent.text = $"Qual o resultado da subtração abaixo?";
							equationTypeValueTextComponent.text = "-";
							equationTypeValueTextComponentSecond.text = "-";
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}
					//Update Group and bottom value.
					firstValueTextComponent.text = $"<u>{equationContent.firstValue}</u>";

					firstValueGroupContentComponent.itemSprite.Value = equationContent.rainbowItem;
					firstValueGroupContentComponent.itemAmount.Value = equationContent.firstValue;

					secondValueTextComponent.text = $"<u>{equationContent.secondValue}</u>";

					secondValueGroupContentComponent.itemSprite.Value = equationContent.rainbowItem;
					secondValueGroupContentComponent.itemAmount.Value = equationContent.secondValue;

					confirmButton.onClick.RemoveAllListeners();
					resultValueGroupContentComponent.SetupItemGroup();
					resultValueGroupContentComponent.itemAmount.Value = 0;
					confirmButton.OnClickAsObservable().Subscribe(unit =>
					{
						resultValueTextComponent.text = equationContent.ResultValue().ToString();
						resultValueGroupContentComponent.SetupItemGroup();
						resultValueGroupContentComponent.itemSprite.Value = equationContent.rainbowItem;
						resultValueGroupContentComponent.itemAmount.Value = equationContent.ResultValue();
					});

					SetButtonAlternatives(equationContent);
					break;
				case 3:

					var tempRandom = Random.Range(0, 3);

					var equationMultiply = equationContents[timesAsk];
//					multiplyInAdditionComponent.equationContent.Value = equationMultiply;

					var resultText = tempRandom == 0 ? $"= {equationMultiply.ResultValue()}" : "?";

//					resultOfAdditionTextComponent.text = resultText;
//					resultOfMultiplyTextComponent.text = resultText;

					var firstValueText = tempRandom == 1 ? equationMultiply.firstValue.ToString() : "?";
					var secondValueText = tempRandom == 2 ? equationMultiply.secondValue.ToString() : "?";
//					multiplyEquationText.text = $"{firstValueText} x {secondValueText}";

					multiplyInAdditionComponent.OnEquationChanged(equationMultiply,
						new TextMeshProUGUI[]{resultOfAdditionTextComponent, resultOfMultiplyTextComponent, multiplyEquationText},
						() =>
						{
							resultOfAdditionTextComponent.text = resultText;
							resultOfMultiplyTextComponent.text = resultText;
							multiplyEquationText.text = $"{firstValueText} x {secondValueText}";
						});
					break;
			}

//			if (timesAsk == 0) {
//
//                int firstTemp = Random.Range(6, 9);
//                int secondTemp = Random.Range(1, 5);
//                idHabilidade = 5;
//
//                ConfigureQuestion(firstTemp, secondTemp, true);
//				ColorHighlight(0, 1);
//			} else if (timesAsk == 1) {
//
//                int firstTemp = Random.Range(6, 9);
//                int secondTemp = Random.Range(1, 5);
//                idHabilidade = 6;
//                ConfigureQuestion(firstTemp, secondTemp, false);
//				ColorHighlight(0,2);
//			}else if (timesAsk == 2) {
//
//                int firstTemp = Random.Range(10, 15);
//                int secondTemp = Random.Range(1, 5);
//                idHabilidade = 7;
//                ConfigureQuestion(firstTemp, secondTemp, true);
//				ColorHighlight(1, 2);
//			}else if (timesAsk == 3) {
//
//                int firstTemp = Random.Range(10, 15);
//                int secondTemp = Random.Range(1, 5);
//                idHabilidade = 8;
//                ConfigureQuestion(firstTemp, secondTemp, false);
//				ColorHighlight(0, 1);
//			}
            confirmButton.interactable = false;
            ButtonsEnable(true);
        } else {
			//TODO chamar painel de desafio aqui.
			Timing.RunCoroutine(CallChalenge());
		}
	}

	private void SetButtonAlternatives(EquationContent equationContent, int tempRandom = 0)
	{
		int resultValue = 0;

		if (anoLetivo == 3)
		{
			switch (tempRandom)
			{
				case 0:
					resultValue = equationContent.ResultValue();
					break;
				case 1:
					resultValue = equationContent.firstValue;
					break;
				default:
					resultValue = equationContent.secondValue;
					break;
			}
		}

		else
		{
			resultValue = equationContent.ResultValue();
		}
		int min;
		if (resultValue - 10 <= 0)
		{
			min = resultValue - (10 - math.abs(resultValue - 10));
		}
		else
		{
			min = resultValue - 10;
		}

		var max = resultValue + 10;

		List<int> ints = new List<int> {resultValue};

		for (int i = 0; i < 3; i++)
		{
			int temp = 0;

			temp = Random.Range(min, max);
			while (ints.Contains(temp))
			{
				temp = Random.Range(min, max);
			}

			bool isOnList = ints.Contains(temp);

			ints.Add(temp);
		}

		alternatives = ints.OrderBy(x => x).ToArray();

		for (int i = 0; i < 4; i++)
		{
			alternativeTexts[i].text = alternatives[i].ToString();
		}

		for (int i = 0; i < alternativeButtons.Length; i++)
		{
			var buttonIndex = alternativeButtons[i];
//						buttonIndex.onClick.RemoveAllListeners();
			var valueIndex = alternatives[i];
			buttonIndex.OnClickAsObservable().Subscribe(unit =>
			{
				alternativeSelected = valueIndex;
				foreach (var button in alternativeButtons)
				{
					button.image.sprite = button == buttonIndex ? buttonsSp[1] : buttonsSp[0];
				}

				if (anoLetivo != 3)
				{
					resultValueTextComponent.text = alternativeSelected.ToString();
					resultValueGroupContentComponent.SetupItemGroup();
					resultValueGroupContentComponent.itemSprite.Value = equationContent.rainbowItem;
					resultValueGroupContentComponent.itemAmount.Value = alternativeSelected;
				}

				confirmButton.interactable = true;
			});
		}

		rightResult = resultValue;
	}

	IEnumerator<float> CallChalenge(){
		challengeController.scoreAmount = this.scoreAmount;
		challengeController.starAmount = this.starAmount;
		chalengePanel.SetActive(true);
		//rainbowController.HUD [1].SetActive (true);
		pointT.SetActive (false);
		chalengePanelAnimator.SetInteger(panelDesafioNumberHash, 1);
		yield return Timing.WaitForSeconds(2f);
		mainPanel.SetActive(false);
		this.enabled = false;

		log.StartTimerDidatica (false);

	}


	public void ColorHighlight(int _first, int _second){
		/*for (int i = 0; i < 3; i++) {
			if (i == _first || i == _second) {
				imagePanels[i].color = selectionColor;
			} else {
				imagePanels[i].color = defaultColor;
			}
		}*/
	}

	public void ConfigureQuestion(int _first,int _second,bool isAddition){
		resultOfOperation.text = "?";
		firstNumberText.text = _first.ToString();
		secondNumberText.text = _second.ToString();

		int tempMin;
		int tempMax;
		int correctResult;

		if (isAddition) {
			correctResult = _first + _second;
			sinalOfOperationText.text = "+";
            contNunbAudioPos = 1;
            if (contNunbAudioPos<2) {
                contNunbAudioPos = contNunbAudioPos+1;
                tutorPanelArcoirirs2.soundManager.startVoiceFXReturn(tutorPanelArcoirirs2.audiosTutorial[8]);
            }
           
            facaConta.text = facaContaT[0];
		} else {
			correctResult = _first - _second;
			sinalOfOperationText.text = "-";
            if (contNunbAudioNeg < 2) {
                contNunbAudioNeg = contNunbAudioNeg+1;
                tutorPanelArcoirirs2.soundManager.startVoiceFXReturn(tutorPanelArcoirirs2.audiosTutorial[9]);
            }           
            facaConta.text = facaContaT[1];
		}

		if (correctResult - 10 <= 0) {
			tempMin = correctResult - (correctResult - 1);
		} else {
			tempMin = correctResult - 10;
		}

		tempMax = correctResult + 10;

		List<int> alternativesTemp = new List<int>();

		alternativesTemp.Add(correctResult);

		for (int i = 0; i < 3; i++) {
			int temp = 0;

			temp = Random.Range(tempMin,tempMax);
			while (alternativesTemp.Contains (temp)) {
				temp = Random.Range(tempMin,tempMax);
			}

			bool isOnList = alternativesTemp.Contains (temp);

			alternativesTemp.Add (temp);

		}

		alternatives = alternativesTemp.OrderBy (x => x).ToArray();

		for (int i = 0; i < 4; i++) {
			alternativeTexts[i].text = alternatives [i].ToString ();
		}

		confirmButton.interactable = true;

		rightResult = correctResult;

		ButtonsEnable(true);
	}

	public void ButtonsEnable(bool _enable){
		for (int i = 0; i < 4; i++) {
			buttonsAlternative[i].interactable = _enable;
		}
	}
		


}