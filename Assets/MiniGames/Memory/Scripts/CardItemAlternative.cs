using System;
using UnityEngine;

namespace MiniGames.Memory.Scripts
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "CardItemAlternative", menuName = "Game Tools/Memory/CardAlternative")]
    public class CardItemAlternative : CardItem
    {
        public Classification classification;

        public string GetString()
        {
            switch (classification)
            {
                case Classification.Monossilabo:
                    return "Monossílabo";
                case Classification.Dissilabo:
                    return "Dissílabo";
                case Classification.Trissilabo:
                    return "Trissílabo";
                case Classification.Polissilabo:
                    return "Polissílabo";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public enum Classification
    {
        Monossilabo,
        Dissilabo,
        Trissilabo,
        Polissilabo
    }
    
}