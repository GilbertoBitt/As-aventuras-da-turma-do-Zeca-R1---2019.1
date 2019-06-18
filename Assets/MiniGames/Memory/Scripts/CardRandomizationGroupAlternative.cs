using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using com.csutil;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace MiniGames.Memory.Scripts
{
    [CreateAssetMenu(fileName = "Card Item Group Alternative", menuName = "Game Tools/Memory/Card Group Alternative")]
    public class CardRandomizationGroupAlternative : CardRandomizationGroup
    {
        public new List<CardItemAlternative> GroupItemListt = new List<CardItemAlternative>();
        [HideInInspector] public List<CardItemAlternative> monosillabas = new List<CardItemAlternative>();
        [HideInInspector] public List<CardItemAlternative> dissilabo = new List<CardItemAlternative>();
        [HideInInspector] public List<CardItemAlternative> trissilabo = new List<CardItemAlternative>();
        [HideInInspector] public List<CardItemAlternative> polissilabo = new List<CardItemAlternative>();
        
        [FormerlySerializedAs("wordsText")]
        [FoldoutGroup("Tools 1")]
        [TextArea]
        public string wordsText2;
        [FoldoutGroup("Tools 1")]
        public Classification wordsClassification;

        [FoldoutGroup("Tools 1")]
        [Button("Generate Assets")]
        public void GenerateCoroutine()
        {
            
#if UNITY_EDITOR
            var separator = ",";
            
            var wordsResult = wordsText2.Split(separator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            
            
            string path = AssetDatabase.GetAssetPath (Selection.activeObject);
            path = path.Replace (Path.GetFileName (path), "");
            for (var index = wordsResult.Length - 1; index >= 0; index--)
            {
                var word = wordsResult[index];
                if (word.IsNullOrWhitespace() || word.IsNullOrEmpty()) return;
                var asset = CreateInstance<CardItemAlternative>();
                var filepath = Path.GetInvalidFileNameChars()
                    .Aggregate(word, (current, c) => current.Replace(c, ' '));
                string assetPathAndName = $"{path}Palavras 3 ano/{wordsClassification}/{word}.asset";
                asset.classification = wordsClassification;
                asset.NameItem = word;
                AssetDatabase.CreateAsset(asset, assetPathAndName);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                if (asset != null)
                {
                    GroupItemListt.Add(asset); 
                }
            }

            EditorUtility.FocusProjectWindow ();
            #endif
        }
        [Button("Regenerate Lists")]
        private void Validate()
        {
            monosillabas.Clear();
            dissilabo.Clear();
            trissilabo.Clear();
            polissilabo.Clear();

            foreach (var item in GroupItemListt)
            {
                switch (item.classification)
                {
                    case Classification.Monossilabo:
                        monosillabas.Add(item);
                        break;
                    case Classification.Dissilabo:
                        dissilabo.Add(item);
                        break;
                    case Classification.Trissilabo:
                        trissilabo.Add(item);
                        break;
                    case Classification.Polissilabo:
                        polissilabo.Add(item);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public void ShuffleLists()
        {
            monosillabas.Shuffle();
            dissilabo.Shuffle();
            trissilabo.Shuffle();
            polissilabo.Shuffle();
            GroupItemListt.Shuffle();
        }

        public (CardItemAlternative monosillaba, CardItemAlternative dissilabo, CardItemAlternative trissilabo, CardItemAlternative polissilabo) GetByIndex(int index) => (monosillabas[index], dissilabo[index], trissilabo[index], polissilabo[index]);

        private void OnValidate()
        {
            int tempCount = GroupItemListt.Count;
            for (int i = 0; i < tempCount; i++)
            {
                GroupItemListt[i].idItem = i;
            }
        }
    }
}