﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Linq;

public class ItemHandler1_4A : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IInitializePotentialDragHandler {

//	[SeparatorAttribute("Referencias")]
	public Manager1_4A gameManager;
	public List<GameObject> itemsOnSlot = new List<GameObject> ();
	//[SeparatorAttribute("Variaveis de Controle")]
	public int slotCount;
	private bool isBeenDrag = false;
	public bool hasEnded = false;
	public int maxBadItems = 0;
	public int amountBadItems = 0;
	public List<ItemHandler1_4A> handlersOfTheFloor = new List<ItemHandler1_4A> ();
	public bool isRed = false;
	[HideInInspector]
	public GameObject itemToDrag;
	private Vector3 startPosition;
	private Vector3 startScale;
	[HideInInspector] 
	public List<ItemGroup1_4A> itensGroup = new List<ItemGroup1_4A>();
//	[SeparatorAttribute("Configuração para Reconstrução")]
	public List<ItemHistory1_4A> originalItems = new List<ItemHistory1_4A>();
	public List<ItemHandler1_4A> handlersOfTheFloorHistory = new List<ItemHandler1_4A> ();
	public int startMaxBadItems = 0;
	public Floor1_4A floorParent;
	public int sortingOrder;	
	private int sortingOrderStart;
    public bool blockDrag = false;

    private Camera _camera;
	// Use this for initialization
	public void Start () {
		//itemsOnSlot.Reverse ();
		slotCount = itemsOnSlot.Count;
		_camera = Camera.main;
	}
	
	// Update is called once per frame
	/*void Update () {
		//BugFixRed a Nonred
		if (itemToDrag.GetComponent<Image> ().color == Color.red && isRed == false) {
			removeRedItem ();
		}
	
	}*/

	public void removeRedItem(){
		itemToDrag.GetComponent<ItemGroup1_4A> ().UpdateImage (Color.white);
		itemToDrag.GetComponent<ItemGroup1_4A> ().toggleLock(false);
	}

	public void removeAddItem(bool _enable){
		itemToDrag.GetComponent<ItemGroup1_4A> ().toggleLock(_enable);
	}

	#region DragAndDrop
	private ItemGroup1_4A _privateItem;


	public void OnBeginDrag (PointerEventData eventData){
		int temp = slotCount - 1;
		if (blockDrag != false || gameManager.isDragging != false || isBeenDrag != false || gameManager.isRemoving ||
		    isRed != false) return;
		UpdateDragItem ();
		gameManager.checkItensPos();
		itemToDrag.GetComponent<CanvasGroup> ().blocksRaycasts = false;
		startPosition = itemToDrag.transform.position;
		startScale = itemToDrag.transform.localScale;

		gameManager.isDragging = true;
		isBeenDrag = true;

		itemToDrag.transform.SetParent (gameManager.dragParent);
		itemToDrag.transform.localScale = startScale;
		_privateItem = itemToDrag.GetComponent<ItemGroup1_4A>();
		_privateItem.isBeenDrag = true;
		_privateItem.DisableBackgroundImage();
		sortingOrderStart = itemToDrag.GetComponent<Canvas>().sortingOrder;
		itemToDrag.GetComponent<Canvas>().sortingOrder = 7;
	}
		
	public void OnDrag (PointerEventData eventData){
		if (gameManager.isDragging != true || isBeenDrag != true || gameManager.isRemoving || isRed != false) return;
		/*float distance = this.transform.position.z - Camera.main.transform.position.z;
			Vector3 pos = new Vector3(Input.mousePosition.x,Input.mousePosition.y,distance);*/
		//itemToDrag.transform.position = Camera.main.ScreenToWorldPoint(pos);
		Vector3 pos = Input.mousePosition;
		pos.z = 100f;
		if (Camera.main != null) itemToDrag.transform.position = _camera.ScreenToWorldPoint(pos);
		itemToDrag.GetComponent<ItemGroup1_4A> ().isBeenDrag = true;
	}

	

