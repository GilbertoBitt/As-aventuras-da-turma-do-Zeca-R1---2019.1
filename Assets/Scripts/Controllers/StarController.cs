using System;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
    public class StarController : MonoBehaviour
    {
        public IntReactiveProperty amountValue = new IntReactiveProperty(0);
        [Sirenix.OdinInspector.MinMaxSlider(0,3)]
        public Vector2Int rangeLimitValueStars;
        [FoldoutGroup("StarSprite")] [InlineEditor(InlineEditorModes.SmallPreview)] public Sprite starSprite;
        [FoldoutGroup("StarSprite")] [InlineEditor(InlineEditorModes.SmallPreview)] public Sprite emptyStarSpaceSprite;
        public Image[] starSpriteIconComponents;

        public void OnValidate()
        {
//            starSpriteIconComponents = GetComponentsInChildren(typeof(Image), true) as Image[];
        }

        private void Start()
        {
            ResetStarts();

            amountValue.Subscribe(updatedValue =>
            {
                if (updatedValue > rangeLimitValueStars.y)
                    updatedValue = rangeLimitValueStars.y;

                if (updatedValue < rangeLimitValueStars.x)
                    updatedValue = rangeLimitValueStars.x;

                UpdateStarsComponent(updatedValue);
            });
        }

        public void ResetStarts()
        {
            foreach (var spriteIconComponent in starSpriteIconComponents)
            {
                spriteIconComponent.sprite = emptyStarSpaceSprite;
            }
        }

        public void UpdateStarsComponent(int value)
        {
            for (int i = 0; i < value; i++)
            {
                starSpriteIconComponents[i].sprite = starSprite;
            }

            for (int i = (starSpriteIconComponents.Length - value) -1; i < starSpriteIconComponents.Length; i++)
            {
                starSpriteIconComponents[i].sprite = emptyStarSpaceSprite;
            }
        }
    }
}
