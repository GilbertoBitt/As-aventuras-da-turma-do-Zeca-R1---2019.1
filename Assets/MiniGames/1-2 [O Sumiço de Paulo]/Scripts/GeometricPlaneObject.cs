using System;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MiniGames.Scripts
{
    [CreateAssetMenu(menuName = "AVTZ/SMDP/Figuras Geométricas Planas")]
    public class GeometricPlaneObject : ScriptableObject
    {
        [ReadOnly]
        public string formName;
        [AssetList(Path = "MiniGames/1-2 [O Sumiço de Paulo]/sprites/GeometricFigures SVG")]
        [InlineEditor(InlineEditorModes.LargePreview)]
        public Sprite objectImage;
        public Color ItemColor;
        public PlaneFigures[] categories;
        public int numberOfSides;
        public int numberOfVertices;
        public bool allSidesSameSizes;
        public bool allSidesDiferentSizes;

        public bool HasCategory(PlaneFigures figures) => categories.Any(plane => plane == figures);

        public string GetSpacialFormName(PlaneFigures form, bool isSingular = false)
        {
            switch (form)
            {
                case PlaneFigures.None:
                    throw new ArgumentOutOfRangeException(nameof(form), form, null);
                case PlaneFigures.Square:
                    return isSingular ? "Quadrado" : "Quadrados";
                case PlaneFigures.Rectangle:
                    return isSingular ? "Retângulo" : "Retângulos";
                case PlaneFigures.Triangle:
                    return isSingular ? "Triângulo" : "Triângulos";
                case PlaneFigures.Trapeze:
                    return isSingular ? "Trapézio" : "Trapézios";
                case PlaneFigures.Parallelogram:
                    return isSingular ? "Paralelogramo" : "Paralelogramos";
                default:
                    throw new ArgumentOutOfRangeException(nameof(form), form, null);
            }
        }

        private void OnValidate()
        {
            formName = this.name;
        }
    }
}
