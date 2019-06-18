using System;
using System.Collections.Generic;
using DG.Tweening;
using MiniGames.Rainbow.Scripts._1ª_Ano;
using Sirenix.OdinInspector;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace MiniGames.Rainbow.Scripts
{
    public class MultiplyInAddition : MonoBehaviour
    {
        public GameObject textPrefab;
        [ReadOnly]
        public EquationContent equationContent;
        [ReadOnly]
        public List<TextMeshProUGUI> textInstances = new List<TextMeshProUGUI>();

        [ReadOnly] public Sequence updatedItensSequence;



        public void OnEquationChanged(EquationContent equation, TextMeshProUGUI[] textEffects, Action onTransitionEffect)
        {

            equationContent = equation;

            updatedItensSequence = DOTween.Sequence();

            for (var index = 0; index < textInstances.Count; index++)
            {
                var textInstance = textInstances[index];
                if (index == 0)
                {
                    updatedItensSequence.Append(textInstance.DOColor(Color.clear, .3f));
                }

                updatedItensSequence.Join(textInstance.DOColor(Color.clear, .3f));
            }

            foreach (var textEffect in textEffects)
            {
                updatedItensSequence.Join(textEffect.DOColor(Color.clear, .3f));
            }

            var textsNeeded = (equation.firstValue * 2) - 1;

            if (textInstances.Count < textsNeeded)
            {
                var needed = textsNeeded - textInstances.Count;

                for (int i = 0; i < needed; i++)
                {
                    var textInstance = InstanceTextPrefab();
                    textInstances.Add(textInstance);
                    textInstance.color = Color.clear;
                }
            }

            onTransitionEffect.Invoke();

            for (var index = 0; index < textsNeeded; index++)
            {
                var textInstance = textInstances[index];
                textInstance.text = IsEven(index) ? equation.secondValue.ToString() : "+";
                if (index == 0)
                {
                    updatedItensSequence.Append(textInstance.DOColor(Color.white, .3f));
                }

                updatedItensSequence.Join(textInstance.DOColor(Color.white, .3f));
            }

            foreach (var textEffect in textEffects)
            {
                updatedItensSequence.Join(textEffect.DOColor(Color.white, .3f));
            }

            updatedItensSequence.Play();
        }

        public static bool IsOdd(int value)
        {
            return value % 2 != 0;
        }

        public static bool IsEven(int value)
        {
            return value % 2 == 0;
        }

        private TextMeshProUGUI InstanceTextPrefab()
        {
            var instanceGameObject = Instantiate(textPrefab, transform);
            var textMeshProInstance = instanceGameObject.GetComponent(typeof(TextMeshProUGUI)) as TextMeshProUGUI;
            return textMeshProInstance;
        }
    }
}
