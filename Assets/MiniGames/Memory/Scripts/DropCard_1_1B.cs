using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Pipes;
using MiniGames.Memory.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using MEC;
using TMPro;

public class DropCard_1_1B : MonoBehaviour, IDropHandler, IPointerClickHandler {

	public CardItem characterSprite;
	public Manager_1_1B manager;
	public DragCard_1_1B cardDraged;
	public Vector3 positionOffset;

	public SoundManager sound;
	public List<AudioClip> clips = new List<AudioClip>();
	private Image thisImageComp;
	public Outline thisoutline;
	public TextMeshProUGUI cardItemName;
	private AudioSource _voicePlayer;
	private static readonly int PosCorpoZeca = Animator.StringToHash("posCorpoZeca");
	// Use this for initialization
	void Start () {

		if(manager == null){
			manager = FindObjectOfType<Manager_1_1B>();
		}

		sound = manager.cardManager.sound;
		thisoutline = this.GetComponent<Outline>();

		thisImageComp = this.GetComponent<Image>();
//		clips = manager.cardManager.clips;
				
	}
	
	// Update is called once per frame
	
	
    public void Rescale1() {
     //   this.transform.localScale = Vector3.one;
        Debug.Log("Reset scale " + this.name, this);
    }

	public void OnDrop(PointerEventData data)
	{
		if (!manager.isPlayTime) return;
		if (data.pointerDrag == null) return;
		var cardDrag = data.pointerDrag.GetComponent<DragCard_1_1B>();
		if (cardDrag == null) return;
		if(cardDraged == null){
			//Debug.Log ("Dropped object was: "  + data.pointerDrag);
			cardDraged = cardDrag;
			cardDrag.hasDroped = true;
			cardDrag.dropedCard = this;
			cardDrag.gameObject.transform.SetParent(this.transform);
			cardDrag.gameObject.transform.localScale = new Vector3(1f,1f,1f);
			cardDrag.gameObject.transform.localPosition = positionOffset;
			sound.startSoundFX(manager.clips[12]);
			manager.verifyCards();				
		} else {
			manager.groupLayout.enabled = false;
			cardDrag.Clear();
			cardDrag.transform.SetParent(cardDrag.OriginalParent);
			cardDrag.transform.localScale = new Vector3(1f,1f,1f);
			manager.groupLayout.enabled = true;
		}
	}

	public void UpdateSprite(CardItem cardItem)
	{
		characterSprite = cardItem;
		if (thisImageComp == null) {
			thisImageComp = this.GetComponent<Image>();
		}
		thisImageComp.sprite = characterSprite.SpriteItem;
		Timing.RunCoroutine(TimeCards());
	}

	public void UpdateName(CardItem cardItem)
	{
		characterSprite = cardItem;
		cardItemName.SetText(characterSprite.NameItem);
	}
	
	public void UpdateName(CardItemAlternative cardItem, int anoLetivo)
	{
		characterSprite = cardItem;
		cardItemName.SetText(anoLetivo == 3 ? cardItem.classification.ToString() : cardItem.NameItem);
	}
	
	
	IEnumerator<float> TimeCards(){

		manager.zecaCard.ControlAnimCorpo.SetInteger (PosCorpoZeca,5);
		yield return Timing.WaitForSeconds(3f);
		manager.zecaCard.ControlAnimCorpo.SetInteger (PosCorpoZeca,5);
	}

	public void Clear(){
		cardDraged = null;
	}

	public void updateRightText(int anoLetivo){
		cardDraged.RightOneTextComponent.SetText(anoLetivo == 2
			? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(characterSprite.NameItem.ToLower())
			: characterSprite.NameItem.ToUpper(CultureInfo.CurrentCulture));
	}
	public void OnPointerClick(PointerEventData eventData) {
		if (_voicePlayer == null || (_voicePlayer != null && !_voicePlayer.isPlaying)) {
			_voicePlayer = sound.startVoiceFXReturn(characterSprite.AudioClipItem);
		}
	}

	/*public AudioClip findAudio(string spriteName){
		switch (spriteName)
		{	
			case "Zeca":
			return manager.clips[10];
			break;
			case "João":
			return manager.clips[4];
			break;
			case "Paulo":
			return manager.clips[7];
			break;
			case "Ana":
			return manager.clips[0];
			break;
			case "Manu":
			return manager.clips[5];
			break;
			case "Tati":
			return manager.clips[8];
			break;
			case "Bia":
			return manager.clips[1];
			break;
			case "Tobias":
			return manager.clips[9];
			break;
			case "José":
			return manager.clips[3];
			break;
			case "Carla":
			return manager.clips[2];
			break;
			case "Juca":
			return manager.clips[11];
			break;
			default:
			return new AudioClip();
			break;
		}
	}*/

}

