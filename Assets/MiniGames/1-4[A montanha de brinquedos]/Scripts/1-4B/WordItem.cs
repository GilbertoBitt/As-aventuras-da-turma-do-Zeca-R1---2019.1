using System;
using System.Globalization;
using com.csutil;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace MiniGames.Scripts
{
    [CreateAssetMenu(menuName = "Eduqbrinq/MDB/WordItem.asset")]
    public class WordItem : ScriptableObject
    {
        public string word;
        [InlineEditor(InlineEditorModes.SmallPreview)]
        public Sprite itemSprite;
        [ListDrawerSettings(HideAddButton = true, HideRemoveButton = true)]
        public string[] syllables;
        public int CountSyllables => wordClassification.classification != ClassificationBySyllables.Polissilaba ? (int) wordClassification.classification : wordClassification.syllableAmount;
        public int CountLetters => letters.Length;
        [ReadOnly]
        public string[] charSyllabes;
        public string[] letters;
        public TonicSyllable tonicSyllables = new TonicSyllable();
        public EnumClassification wordClassification = new EnumClassification();
        public AlternativeWords alternativeWordsContent = new AlternativeWords();
        public bool buildWithLetters = false;

        [Button("Force Validate")]
        private void OnValidate()
        {
            charSyllabes = new string[word.Length];
            for (int i = 0; i < word.Length; i++)
            {
                charSyllabes[i] = word.ToLower()[i].ToString();
            }

            if (syllables != null && syllables.Length != CountSyllables)
            {
                syllables = new string[CountSyllables];
            }

            string[] copy = new string[CountSyllables];
            syllables?.CopyTo(copy, 0);
            syllables = new string[CountSyllables];
            for (int i = 0; i < CountSyllables; i++)
            {
                syllables[i] = copy[i];
            }

            letters = new string[word.Length];

            for (int i = 0; i < word.Length; i++)
            {
                letters[i] = word[i].ToString().ToLower();
            }

            if (!alternativeWordsContent.enabled) return;

            for (var index = 0; index < alternativeWordsContent.alternativeWords.Length; index++)
            {
                var alternativeWord = alternativeWordsContent.alternativeWords[index];
                alternativeWordsContent.alternativeWords[index] = alternativeWord.ToLower();
            }
        }

        public string ToTitleCase(string str) {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
        }

        public enum ClassificationBySyllables
        {
            Monosillabas = 1,
            Dissilabas,
            Trissilabas,
            Polissilaba
        }

        public enum TonicSyllablesClassification
        {
            Oxitona,
            Paroxitona,
            Proparoxitona,
        }

        #if UNITY_EDITOR
        [Button(ButtonSizes.Medium, ButtonStyle.FoldoutButton)]
        public void Fonetizars(string strings)
        {
            var tempSyllable = strings.Split("·");
            wordClassification.enabled = true;

            if (tempSyllable.Length < 4)
            {

                wordClassification.classification = (ClassificationBySyllables) tempSyllable.Length;
            }
            else
            {
                wordClassification.classification = ClassificationBySyllables.Polissilaba;
                wordClassification.syllableAmount = tempSyllable.Length;
            }

            this.syllables = tempSyllable;

        }

        [Button(ButtonSizes.Medium, ButtonStyle.FoldoutButton)]
        public void GenerateWords(string strings, TonicSyllablesClassification classification)
        {
            var words = strings.Split(",");
            words.ForEach(s =>
            {
                var instance = ScriptableObject.CreateInstance<WordItem>();
                instance.word = s.ToLower();
                instance.tonicSyllables = new TonicSyllable {enabled = true, classification = classification};
                AssetDatabase.CreateAsset(instance, $"Assets/MiniGames/1-4[A montanha de brinquedos]/Scripts/1-4B/Words/3ª Ano/{s.ToLower()}.asset");
            });
            AssetDatabase.SaveAssets ();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow ();

        }

        [Button(name:"Generate with Fonetic", ButtonSizes.Medium, ButtonStyle.FoldoutButton)]
        public void GenerateWordsWithSyllables(string strings, TonicSyllablesClassification classification)
        {
            var words = strings.Split(",");
            words.ForEach(s =>
            {
                var instance = ScriptableObject.CreateInstance<WordItem>();
                instance.word = s.Replace("·", "").ToLower();
                instance.tonicSyllables.enabled = true;
                instance.tonicSyllables.classification = classification;

                var tempSyllable = s.Split("·");
                instance.wordClassification.enabled = true;
                if (tempSyllable.Length >= 4)
                {
                    instance.wordClassification.classification = ClassificationBySyllables.Polissilaba;
                    instance.wordClassification.syllableAmount = tempSyllable.Length;
                }
                else
                {
                    instance.wordClassification.classification = (ClassificationBySyllables) tempSyllable.Length;
                }

                instance.syllables = tempSyllable;
                AssetDatabase.CreateAsset(instance, $"Assets/MiniGames/1-4[A montanha de brinquedos]/Scripts/1-4B/Words/3ª Ano/{s.Replace("·", "").ToLower()}.asset");
            });
            AssetDatabase.SaveAssets ();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow ();

        }

        #endif
    }

    [Serializable, Toggle("enabled")]
    public class TonicSyllable
    {
        public bool enabled;
        public WordItem.TonicSyllablesClassification classification;
    }

    [Serializable, Toggle("enabled")]
    public class EnumClassification
    {
        public bool enabled;
        public WordItem.ClassificationBySyllables classification;
        [ShowIf("classification", WordItem.ClassificationBySyllables.Polissilaba)]
        public int syllableAmount;
    }

    [Serializable, Toggle("enabled")]
    public class AlternativeWords
    {
        public bool enabled;
        public string[] alternativeWords = new string[1];

    }
}
