using System;
using System.Globalization;
using System.Text;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MiniGames.Scripts._1_3B
{
    [CreateAssetMenu(menuName = "ADTZ/Vacina do Leite Materno/Palavra")]
    [Serializable]
    public class ItemWord1_3b : ScriptableObject
    {
        [InlineEditor(InlineEditorModes.SmallPreview)]
        public Sprite itemSprite;

        public string itemWord;

        [ReadOnly] public char startLetter;

        private void OnValidate()
        {
            if(string.IsNullOrEmpty(itemWord) || string.IsNullOrWhiteSpace(itemWord)) return;
            startLetter = RemoverAcentos(itemWord).ToUpper(CultureInfo.InvariantCulture)[0];
        }

        private string RemoverAcentos(string texto)
        {
            string s = texto.Normalize(NormalizationForm.FormD);

            StringBuilder sb = new StringBuilder();

            for (int k = 0; k < s.Length; k++)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(s[k]);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(s[k]);
                }
            }
            return sb.ToString();
        }
    }
}
