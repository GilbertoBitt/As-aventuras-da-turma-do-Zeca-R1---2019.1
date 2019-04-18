using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class AlbumCard : OverridableMonoBehaviour{

    public int id;
    public Image cardImageComponent;
    public Sprite defaultSprite;
    public Sprite defaultSpriteEmpty;
    public StoreItem storeItem;
    public string itemNameText;
    public TextMeshProUGUI itemNameTextComponent;

    /// <summary>
    /// Muda o status da carta.
    /// </summary>
    /// <param name="hasCard">
    /// true = o usuario ja tem a carta 
    /// false = o usuario não tem a carta.
    /// </param>
    public void SetCardStatus(bool hasCard)
    {
        itemNameText = storeItem != null ? storeItem.itemName : "";

        if (hasCard) {
            cardImageComponent.sprite = defaultSprite;            
            itemNameTextComponent.enabled = false;
        } else {
            cardImageComponent.sprite = defaultSpriteEmpty;
            itemNameTextComponent.text = itemNameText;
            itemNameTextComponent.enabled = true;
        }
    }

}
