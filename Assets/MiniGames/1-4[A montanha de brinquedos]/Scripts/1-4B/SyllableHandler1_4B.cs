using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;
using UniRx;

public class SyllableHandler1_4B : OverridableMonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler {

	Outline outlineComp;
	public CanvasGroup thisCanvasGroup;
	Vector3 startDragPos;
	public Manager1_4B manager;
	public TextMeshProUGUI textComp;
	public string syllable;
	public bool isOverBlank = false;
	public bool hasDroped = false;
	public bool isBeenDrag = false;
	public BlankSpace1_4B blankSpaceDroped;
	public Image imageComp;
	private float zValue;
	private Camera _camera;
	private Vector3 _position;

    public void ResetToDefault() {
//	    _position = transform.position;
        thisCanvasGroup.alpha = 0;
        textComp.text = "";
        syllable = "";
        isOverBlank = false;
        hasDroped = false;
        isBeenDrag = false;
        blankSpaceDroped = null;
        transform.SetParent(manager.blueBoxParent);
//        transform.position = new Vector3(5000, 5000, 0);
    }

    public void ResetToDefault(Transform parent) {
	    thisCanvasGroup.alpha = 0;
	    textComp.text = "";
	    syllable = "";
	    isOverBlank = false;
	    hasDroped = false;
	    isBeenDrag = false;
	    blankSpaceDroped = null;
//	    transform.SetParent(parent);
//	    transform.position = new Vector3(5000, 5000, 0);
    }




	// Use this for initialization
	void Start () {
		if (outlineComp == null) {
			outlineComp = this.GetComponent<Outline>();
		}

		if (thisCanvasGroup == null) {
			thisCanvasGroup = this.GetComponent<CanvasGroup>();
		}

		if (imageComp == null) {
			imageComp = this.GetComponent<Image>();
		}

		_camera = Camera.main;
		_position = transform.position;

		Observable.EveryLateUpdate().Where(x => isOverBlank && !hasDroped && !isBeenDrag).Subscribe(l =>
		{
			manager.isDragging = false;
			isOverBlank = false;
			transform.SetParent(manager.blueBoxParent);
			manager.UpdatedGridLayout();
		});
	}

//    public override void LateUpdateMe() {
//	    if (blankSpaceDroped != null || isBeenDrag || (!hasDroped && !isOverBlank)) return;
//	    thisCanvasGroup.blocksRaycasts = true;
//        manager.isDragging = false;
//        hasDroped = false;
//        isOverBlank = false;
////        this.transform.SetParent(manager.syllablesParent);
//    }

    public void OnBeginDrag (PointerEventData eventData)
    {
	    thisCanvasGroup.blocksRaycasts = false;
	    manager.isDragging = true;
	    if (hasDroped && blankSpaceDroped != null)
	    {
		    blankSpaceDroped.RaycastTargetUpdate(true);
		    blankSpaceDroped.hasDrop = false;
		    blankSpaceDroped.thisSyllable = null;
		    blankSpaceDroped = null;
	    }
	    hasDroped = false;
	    this.transform.SetParent(manager.poolParent);
	    var pos = Input.mousePosition;
	    pos.z = 100f;
	    transform.position = _camera.ScreenToWorldPoint(pos);
	    isBeenDrag = true;
    }

	public void OnDrag (PointerEventData eventData){
		var pos = Input.mousePosition;
		pos.z = 100f;
		transform.position = _camera.ScreenToWorldPoint(pos);
		manager.isDragging = true;
	}

	public void OnEndDrag (PointerEventData eventData){
		thisCanvasGroup.blocksRaycasts = true;
		if (!isOverBlank) {
			transform.SetParent(manager.blueBoxParent);
//			transform.position = _position;
			manager.UpdatedGridLayout();
		}
		isBeenDrag = false;
		manager.isDragging = false;
	}

	public void UpdateTextContent(){
		textComp.text = syllable;
	}

	public void UpdateTextContent(string textToUpdate)
	{
		syllable = textToUpdate.ToUpper();
		textComp.text = syllable;
	}

    public void DoFade(float alpha) {
        thisCanvasGroup.DOFade(alpha, 0.3f);
    }
}
