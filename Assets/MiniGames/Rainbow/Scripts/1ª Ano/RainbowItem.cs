using Sirenix.OdinInspector;
using UnityEngine;

namespace MiniGames.Rainbow.Scripts._1ª_Ano
{
    [CreateAssetMenu(menuName = "Eduqbrinq/Arcoiris/Rainbow Item")]
    public class RainbowItem : ScriptableObject
    {
        [PreviewField]
        public Sprite sprite;
        public string name;
    }
}
