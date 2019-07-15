using System;
using System.Collections;
using System.Collections.Generic;
using com.csutil;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(GraphicRaycaster))]
public class InfoSkillWindow : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    private Canvas _canvas;
    private GraphicRaycaster _graphicRaycaster;
    public TextMeshProUGUI textInfoDescription;
    public PauseManager pauseManager;


    private void Start()
    {
        _canvasGroup = GetComponent(typeof(CanvasGroup)) as CanvasGroup;
        if (pauseManager == null)
        {
            pauseManager = FindObjectOfType<PauseManager>();
        }
        _canvas = GetComponent(typeof(Canvas)) as Canvas;
        _graphicRaycaster = GetComponent(typeof(GraphicRaycaster)) as GraphicRaycaster;

        IoC.inject.SetSingleton(this);
    }

    public void ShowWindowInfo(HabilidadeBNCCInfo targetInfo)
    {
        textInfoDescription.text = $"Habilidade: ({targetInfo.codigo}) {targetInfo.description}";
        _graphicRaycaster.enabled = true;
        _canvas.enabled = true;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.DOFade(1f, 0.3f).OnComplete(() =>
        {
            pauseManager.Pause();
            pauseManager.soundManager.musicBack.Play(pauseManager.soundManager.backgroundMusic);
        });
    }

    public void ShowWindowInfo(HabilidadeBNCCInfo targetInfo, HabilidadeBNCCInfo targetInfo2)
    {
        textInfoDescription.text = $"Habilidade: <link=\"google.com.br\">({targetInfo.codigo})<link> {targetInfo.description} \n\nHabilidade: ({targetInfo2.codigo}) {targetInfo2.description}";
        _graphicRaycaster.enabled = true;
        _canvas.enabled = true;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.DOFade(1f, 0.3f).OnComplete(() => { pauseManager.Pause(); });
    }

    public void HideWindowsInfo()
    {

        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
        pauseManager.Unpause();
        _canvasGroup.DOFade(0f, 0.3f).OnComplete(() =>
        {
            _canvas.enabled = false;
            _graphicRaycaster.enabled = false;
        });
    }

}