	public void OnEndDrag (PointerEventData eventData){
		if (itemToDrag == null) return;
		itemToDrag.GetComponent<CanvasGroup> ().blocksRaycasts = true;
		itemToDrag.transform.SetParent (this.transform);
		itemToDrag.transform.position = startPosition;
		itemToDrag.transform.localScale = startScale;
		isBeenDrag = false;
		gameManager.isDragging = false;
		_privateItem.isBeenDrag = false;
		_privateItem.EnableBackgroundImage();
		itemToDrag.GetComponent<Canvas>().sortingOrder = sortingOrderStart;
	}

	public void EndDrag() {
		if (itemToDrag == null) return;
		itemToDrag.GetComponent<CanvasGroup> ().blocksRaycasts = true;
		itemToDrag.transform.SetParent (this.transform);
		itemToDrag.transform.position = startPosition;
		itemToDrag.transform.localScale = startScale;
		isBeenDrag = false;
		gameManager.isDragging = false;
		_privateItem.isBeenDrag = false;
		_privateItem.EnableBackgroundImage();
		itemToDrag.GetComponent<Canvas>().sortingOrder = sortingOrderStart;
	}

	private void LateUpdate() {
//		if(gameManager.isDragging) return;
//		if(Input.GetMouseButton(0)) return;
//		if (itemToDrag == null) return;
//		if (_privateItem == null || _privateItem.isBeenDrag || !gameManager.isPlaying) return;
//		EndDrag();
	}

	#endregion

	public void AddHandler(ItemHandler1_4A _Handler){
		handlersOfTheFloor.Add (_Handler);
	}

	public void RemoveHandler(ItemHandler1_4A _Handler){
		handlersOfTheFloor.Remove (_Handler);
	}

	/// <summary>
	/// Updates the item information and Update his sprite icon/Image.
	/// </summary>
	/// <param name="id">Identifier.</param>
	/// <param name="spriteImage">Sprite image.</param>
	public void UpdateItemInfo(int id,Item1_4A itemInfo){
		if (id > itemsOnSlot.Count) return;
		itemsOnSlot [id].GetComponent<ItemGroup1_4A> ().itemInfo = itemInfo;
		itemsOnSlot [id].GetComponent<ItemGroup1_4A> ().UpdateImage ();
	}

	public void UpdateItemInfo(int id,Item1_4A itemInfo, float alphaImage){
		if (id > itemsOnSlot.Count) return;
		itemsOnSlot [id].GetComponent<ItemGroup1_4A> ().itemInfo = itemInfo;
		itemsOnSlot [id].GetComponent<ItemGroup1_4A> ().UpdateImage (alphaImage);
	}

	public void ActiveCollider2D(){
		if (itemToDrag != null) {
			itemToDrag.GetComponent<ItemGroup1_4A> ().toggleCollider (true);
		}
	}

	public void deactiveCollider2D(){
		if (itemToDrag != null) {
			itemToDrag.GetComponent<ItemGroup1_4A> ().toggleCollider (false);
		}
	}

	public void removeItemInFront(bool _isRight){
		int temp = slotCount - 1;
		if (!(itemsOnSlot[temp] != null)) return;
		GameObject item = itemsOnSlot[temp];
        itemsOnSlot[temp].GetComponent<ItemGroup1_4A>().DisableBackgroundImage();
        itemsOnSlot.RemoveAt(temp);
        if (_isRight) {
	        item.SetActive(false);
        } else {
	        gameManager.ThrowItemAway(item.transform);
        }
        slotCount--;
	}

	public void UpdateDragItem(){
		if (itemsOnSlot.Count <= 0) return;
		slotCount = itemsOnSlot.Count;
		var invertedList = itemsOnSlot;
		invertedList.Reverse();
		itemToDrag = invertedList [slotCount-1];
		itemsOnSlot.ForEach(item =>
		{
			var itemGroup = item.GetComponent(typeof(CanvasGroup)) as CanvasGroup;
			if (itemGroup == null) return;
			itemGroup.interactable = item == itemToDrag;
			itemGroup.blocksRaycasts = item == itemToDrag;
		});
		//blockDrag = false;
	}

	public void UpdateStartList()
	{
		foreach (var t in itemsOnSlot)
		{
			ItemHistory1_4A item = new ItemHistory1_4A {itemComp = t, sibblingIndex = t.transform.GetSiblingIndex()};
			originalItems.Add (item);
		}
	}

