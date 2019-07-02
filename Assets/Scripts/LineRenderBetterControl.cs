using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI.Extensions;

[RequireComponent(typeof(UILineRenderer))]
public class LineRenderBetterControl : MonoBehaviour
{
    public int startTracingPosition;
    public int amountOfLines;
    public int LineSpacing;
    public int LineSize;

    public List<Vector2> points;

    [Button("Update List")]
    public void UpdateLineTracer()
    {
        var lineRender = GetComponent(typeof(UILineRenderer)) as UILineRenderer;
        points.Clear();
        for (int i = 1; i < amountOfLines+1; i++)
        {
            points.Add(new Vector2(startTracingPosition + (LineSpacing * i) + (LineSize * i), 0f));
        }

        if (lineRender != null) lineRender.Points = points.ToArray();
    }
}
