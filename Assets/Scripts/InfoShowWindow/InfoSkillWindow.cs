using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class InfoSkillWindow : MonoBehaviour
{

    private CanvasGroup _canvasGroup;
    public TextMeshProUGUI textInfoDescription;

    private void Start()
    {
        _canvasGroup = GetComponent(typeof(CanvasGroup)) as CanvasGroup;
    }

    public void ShowWindowInfo(HabilidadeBNCCInfo targetInfo)
    {
        textInfoDescription.text = targetInfo.description;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.DOFade(1f, 0.3f);
    }

    public void HideWindowsInfo()
    {
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.DOFade(0f, 0.3f);
    }

}

[CreateAssetMenu(menuName = "Eduqbrinq/BNCC/Habilidade Info", fileName = "Habilidade")]
public class HabilidadeBNCCInfo : ScriptableObject
{
    public string codigo;
    public int id;
    [TextArea(3,50)]
    public string description;
}
