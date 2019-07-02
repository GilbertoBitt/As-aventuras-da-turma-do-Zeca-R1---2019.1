﻿using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UniRx;
using UnityEngine;

namespace Controllers
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class ScoreController : MonoBehaviour
    {
        public IntReactiveProperty amountValue = new IntReactiveProperty(0);
        private int _lastValue = 0;
        [ReadOnly] public TextMeshProUGUI scoreTextComponent;
        [Range(0.1f, 2f)]
        public float animationDuration;

        private void Awake()
        {
            scoreTextComponent = GetComponent(typeof(TextMeshProUGUI)) as TextMeshProUGUI;
        }

        private void Start()
        {
            amountValue.Subscribe(updateValue =>
            {
                scoreTextComponent.DOTextInt(_lastValue, updateValue, animationDuration)
                    .OnComplete(() => { _lastValue = updateValue; });
            });
        }

        public void ResetScore()
        {
            amountValue = new IntReactiveProperty(0);
        }

    }
}