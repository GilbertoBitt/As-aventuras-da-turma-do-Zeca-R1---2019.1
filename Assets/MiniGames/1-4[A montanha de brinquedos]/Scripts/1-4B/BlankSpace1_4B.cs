﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DG.Tweening;

public class BlankSpace1_4B : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {

	public Manager1_4B manager;
	public SyllableHandler1_4B thisSyllable;
	public bool hasDrop;
	public Image thisImageComp;

	public Image traceImageComp;

	private Transform _parentTransform;
	// Use this for initialization

    public void ResetToDefault() {
//        transform.SetParent(manager.poolParent);
//        transform.position = new Vector3(10000, 10000, 0);
        hasDrop = false;
        thisSyllable = null;
        thisImageComp.DOFade(0f, 0.1f);
        RaycastTargetUpdate(true);
    }

    public void ResetToDefault(Transform parent)
    {
	    _parentTransform = parent;
	    transform.SetParent(parent);
//	    transform.position = new Vector3(10000, 10000, 0);
	    hasDrop = false;
	    thisSyllable = null;
	    thisImageComp.DOFade(0f, 0.1f);
	    RaycastTargetUpdate(true);
    }

	void Start () {
		thisImageComp = this.GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {

		/*if (thisSyllable != null && hasDrop == true) {
			if (thisSyllable.transform.parent != manager.dragParent) {
				thisSyllable = null;
				hasDrop = false;
			}
		}*/

	}

	public void OnDrop (PointerEventData eventData){
		if (eventData.pointerDrag == null) return;
		//Debug.Log(eventData.pointerDrag.name);
		SyllableHandler1_4B syllable = eventData.pointerDrag.GetComponent<SyllableHandler1_4B>();
		if (syllable == null || thisSyllable != null) return;
		syllable.gameObject.transform.SetParent(manager.poolParent);
		syllable.transform.position = this.transform.position;
		//syllable.transform.SetParent(this.transform);
		thisSyllable = syllable;
		syllable.hasDroped = true;
		hasDrop = true;
		syllable.blankSpaceDroped = this;
		RaycastTargetUpdate(false);
		manager.DropCardSound();
	}

	public void OnPointerEnter(PointerEventData eventData){
		if (eventData.pointerDrag == null) return;
		SyllableHandler1_4B syllable = eventData.pointerDrag.GetComponent<SyllableHandler1_4B>();
		if (syllable != null) {
			syllable.isOverBlank = true;
		}
	}

	public void OnPointerExit(PointerEventData eventData){
		if (eventData.pointerDrag == null) return;
		SyllableHandler1_4B syllable = eventData.pointerDrag.GetComponent<SyllableHandler1_4B>();
		if (syllable != null) {
			syllable.isOverBlank = false;
			RaycastTargetUpdate(true);
		}
	}


	public void RaycastTargetUpdate(bool isEnable){
		thisImageComp.raycastTarget = isEnable;
	}

    public void DoFade(float _alpha) {
        thisImageComp.DOFade(_alpha, 0.3f);
    }
}
