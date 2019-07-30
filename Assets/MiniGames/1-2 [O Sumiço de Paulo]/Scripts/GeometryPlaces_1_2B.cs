using System;
using System.Collections;
using System.Collections.Generic;
using Coffee.UIExtensions;
using DG.Tweening;
using MiniGames.Scripts;
using Sirenix.OdinInspector;
using UniRx;
using UnityAtoms.Extensions;
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
	public GeometricPlaneObject planeObject;

	[Space(20)]
	[BoxGroup("Scale")]
	public AnimationCurve scaleCurveAnimation;

	[BoxGroup("Scale")] public float scaleAnimationDuration;
	[BoxGroup("Scale")] public Transform scaleTransformTarget;

	[BoxGroup("Glow Effect")]
	public AnimationCurve glowEffectAnimationCurve;

	[BoxGroup("Glow Effect")]
	public UIEffect effectComponent;

	private Sequence _animationSequence;

	private FloatReactiveProperty _effectFactorUpdated = new FloatReactiveProperty(0f);

	[ButtonGroup]
	[Button("Play Animation")]
	public void StartAnimations()
	{
		_animationSequence = DOTween.Sequence();
		_animationSequence.Append(scaleTransformTarget.DOScale(new Vector3(1.2f, 1.2f, 1.2f), scaleAnimationDuration).SetEase(scaleCurveAnimation).From(Vector3.one));
		_animationSequence.Join(DOTween.To(() => effectComponent.colorFactor, x => effectComponent.colorFactor = x, 0.8f, scaleAnimationDuration).From(0f).SetEase(glowEffectAnimationCurve));
		_animationSequence.SetId(1000);
		_animationSequence.SetLoops(-1, LoopType.Yoyo);
		_animationSequence.Play();
	}

	[ButtonGroup]
	[Button("Stop Animation")]
	public void StopAnimations()
	{
		DOTween.Kill(1000);
		DOTween.To(() => effectComponent.colorFactor, x => effectComponent.colorFactor = x, 0f,
			scaleAnimationDuration);
		scaleTransformTarget.DOScale(Vector3.one, scaleAnimationDuration);
	}


	private void Start()
	{

		effectComponent = this.GetOrAddComponent<UIEffect>();
		scaleTransformTarget = transform;

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
		imageComponent.enabled = false;
		canbeFound = false;
		manager.soundManager.startVoiceFX(manager.clipsAudio[0]);
	}

	public void updateImage(Sprite spriteItem){
		imageComponent.sprite = spriteItem;
		imageComponent.color = Color.white;
		imageComponent.enabled = true;
		imageComponent.preserveAspect = true;
		canbeFound = true;
	}

	public void updateImage(Sprite spriteItem, Color Outline){
		imageComponent.sprite = spriteItem;
		imageComponent.color = Outline;
		imageComponent.preserveAspect = true;
		canbeFound = true;
		if (_outline == null)
		{
			Start();
		}
		_outline.enabled = true;
		_outline.effectColor = Outline;
	}

	public void resetConfig(){
		form = Manager_1_2B.geometryForm.none;
		spacialForm = SpacialForms.None;
		planeFigures = PlaneFigures.None;
		planeObject = null;
		canbeFound = false;
		imageComponent.sprite = null;
		imageComponent.color = Color.clear;
		transform.rotation = Quaternion.Euler(0f,0f,0f);
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




