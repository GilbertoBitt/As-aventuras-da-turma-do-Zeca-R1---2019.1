using System;
using System.Globalization;
using Sirenix.OdinInspector;
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
        public int CountSyllables => (int) wordClassification.classification;
        public int CountLetters => syllables.Length;
        [ReadOnly]
        public string[] charSyllabes;
        public TonicSyllable tonicSyllables;
        public EnumClassification wordClassification;

        private void OnValidate()
        {
            charSyllabes = new string[word.Length];
            for (int i = 0; i < word.Length; i++)
            {
                charSyllabes[i] = word.ToLower()[i].ToString();
            }

            if (syllables.Length != CountSyllables)
            {
                syllables = new string[CountSyllables];
            }

            string[] copy = new string[CountSyllables];
            syllables.CopyTo(copy, 0);
            syllables = new string[CountSyllables];
            for (int i = 0; i < CountSyllables; i++)
            {
                syllables[i] = copy[i];
            }


        }

        public string ToTitleCase(string str) {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
        }

        public enum ClassificationBySyllables
        {
            Monosillabas = 1,
            Dissilabas,
            Trissilabas
        }

        public enum TonicSyllablesClassification
        {
            Oxitona,
            Paroxitona,
            Proparoxitona,
        }

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
    }
}
