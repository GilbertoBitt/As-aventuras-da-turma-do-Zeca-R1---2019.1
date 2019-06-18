using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class GridLayoutDinamicChildComponent : MonoBehaviour
{
    [ReadOnly] public RectTransform rectTransform;
    [ReadOnly] public GridLayoutGroup gridLayoutGroup;

    // Start is called before the first frame update
    void Calculate()
    {
        if (rectTransform == null)
        {
            rectTransform = GetComponent<RectTransform>();
        }

        if (gridLayoutGroup == null)
        {
            gridLayoutGroup = GetComponent<GridLayoutGroup>();
        }


        var width = rectTransform.rect.width - gridLayoutGroup.spacing.x - gridLayoutGroup.padding.left - gridLayoutGroup.padding.right;
        Vector2 newSize = new Vector2(width / 4, width / 4);
        gridLayoutGroup.cellSize = newSize;
    }

    private void Start()
    {
        Calculate();
    }

    private void OnValidate()
    {
        Calculate();
    }
}
