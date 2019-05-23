using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TextMeshProUGUI))]
public class OpenHyperlinks : MonoBehaviour, IPointerClickHandler
{

    private TextMeshProUGUI _text;
    private Camera _camera;
    private List<Color32[]> oldColors;

    private void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _camera = Camera.main;
    }

    public void OnPointerClick(PointerEventData eventData) {
        var linkIndex = TMP_TextUtilities.FindIntersectingLink(_text, Input.mousePosition, _camera);
        if (linkIndex == -1) return; // was a link clicked?
        var linkInfo = _text.textInfo.linkInfo[linkIndex];

        // open the link id as a url, which is the metadata we added in the text field
        Application.OpenURL(linkInfo.GetLinkID());
    }

    List<Color32[]> SetLinkToColor(int linkIndex, Color32 color) {
        TMP_LinkInfo linkInfo = _text.textInfo.linkInfo[linkIndex];

        var oldVertColors = new List<Color32[]>(); // store the old character colors

        for( int i = 0; i < linkInfo.linkTextLength; i++ ) { // for each character in the link string
            int characterIndex = linkInfo.linkTextfirstCharacterIndex + i; // the character index into the entire text
            var charInfo = _text.textInfo.characterInfo[characterIndex];
            int meshIndex = charInfo.materialReferenceIndex; // Get the index of the material / sub text object used by this character.
            int vertexIndex = charInfo.vertexIndex; // Get the index of the first vertex of this character.

            Color32[] vertexColors = _text.textInfo.meshInfo[meshIndex].colors32; // the colors for this character
            oldVertColors.Add(vertexColors.ToArray());

            if (!charInfo.isVisible) continue;
            vertexColors[vertexIndex + 0] = color;
            vertexColors[vertexIndex + 1] = color;
            vertexColors[vertexIndex + 2] = color;
            vertexColors[vertexIndex + 3] = color;
        }

        // Update Geometry
        _text.UpdateVertexData(TMP_VertexDataUpdateFlags.All);

        return oldVertColors;
    }
}
