using System;
using System.Globalization;
using System.Linq;
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

            if (tonicSyllables.enabled)
            {
                tonicSyllables.silabaTonica = syllables[
                    tonicSyllables.classification == TonicSyllablesClassification.Oxitona
                        ? syllables.Length - 1
                        : tonicSyllables.classification == TonicSyllablesClassification.Paroxitona
                            ? syllables.Length - 2
                            : syllables.Length - 3].ToLower();
            }
            
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
        [Button(name:"Genarete 1ª ano",ButtonSizes.Medium, ButtonStyle.FoldoutButton)]
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

        [Button(name:"Generate 2ª ano",ButtonSizes.Medium, ButtonStyle.FoldoutButton)]
        public void GenerateWords(string strings, TonicSyllablesClassification classification)
        {
            var words = strings.Split("!");
            for (var index = 0; index < words.Length; index++)
            {
                var sw = words[index];
                var final = sw.Split("/");
                words[index] = final.First();
            }

            words.ForEach(s =>
            {
                var instance = ScriptableObject.CreateInstance<WordItem>();
                var wordFromOptions = s.Split("*");
                instance.word = wordFromOptions.First();
                instance.alternativeWordsContent = new AlternativeWords
                {
                    enabled = true,
                    alternativeWords = wordFromOptions.Last().Split(",")
                };
                try
                {
                    AssetDatabase.CreateAsset(instance,
                        $"Assets/MiniGames/1-4[A montanha de brinquedos]/Scripts/1-4B/Words/2ª Ano/{instance.word}.asset");
                } catch (Exception e) {
                    Debug.Log($"Execption {e}");
                    Debug.DebugBreak();
                }
            });
            AssetDatabase.SaveAssets ();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow ();

        }

        [Button(name:"Generate 3ª ano word", ButtonSizes.Medium, ButtonStyle.FoldoutButton)]
        public void GenerateWordsWithSyllables(string strings, TonicSyllablesClassification classification)
        {
            var words = strings.Split(",");
            words.ForEach(s =>
            {
                var instance = ScriptableObject.CreateInstance<WordItem>();
                var splittedWord = s.ToLower().Split("-");
                instance.wordClassification.enabled = true;
                instance.wordClassification.classification = (ClassificationBySyllables)splittedWord.Length;
                instance.syllables = splittedWord;
                instance.wordClassification.syllableAmount = splittedWord.Length;
                var sCompleted = s.ToLower().Replace("-", "");
                instance.word = sCompleted;
                instance.tonicSyllables = new TonicSyllable {enabled = true, classification = classification};
                try
                {
                    AssetDatabase.CreateAsset(instance,
                        $"Assets/MiniGames/1-4[A montanha de brinquedos]/Scripts/1-4B/Words/3ª Ano/{sCompleted}.asset");
                } catch (Exception e) {
                    Debug.Log($"Execption {e}");
                    Debug.DebugBreak();
                }
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
        public string silabaTonica;
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