	public void disableAllColliders2D()
	{
		foreach (var t in itemsOnSlot)
		{
			t.GetComponent<ItemGroup1_4A> ().toggleCollider (false);
		}
	}

	public void disableAllLocks(){
		for (int i = 0; i < itemsOnSlot.Count; i++) {
			itemsOnSlot[i].GetComponent<ItemGroup1_4A>().toggleLock(false);
		}
	}

	public void ResetItemsPos(){
		itemsOnSlot.Clear ();
		foreach (var t in originalItems)
		{
			itemsOnSlot.Add (t.itemComp);
			t.itemComp.transform.SetParent (this.transform);
			t.itemComp.transform.SetSiblingIndex (t.sibblingIndex);
			t.itemComp.SetActive (true);
		}
        blockDrag = false;
		slotCount = itemsOnSlot.Count;
		handlersOfTheFloor = handlersOfTheFloorHistory;
		isRed = false;
		amountBadItems = 0;
		maxBadItems = startMaxBadItems;
		hasEnded = false;
		UpdateDragItem ();
		disableAllLocks();
		disableAllColliders2D ();
		ActiveCollider2D ();
		removeRedItem ();
	}

	public void StartMaxBadItemsUpdate(){
		startMaxBadItems = maxBadItems;
	}

	public void UpdateItemHandlerOnChilds()
	{
		foreach (var t in itemsOnSlot)
		{
			t.GetComponent<ItemGroup1_4A> ().UpdateItemHandler (this);
		}
	}

	public void ResetBonusItems()
	{
		foreach (var t in itemsOnSlot)
		{
			t.GetComponent<ItemGroup1_4A>().toggleBonusItem(false);
		}
	}

	public void UpdateItemHandlersHistory(){
		//handlersOfTheFloorHistory = handlersOfTheFloor;
		handlersOfTheFloorHistory = handlersOfTheFloor.ToList();
		/*for (int i = 0; i < handlersOfTheFloor.Count; i++) {
			ItemHandler1_4A _item = new ItemHandler1_4A ();
			_item = handlersOfTheFloor [i];
			handlersOfTheFloor.Add (_item);
		}*/
	}

	public void UpdateBadItemsLimit(){
		maxBadItems = returnMaxBadItems ();
	}

	public int returnMaxBadItems(){
		return handlersOfTheFloor.Count;	
	}

	public void updateOtherHandlers()
	{
		foreach (var t in handlersOfTheFloorHistory)
		{
			t.amountBadItems = amountBadItems;
			t.maxBadItems = maxBadItems;
		}
	}

	public void RemoveThisHandlerFromAll(ItemHandler1_4A _handler)
	{
		foreach (var t in this.handlersOfTheFloor)
		{
			t.removeIfExist (_handler);
		}
	}

	public void removeIfExist(ItemHandler1_4A _handler){
		if (this.handlersOfTheFloor.Contains (_handler)) {
			this.handlersOfTheFloor.Remove (_handler);
		}
	}

	public ItemGroup1_4A ItemBonusHere(){
		if(itemToDrag != null){
			itemToDrag.GetComponent<ItemGroup1_4A>().isBonusItem = true;
			//itemToDrag.GetComponent<ItemGroup1_4A>().UpdateColor(Color.blue);
			return itemToDrag.GetComponent<ItemGroup1_4A>();
		} else {
			//Debug.Log("ERRO");
			return null;
		}
	}

	public void checkDragItem(){
		itemToDrag.GetComponent<ItemGroup1_4A>().startDragging();
	}

	public void redItem(){
		StartCoroutine(CheckIfRedsGone());
	}

	public IEnumerator CheckIfRedsGone(){
		yield return new WaitUntil (() => handlersOfTheFloor.All (x => x.hasEnded == true));
		isRed = false;
		removeRedItem ();
		yield return Yielders.Get(1f);
		removeRedItem ();
	}


	public void OnInitializePotentialDrag(PointerEventData eventData)
	{
		UpdateDragItem();
	}
}

[System.Serializable]
public class ItemHistory1_4A {

	public GameObject itemComp;
	public int sibblingIndex;
	
}
