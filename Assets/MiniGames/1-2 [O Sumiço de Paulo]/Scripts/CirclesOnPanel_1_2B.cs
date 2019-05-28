using System;
using System.Collections;
using System.Collections.Generic;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class CirclesOnPanel_1_2B : MonoBehaviour, IPointerClickHandler{
	public Image imageComp;
	public Image buttonRemove;
	public Image correctCompImage;
	public GeometryPlaces_1_2B placeOfItem;
	public Outline outlineImage;
	public Manager_1_2B manager;

	

	public void OnPointerClick(PointerEventData eventData){
		if(placeOfItem != null && manager.canBePlayed == true){			
			onClick();
		}
	}

	public void onClick(){
        //placeOfItem.updateImage(imageComp.sprite);
        //placeOfItem.canbeFound = true;
        //manager.RemoveUserPick(placeOfItem);
        //manager.hasFound--;
        //manager.soundManager.startSoundFX(manager.clipsAudio[1]);
        switch (manager.anoLetivo)
        {
	        case 1:
		        manager.ShowObjectFormAnimation(placeOfItem.form);
		        break;
	        case 2:
		        manager.ShowObjectFormAnimation(placeOfItem.spacialForm);
		        break;
	        case 3:
		        manager.ShowObjectFormAnimation(placeOfItem.planeFigures);
		        break;
        }
	}

	public void updateImage(GeometryPlaces_1_2B place){
		manager = place.manager;
		switch (manager.anoLetivo)
		{
			case 1:
				imageComp.sprite = place.imageComponent.sprite;
				imageComp.preserveAspect = true;
				break;
			case 2:
				imageComp.sprite = place.imageComponent.sprite;
				imageComp.preserveAspect = true;
				break;
			case 3:
				imageComp.sprite = place.imageComponent.sprite;
				imageComp.color = place.imageComponent.color;
				imageComp.preserveAspect = true;
				break;
		}

		placeOfItem = place;
	}

	public void updateImage(){
		imageComp.sprite = null;
		placeOfItem = null; 
	}

	public void hideCloseButtton(){
		buttonRemove.gameObject.SetActive(false);
	}

	public void showCloseButton(){
		buttonRemove.gameObject.SetActive(true);
	}


}
