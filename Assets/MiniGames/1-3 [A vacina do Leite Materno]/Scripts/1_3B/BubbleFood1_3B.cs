﻿using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.UI;
using MEC;
using DG.Tweening;
using MiniGames.Scripts._1_3B;
using Sirenix.OdinInspector;
using TMPro;

public class BubbleFood1_3B : MonoBehaviour {

	public SpriteRenderer spriteRenderer;
	public FoodItem1_3B food;
	public ItemWord1_3b word;
	public AnimationCurve fadeInCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
	public Vector3 originalPos;
	public Manager1_3B manager;
	public bool isLooping = false;
	public float delayLooping;
    public SpriteRenderer iconSpriteRender;
    public SpriteRenderer bubbleSpriteRender;
    public GameObject partExploBolha;
    public Transform bolha;
    private ParticleSystem _particle;
    public TextMeshPro textItem;
    public SpriteRenderer incorrectIcon;
    public ParticleSystem[] particleSystems;
    public void Awake() {
        _particle = partExploBolha.GetComponent<ParticleSystem>();
    }

    public void UpdateFood(FoodItem1_3B _food){
		food = _food;
        bubbleSpriteRender.color = Color.white;
        iconSpriteRender.sprite = food.spriteItem;
		originalPos = this.transform.position;
	}

    public void UpdateFood(ItemWord1_3b wordTemp){
	    word = wordTemp;
	    bubbleSpriteRender.color = Color.white;
	    iconSpriteRender.sprite = wordTemp.itemSprite;
	    originalPos = this.transform.position;
	    textItem.SetText(word.itemWord.ToUpperInvariant());
	    incorrectIcon.DOFade(0f, 0.1f);
    }

	void Update(){
		if (!manager.isPlaying || !isLooping) return;
		Vector3 pos = this.transform.position;
		pos.x = originalPos.x + (Mathf.PingPong(Time.time,2.0f) - 1f);
		this.transform.position = pos;
	}

	public void StartFadeIn(float delay){
		
		//Timing.RunCoroutine (FadeIn (delay), Segment.Update);
        iconSpriteRender.DOFade(1f, delay);
        bubbleSpriteRender.DOFade(1f, delay);
        this.transform.DOScale(0.5f, delay);
        isLooping = true;

	}

	public void StartFadeOut(float _delay){
		isLooping = false;
        iconSpriteRender.DOFade(0f, _delay);
        bubbleSpriteRender.DOFade(0f, _delay);
        this.transform.DOScale(Vector3.zero, _delay).OnComplete(returnToOrigin);
        if(textItem.color.a >= 0.3f) textItem.DOFade(0f, .3f);
        if(incorrectIcon.color.a >= .3f) incorrectIcon.DOFade(0f, 0.1f);
        //Timing.RunCoroutine (FadeOut (_delay));
	}

	public void MakeItRed(){
        bubbleSpriteRender.color = Color.red;
        incorrectIcon.DOFade(1f, .3f);
    }

    IEnumerator<float> FadeIn(float delay){
		float times = 0.0f;

		Vector3 scaleStart = new Vector3 (0f, 0f, 0f);
		Vector3 scaleEnd = new Vector3 (0.5f, 0.5f, 0.5f);

		Color WhiteZeroAlpha = new Vector4 (1f, 1f, 1f, 0f);

		while (times < delay)
		{
			times += Time.deltaTime;
			float s = times / delay;

			this.transform.localScale = Vector3.Lerp (scaleStart, scaleEnd, fadeInCurve.Evaluate (s));
            iconSpriteRender.color = Color.Lerp (WhiteZeroAlpha, Color.white, fadeInCurve.Evaluate (s));
            bubbleSpriteRender.color = Color.Lerp(WhiteZeroAlpha, Color.white, fadeInCurve.Evaluate(s));
            yield return Timing.WaitForOneFrame;
        }
		isLooping = true;
	}

	IEnumerator<float> FadeOut(float _delay) {
		float times = 0f;
        //float delay = 1f;

        Vector3 scaleStart = new Vector3 (0.5f, 0.5f, 0.5f);
		Vector3 scaleEnd = new Vector3 (0f, 0f, 0f);

		Color WhiteZeroAlpha = new Vector4 (1f, 1f, 1f, 0f);
		Color startColor = spriteRenderer.color;

		while (times < _delay)
		{
			times += Time.deltaTime;
			float s = times / _delay;
            this.transform.localScale = Vector3.Lerp(scaleStart, scaleEnd, fadeInCurve.Evaluate(s));
            iconSpriteRender.color = Color.Lerp(startColor, WhiteZeroAlpha, fadeInCurve.Evaluate(s));
            bubbleSpriteRender.color = Color.Lerp(startColor, WhiteZeroAlpha, fadeInCurve.Evaluate(s));

            yield return Timing.WaitForOneFrame;
		}

		returnToOrigin ();
	}


	public void StartCorrectcenter(float delay){
		isLooping = false;
        Vector3 worldCenterPosition = manager.positionCenter.position;
        worldCenterPosition.z = this.transform.position.z;
        this.transform.DOMove(worldCenterPosition, delay).OnComplete(() => { ParticleTrigger(); });
//		Timing.RunCoroutine (GoToCenter (delay));
	}

	[Button("Trigger Particles")]
	private void ParticleTrigger()
	{
		particleSystems.ForEach(system => { system.Play(); });
	}

	IEnumerator<float> GoToCenter(float delay){
		float times = 0.0f;


		Vector3 worldCenterPosition = manager.positionCenter.position;
		worldCenterPosition.z = this.transform.position.z;
		Vector3 startPos = this.transform.position;
		Color colorEnd = Color.green;
		Color startColor = spriteRenderer.color;

		while (times < delay)
		{
			times += Time.deltaTime;
			float s = times / delay;

			this.transform.position = Vector3.Lerp (startPos, worldCenterPosition, fadeInCurve.Evaluate (s));
			//this.bubbleSpriteRender.color = Color.Lerp (startColor, colorEnd, fadeInCurve.Evaluate (s));

			yield return Timing.WaitForOneFrame;
        }


	}

	public void returnToOrigin(){
		this.transform.position = originalPos;
	}


	void OnTriggerEnter2D(Collider2D other) {
		manager.OnBulletHit (this);
        BulletManager1_3B temp = other.GetComponent<BulletManager1_3B>();
        temp.ResetBullet();
        Color tempColor = Color.white;
        tempColor.a = 0f;
        bubbleSpriteRender.color = tempColor;
        _particle.Play();
        //GameObject explo = Instantiate(partExploBolha, transform.position, transform.rotation) as GameObject;
        partExploBolha.transform.SetParent(bolha.transform);
        var localScale = partExploBolha.transform.localScale;
        localScale = new Vector3(localScale.x, localScale.y, localScale.z);
        partExploBolha.transform.localScale = localScale;
	}

}
