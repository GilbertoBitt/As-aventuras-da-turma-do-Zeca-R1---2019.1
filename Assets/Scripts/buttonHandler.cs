using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using TMPro;

public class buttonHandler :  MonoBehaviour, IPointerUpHandler, IPointerDownHandler{

	private Vector2 startPoint;
	private Vector2 endPoint;
	private float delta;
	public gameSelection gSelection;
	public int minigameID;
    public TextMeshProUGUI text;

	public UnityEvent OnClick = new UnityEvent();

    public void Start() {
        text.ForceMeshUpdate();
        //text.SetAllDirty();
    }

    public void ButtonDown(){
		this.transform.localScale = new Vector3 (1f, 1f, 1f);
	}

	public void ButtonUp(){
		this.transform.localScale = new Vector3 (0.95f, 0.95f, 0.95f);
	}



	public void OnPointerUp(PointerEventData eventData){
		/*this.transform.localScale = new Vector3 (0.95f, 0.95f, 0.95f);
		endPoint = Input.mousePosition;
		delta = Vector2.Distance (startPoint, endPoint);
		OnClick.Invoke ();
				if (delta < 0.1f) {
			gSelection.buttonShowMinigame (minigameID);
			//Debug.Log ("Minigame Selectionado" + gSelection.gameConfigs.allMinigames[minigameID].name);
		}*/
	}
	public void OnPointerDown(PointerEventData eventData){
		/*startPoint = Input.mousePosition;
		this.transform.localScale = new Vector3 (1f, 1f, 1f);*/
		//gSelection.buttonShowMinigame (minigameID);
		//Debug.Log ("Minigame Selectionado" + gSelection.gameConfigs.allMinigames[minigameID].name);
	}


}
