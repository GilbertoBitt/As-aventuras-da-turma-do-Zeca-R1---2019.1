﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

[System.SerializableAttribute]
public class GeometryPlaces_1_2B : MonoBehaviour, IPointerClickHandler{

	public Image imageComponent;
	public Manager_1_2B manager;
	public Manager_1_2B.geometryForm form;
	public SpacialForms spacialForm;
	public PlaneFigures planeFigures;
	public bool canbeFound = false;
	private NicerOutline _outline;

	private void Start()
	{
		_outline = GetComponent(typeof(NicerOutline)) as NicerOutline;
		if (_outline == null)
		{
			_outline = gameObject.AddComponent(typeof(NicerOutline)) as NicerOutline;
		}

		if (_outline == null) return;
		_outline.enabled = false;
		_outline.effectDistance = new Vector2(5f, -5f);
		_outline.effectColor = Color.white;

	}



	public void OnPointerClick(PointerEventData eventData){
		if (canbeFound != true || manager.canBePlayed != true) return;
		manager.AddUserPick(this);
		manager.hasFound++;
		imageComponent.color = Color.clear;
		canbeFound = false;
		manager.soundManager.startVoiceFX(manager.clipsAudio[0]);
	}

	public void updateImage(Sprite spriteItem){
		imageComponent.sprite = spriteItem;
		imageComponent.color = Color.white;
		imageComponent.preserveAspect = true;
		canbeFound = true;
	}

	public void updateImage(Sprite spriteItem, Color Outline){
		imageComponent.sprite = spriteItem;
		imageComponent.color = Color.white;
		imageComponent.preserveAspect = true;
		canbeFound = true;
		if (_outline == null)
		{
			Start();
		}
		_outline.enabled = true;
	}

	public void resetConfig(){
		form = Manager_1_2B.geometryForm.none;
		spacialForm = SpacialForms.None;
		planeFigures = PlaneFigures.None;
		canbeFound = false;
		imageComponent.sprite = null;
		imageComponent.color = Color.clear;
		if (_outline == null)
		{
			Start();
		}
		_outline.enabled = false;
	}
	
}

public enum SpacialForms
{
	None,
	Cube,
	Retangular,
	Pyramid,
	Cone,
	Cylinder,
	Sphere
}

public enum PlaneFigures
{
	None,
	Triangle,
	Square,
	Rectangle,
	Trapeze,
	Parallelogram,
}


