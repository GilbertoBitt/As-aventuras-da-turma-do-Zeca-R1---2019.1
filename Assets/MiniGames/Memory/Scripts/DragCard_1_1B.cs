using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using com.csutil;
using MiniGames.Memory.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class DragCard_1_1B : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler  {
	
	public Manager_1_1B manager;
	public CardItem characterSprite;
	public TextMeshProUGUI TextComponent;
	public TextMeshProUGUI RightOneTextComponent;
	public UnityEvent OnBeginDragE;
	public UnityEvent OnDragE;
	public UnityEvent OnEndDragE;

	public bool hasDroped = false;
	public DropCard_1_1B dropedCard;
	public Transform OriginalParent;
	public Vector3 pos;
	public GameObject ParticleItem;
	public CanvasGroup thisCanvasGroup;
	public bool isBeenDrag = false;
	
	// Use this for initialization
	void Start () {
		if(manager == null){
			manager = FindObjectOfType<Manager_1_1B>();
		}

		thisCanvasGroup = GetComponent<CanvasGroup>();
		
	}
	
	// Update is called once per frame


	public void OnBeginDrag(PointerEventData eventData){
		if(manager.isPlayTime && Input.touchCount <= 1) {
			isBeenDrag = true;
			ParticleItem.SetActive(true);
			OnBeginDragE.Invoke();
			thisCanvasGroup.blocksRaycasts = false;
			//manager.sound.startSoundFX(findAudio(characterSprite.name));
			if(hasDroped){
				hasDroped = false;
				dropedCard.cardDraged = null;
				this.transform.SetParent(OriginalParent);
				transform.localScale = new Vector3(1f,1f,1f);
			} else {
				pos = this.transform.position;
			}
		}
	}

	public void OnDrag(PointerEventData data){
		if(manager.isPlayTime && Input.touchCount <= 1){
			OnDragE.Invoke();
			float distance = this.transform.position.z - Camera.main.transform.position.z;
			Vector3 pos = new Vector3(Input.mousePosition.x,Input.mousePosition.y,distance);
			this.transform.position = Camera.main.ScreenToWorldPoint(pos);
		}
	}

    public void OnEndDrag(PointerEventData eventData) {
	    EndDrag();
    }

	public void EndDrag() {
		OnEndDragE.Invoke();
		isBeenDrag = false;
		thisCanvasGroup.blocksRaycasts = true;
		manager.groupLayout.enabled = false;
		manager.groupLayout.enabled = true;
		ParticleItem.SetActive(false);
	}

	private void LateUpdate() {
		if (!Input.GetMouseButton(0) && isBeenDrag) {
			EndDrag();
		}
	}

	public void UpdateSprite(CardItem cardItem, int anoLetivo){
		characterSprite = cardItem;
		if (anoLetivo == 2)
		{
			TextComponent.SetText(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(characterSprite.NameItem.ToLower()));
			Log.d("Valores de texto", TextComponent.text);
		}
		else
		{
			TextComponent.SetText(characterSprite.NameItem.ToUpper(CultureInfo.CurrentCulture));
		}
		
		
		
	}
	
	

	public void Clear(){
		dropedCard = null;
		hasDroped = false;
	}

}
