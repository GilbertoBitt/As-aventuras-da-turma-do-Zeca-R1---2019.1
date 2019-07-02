using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UniRx.Toolkit;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace MiniGames.Scripts
{
    [RequireComponent(typeof(Button))]
    [System.Serializable]
    public class ButtonTMPComponent : SerializedMonoBehaviour
    {
        [ReadOnly]
        public TextMeshProUGUI textComponent;
        [ReadOnly]
        public Button buttonComponent;


        private void OnValidate()
        {
            buttonComponent = GetComponent(typeof(Button)) as Button;
            textComponent = GetComponentInChildren(typeof(TextMeshProUGUI), true) as TextMeshProUGUI;
        }

        public void Fade(float value = 0f, Action action = null)
        {
            buttonComponent.image.DOFade(value, 0.3f);
            if (action != null)
            {
                textComponent.DOFade(value, 0.3f).OnComplete(action.Invoke);
            }
            else
            {
                textComponent.DOFade(value, 0.3f);
            }
        }

    }

    public class ButtonTMPComponentPool : ObjectPool<ButtonTMPComponent>
    {
        readonly ButtonTMPComponent _prefab;
        readonly Transform _hierarchyParent;

        public ButtonTMPComponentPool(ButtonTMPComponent prefab, Transform hierarchyParent)
        {
            this._prefab = prefab;
            this._hierarchyParent = hierarchyParent;
        }

        protected override ButtonTMPComponent CreateInstance()
        {
            var buttonInstance = Object.Instantiate<ButtonTMPComponent>(_prefab, _hierarchyParent);
            return buttonInstance;
        }

        protected override void OnBeforeReturn(ButtonTMPComponent instance)
        {
            instance.Fade(action: () =>
            {
                instance.gameObject.SetActive(false);
            });

        }

        protected override void OnBeforeRent(ButtonTMPComponent instance)
        {
            base.OnBeforeRent(instance);
            instance.Fade(1f);
        }
// You can overload OnBeforeRent, OnBeforeReturn, OnClear for customize action.
        // In default, OnBeforeRent = SetActive(true), OnBeforeReturn = SetActive(false)

        // protected override void OnBeforeRent(Foobar instance)
        // protected override void OnBeforeReturn(Foobar instance)
        // protected override void OnClear(Foobar instance)
    }
}
