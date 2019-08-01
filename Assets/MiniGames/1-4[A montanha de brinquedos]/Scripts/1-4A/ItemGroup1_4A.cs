﻿using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemGroup1_4A : OverridableMonoBehaviour {

	public Item1_4A itemInfo;
	private Image imageComp;
	public Manager1_4A manager;
	public bool isBeenDrag = false;
	public bool hasObjectOnRight = false;
	public bool hasObjectOnLeft = false;
	private Transform parent;
	private CircleCollider2D circleCollider;
	public Transform leftObject;
	public Transform rightObject;
	public ItemHandler1_4A thisItemHandler;
	public bool isBonusItem = false;
	public GameObject bonusItemParticle;
	public Image lockImageComp;
	public Image backgroundImageComp;
    

	public GameObject partExplo;
	GameObject partExploClone;
	int nubRand;
	bool checkExplo;
	Color[] corLuz = new Color[7];
	public GameObject tutorial;
	public int tutorObBonus;
   // public TutorMontanha TutorMontanha2;

    public CanvasGroup canvasGroupComp;
    public float timeToShowLock = 0.3f;

    // Use this for initialization
    void Start () {

        canvasGroupComp = this.GetComponent(typeof(CanvasGroup)) as CanvasGroup;

        corLuz[0] = new Vector4(0, 0, 1, 1);
		corLuz[1] = new Vector4(0, 1, 1, 1);
		corLuz[2] = new Vector4(0, 1, 0, 1);
		corLuz[3] = new Vector4(1, 0, 1, 1);
		corLuz[4] = new Vector4(0, 1, 1, 1);
		corLuz[5] = new Vector4(1, 0, 0, 1);
		corLuz[6] = new Vector4(1, 0.9f, 0.016f, 1);	

		imageComp = this.GetComponent<Image> ();
		if (manager == null) {
			manager = FindObjectOfType<Manager1_4A> ();
            
        }

		if (parent == null) {
			parent = this.transform.parent;
		}

		if (circleCollider == null) {
			circleCollider = this.GetComponent<CircleCollider2D> ();
		}

    }


	public void BonusItemFix(){
		if(isBonusItem && bonusItemParticle.activeInHierarchy == false){
			bonusItemParticle.SetActive(true);
			//StartCoroutine (TimeitemBonus());

		} else if (!isBonusItem && bonusItemParticle.activeInHierarchy == true){
			bonusItemParticle.SetActive(false);
		}
	}

	public void startDragging(){
		if (isBeenDrag) return;
		RaycastHit2D hit = Physics2D.Raycast (this.transform.position, manager.directionOfRayRight.normalized, Mathf.Infinity, manager.layerItemHandler);
		if (hit.collider != null) {
			//Debug.DrawRay (transform.position, manager.directionOfRayRight, manager.debugDrawRayColorRight);
			Debug.DrawLine (this.transform.position, hit.point, manager.debugDrawRayColorRight);
			ItemGroup1_4A item = hit.collider.gameObject.GetComponent<ItemGroup1_4A> ();
			if (item != null && hit.collider.transform.IsChildOf (this.parent) == false) {
				hasObjectOnRight = true;
				rightObject = hit.collider.transform;
				//Debug.Log (hit.collider.name);
			} else {
				hasObjectOnRight = false;
				rightObject = null;
			}
		} else {
			hasObjectOnRight = false;
			rightObject = null;
		}

		RaycastHit2D hitLeft = Physics2D.Raycast (this.transform.position, manager.directionOfRayLeft.normalized, Mathf.Infinity, manager.layerItemHandler);
		if (hitLeft.collider != null) {
			//Debug.DrawRay (transform.position, manager.directionOfRayLeft, manager.debugDrawRayColorLeft);
			Debug.DrawLine (this.transform.position, hitLeft.collider.transform.position, manager.debugDrawRayColorLeft);
			ItemGroup1_4A item = hitLeft.collider.gameObject.GetComponent<ItemGroup1_4A> ();
			if (item != null && hitLeft.collider.transform.IsChildOf (this.parent) == false) {
				hasObjectOnLeft = true;
				leftObject = hitLeft.collider.transform;
				//Debug.Log (hitLeft.collider.name);
			} else {
				hasObjectOnLeft = false;
				leftObject = null;
			}
		} else {
			hasObjectOnLeft = false;
			leftObject = null;
		}
	}
	

	public void toggleCollider(bool enable){
		if (circleCollider == null) {
			circleCollider = this.GetComponent<CircleCollider2D> ();
		}
		circleCollider.enabled = enable;
	}

	public void toggleBonusItem(bool _enable){
		isBonusItem = _enable;
		BonusItemFix();
	}

	IEnumerator timeExplo(){
		int numbRange = Random.Range(0,50);
		if(numbRange==9){
			float tmp = Random.Range(.1f,1f);
			int corN = Random.Range(0,corLuz.Length);
			partExplo.GetComponent<ParticleSystem>().startColor = corLuz[corN];
			//yield return new WaitForSeconds (tmp);
			yield return Yielders.Get(tmp);
			checkExplo=!checkExplo;				
			this.partExplo.SetActive(checkExplo);
		}
			
	}



	public void UpdateImage(){
		if (itemInfo == null) return;
		this.GetComponent<Image> ().sprite = itemInfo.itemImage;
		this.GetComponent<Canvas>().overrideSorting = true;
	}

	public void UpdateImage(float Alpha){
		if (itemInfo == null) return;
		this.GetComponent<Image> ().sprite = itemInfo.itemImage;
		this.GetComponent<Image> ().color = new Vector4 (1f, 1f, 1f, Alpha);
		this.GetComponent<Canvas>().overrideSorting = true;
	}

	public void UpdateImage(Color _color){
		if (itemInfo == null) return;
		this.GetComponent<Image> ().sprite = itemInfo.itemImage;
		this.GetComponent<Image> ().color = _color;
		this.GetComponent<Canvas>().overrideSorting = true;
	}

	public void UpdateImageLock(Color _color){
			this.lockImageComp.color = _color;
	}

	public void UpdateColor(Color _color){
		this.GetComponent<Image> ().color = _color;
	}

	public Color GetColor(){
		return this.GetComponent<Image> ().color;
	}

	public Color GetColor(float alpha){
		return new Vector4 (1f, 1f, 1f, alpha);
	}


	public void UpdateItemHandler(ItemHandler1_4A itemHandler){
		thisItemHandler = itemHandler;
	}

	public void toggleLock(bool _enable){
		

        if (_enable == true)
        {
            lockImageComp.gameObject.SetActive(_enable);
            lockImageComp.DOFade(1f,timeToShowLock);
        } else
        {
            Sequence hiding = DOTween.Sequence();
            hiding.Append(lockImageComp.DOFade(0f,timeToShowLock));
            hiding.AppendCallback(() => lockImageComp.gameObject.SetActive(_enable));
            hiding.Play();

        }

    }

	public void EnableBackgroundImage(){
		backgroundImageComp.color = manager.gridColor;
	}

	public void DisableBackgroundImage(){
		backgroundImageComp.color = Color.clear;
		
	}

	
}
