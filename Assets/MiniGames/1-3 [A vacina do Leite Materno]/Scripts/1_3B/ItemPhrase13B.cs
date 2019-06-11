using System;
using System.Collections.Generic;
using MiniGames.Scripts._1_3B;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif
using UnityEngine;

namespace MiniGames.Scripts
{
    [CreateAssetMenu(menuName = "ADTZ/Vacina do Leite Materno/Frase")]
    public class ItemPhrase13B : ScriptableObject
    {
        [TextArea(0,3)]
        public string question;
        [AssetList(Path = "MiniGames/1-3 [A vacina do Leite Materno]/SO/1ª Ano/")]
        public List<ItemWord1_3b> correctAlternatives;
        [AssetList(Path = "MiniGames/1-3 [A vacina do Leite Materno]/SO/1ª Ano/", CustomFilterMethod = "IsItemOnCorrect")]
        public List<ItemWord1_3b> wrongAlternatives;

#if UNITY_EDITOR
        [Button("Update Asset Name", ButtonSizes.Medium)]
        public void UpdateAssetName()
        {
            var pathAsset = AssetDatabase.GetAssetPath(this);
            AssetDatabase.RenameAsset(pathAsset, question);
        }

        [Button("Add New Asset", ButtonSizes.Medium, ButtonStyle.FoldoutButton)]
        public void DuplicateWith(string newQuestion)
        {
            var asset = CreateInstance<ItemPhrase13B> ();
            asset.question = newQuestion;


            string path = AssetDatabase.GetAssetPath (Selection.activeObject);
            if (path == "")
            {
                path = "Assets";
            }
            else if (Path.GetExtension (path) != "")
            {
                path = path.Replace (Path.GetFileName (AssetDatabase.GetAssetPath (Selection.activeObject)), "");
            }

            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath (path + "/" + newQuestion + ".asset");


            Debug.Log(assetPathAndName);

            AssetDatabase.CreateAsset (asset, assetPathAndName);

            AssetDatabase.SaveAssets ();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow ();
            Selection.activeObject = asset;

        }

        [Button("Add New Assets", ButtonSizes.Medium, ButtonStyle.FoldoutButton)]
        public void CreateNewQuestionAssets(List<String> newQuestions)
        {
            string path = AssetDatabase.GetAssetPath (Selection.activeObject);
            if (path == "")
            {
                path = "Assets";
            }
            else if (Path.GetExtension (path) != "")
            {
                path = path.Replace (Path.GetFileName (AssetDatabase.GetAssetPath (Selection.activeObject)), "");
            }



            foreach (var questionString in newQuestions)
            {
                var asset = CreateInstance<ItemPhrase13B> ();
                asset.question = questionString;

                string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath (path + "/" + questionString + ".asset");

                AssetDatabase.CreateAsset (asset, assetPathAndName);

                AssetDatabase.SaveAssets ();
            }

            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow ();

        }

#endif

        private bool IsItemOnCorrect(ItemWord1_3b obj)
        {
            return !correctAlternatives.Contains(obj);
        }

        private void OnValidate()
        {
            foreach (var alternative in wrongAlternatives)
            {
                if (correctAlternatives.Contains(alternative))
                {
                    correctAlternatives.Remove(alternative);
                }
            }
        }
    }

}
