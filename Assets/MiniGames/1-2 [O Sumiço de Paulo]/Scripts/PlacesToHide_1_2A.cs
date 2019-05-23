using System.Collections;
using System.Collections.Generic;
using com.csutil;
using MEC;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlacesToHide_1_2A : MonoBehaviour, IPointerClickHandler {

	public Sprite Image;
	public Image imageComponent;
	public Manager_1_2A manager;
	public bool isDone = false;
	public Image imagePanel;
	public GameObject touchPrefabIcon;
	public Transform touchParent;
	public List<GameObject> touchesInstance = new List<GameObject>();
	public float smoothMovementOffset = 1f;

	// Use this for initialization
	void Start () {

		if (imageComponent == null) {
			//imageComponent = this.GetComponent<Image> ();
		}

		//imageComponent.preserveAspect = true;
		
	}
	
	// Update is called once per frame

	public void UpdateImage(Sprite hideImage){
		Image = hideImage;
		ImageComponentUpdate (hideImage);
	}
		
	public void ImageComponentUpdate(Sprite hideImage){
		imageComponent.sprite = hideImage;
		//imageComponent.preserveAspect = true;
	}

	public void OnPointerClick(PointerEventData eventData){
		Log.d($"Event Clicked { eventData.pointerPress }");
		if(eventData.clickCount <= 1 && manager.isChoosingFound == false && manager.isGameRunning && manager.pauseManager.isOnPause == false){
			if (Image == manager.pauloSprite) {
				if (manager.foundPaulo) {
					clicked ();
					//manager.soundManager.startSoundFX(manager.sfx[0]);
				} else {
					manager.NothingFinded ();
					//Debug.Log ("Achou nada!");
					manager.actualCombo = 1;
					TouchHandler();
				}
			} else {
				clicked ();
			}
		}


	}

	public void clicked(){
		if (isDone) return;
		if (Image != null) {
			//Debug.Log (Image.name);
			//manager.amountsFinded++;
			if(manager.foundPaulo){
				manager.FoundedPaulo (Image, calculatePosition(),this.GetComponent<RectTransform>());
			} else {
				manager.Founded (Image, calculatePosition(),this.GetComponent<RectTransform>());
			}
			imagePanel.material = null;
			imageComponent.color = Color.clear;
			isDone = true;
		} else {
			manager.NothingFinded ();
			//Debug.Log ("Achou nada!");
			manager.actualCombo = 1;
			TouchHandler();
		}
	}

	public Vector3 calculatePosition(){
	
		return this.transform.position;;
	}

	public void TouchHandler(){
		if(Input.touchCount > 0){
			GameObject touchIcon = Instantiate(touchPrefabIcon,touchParent) as GameObject;
			touchIcon.transform.localScale = new Vector3(1f,1f,1f);
			Vector3 screenPoint = Input.GetTouch(0).position;
			screenPoint.z = 10.0f; //distance of the plane from the camera
			touchIcon.transform.position = Camera.main.ScreenToWorldPoint(screenPoint);
			Timing.RunCoroutine(HideItem(touchIcon), Segment.SlowUpdate);
		}

	}

	public IEnumerator<float> HideItem(GameObject itemToHide){
		yield return Timing.WaitForSeconds(manager.errorTextEffectDuration);
		itemToHide.SetActive(false);
	}



}
