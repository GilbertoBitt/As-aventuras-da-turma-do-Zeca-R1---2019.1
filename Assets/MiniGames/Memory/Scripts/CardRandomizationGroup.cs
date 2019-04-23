using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using com.csutil;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

namespace MiniGames.Memory.Scripts
{
    public class CardRandomizationGroup : ScriptableObject
    {
        public List<CardItem> GroupItemList = new List<CardItem>();
        
        [FoldoutGroup("Tools 2")]
        [TextArea]
        public string wordsText;

        [FoldoutGroup("Tools 2")]
        [Button("Generate Assets")]
        public void GenerateCoroutine()
        {
            var separator = ",";
            
            var wordsResult = wordsText.Split(separator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            
#if UNITY_EDITOR
            string path = AssetDatabase.GetAssetPath (Selection.activeObject);
            path = path.Replace (Path.GetFileName (path), "");
            for (var index = wordsResult.Length - 1; index >= 0; index--)
            {
                var word = wordsResult[index];
                if (word.IsNullOrWhitespace() || word.IsNullOrEmpty()) return;
                var asset = CreateInstance<CardItem>();
                var filepath = Path.GetInvalidFileNameChars()
                    .Aggregate(word, (current, c) => current.Replace(c, ' '));
                string assetPathAndName = $"{path}Palavras 2 ano/{word}.asset";
                asset.NameItem = word;
                AssetDatabase.CreateAsset(asset, assetPathAndName);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                if (asset != null)
                {
                    GroupItemList.Add(asset); 
                }
            }

            EditorUtility.FocusProjectWindow ();
            #endif
        }

        public List<CardItem> SortCardGroup(int level)
        {
            var spritesChoosen = new List<CardItem>();
            spritesChoosen.Clear();
            switch (level)
            {
                case 0:
                    for (var i = 0; i < 5; i++)
                    {
                        spritesChoosen.Add(GroupItemList[i]);
                    }

                    break;
                case 1:
                    for (var i = 5; i < 10; i++)
                    {
                        spritesChoosen.Add(GroupItemList[i]);
                    }

                    break;
                case 2:
                    for (var i = 0; i < 11; i++)
                    {
                        if (i % 2 == 1)
                        {
                            spritesChoosen.Add(GroupItemList[i]);
                        }
                    }

                    break;
                default:
                    break;
            }

            return spritesChoosen;

        }

        private void OnValidate()
        {
            int tempCount = GroupItemList.Count;
            for (int i = 0; i < tempCount; i++)
            {
                GroupItemList[i].idItem = i;
            }
        }
    }
}