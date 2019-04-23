using UnityEngine;

namespace MiniGames.Memory.Scripts
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "CardItemAlternative", menuName = "Game Tools/Memory/CardAlternative")]
    public class CardItemAlternative : CardItem
    {
        public Classification classification;
    }

    public enum Classification
    {
        Monossilabo,
        Dissilabo,
        Trissilabo,
        Polissilabo
    }
}