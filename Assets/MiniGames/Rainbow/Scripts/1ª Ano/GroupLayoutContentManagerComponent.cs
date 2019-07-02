using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace MiniGames.Rainbow.Scripts._1ª_Ano
{
    public class GroupLayoutContentManagerComponent : MonoBehaviour
    {
        public ReactiveProperty<RainbowItem> itemSprite = new ReactiveProperty<RainbowItem>();
        public ReactiveProperty<int> itemAmount = new ReactiveProperty<int>();

        public List<Image> instancedSprites;

        [AssetsOnly]
        public GameObject prefabItem;

        [ReadOnly] public Image imageComponent;
        private bool _setupDone = false;
        [ReadOnly] public Sequence updatedItensSequence;

        public void Start()
        {
            if (imageComponent == null)
            {
                imageComponent = GetComponent(typeof(Image)) as Image;
            }

        }

        public void SetupItemGroup()
        {
            if(_setupDone) return;
            if (imageComponent == null)
            {
                imageComponent = GetComponent(typeof(Image)) as Image;
            }
            itemSprite.Subscribe(UpdateImageGroup);
            itemAmount.Subscribe(UpdateItemGroup);
            _setupDone = true;
        }

        public void UpdateItemGroup(int amountItem)
        {
            updatedItensSequence = DOTween.Sequence();

            for (var index = 0; index < instancedSprites.Count; index++)
            {
                var instancedSprite = instancedSprites[index];
                if (index == 0)
                {
                    updatedItensSequence.Append(instancedSprite.DOColor(Color.clear, .3f));
                }

                updatedItensSequence.Join(instancedSprite.DOColor(Color.clear, .3f));
            }


            if (instancedSprites.Count < amountItem)
            {
                var needed = amountItem - instancedSprites.Count;

                for (int i = 0; i < needed; i++)
                {
                    var instanced = Instantiate(prefabItem, this.transform) as GameObject;
                    var instancedImage = instanced.GetComponent<Image>();
                    instancedSprites.Add(instancedImage);
                    instancedImage.sprite = itemSprite.Value.sprite;
                    instancedImage.color = Color.clear;
                    instanced.SetActive(false);
                }
            }

            for (var index = 0; index < amountItem; index++)
            {
                var instancedSprite = instancedSprites[index];
                if (index == 0)
                {
                    updatedItensSequence.Append(instancedSprite.DOColor(Color.white, .3f));
                }
                updatedItensSequence.Join(instancedSprite.DOColor(Color.white, .3f));
                instancedSprite.gameObject.SetActive(true);
            }

            if (instancedSprites.Count - amountItem >= 1)
            {
                for (int i = amountItem; i < instancedSprites.Count; i++)
                {
                    instancedSprites[i].gameObject.SetActive(false);
                }
            }


            updatedItensSequence.Play();
        }

        public void UpdateImageGroup(RainbowItem item)
        {
            foreach (var spriteItem in instancedSprites)
            {
                spriteItem.sprite = item.sprite;
            }
        }

    }
}
