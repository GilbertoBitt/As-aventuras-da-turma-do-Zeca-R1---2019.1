using System;
using UnityEngine.Serialization;

namespace MiniGames.Rainbow.Scripts._1ª_Ano
{
    [Serializable]
    public class EquationContent
    {
        public int firstValue;
        public EquationType equationType = EquationType.Multiplication;
        public GroupType groupType = GroupType.Acrescentar;
        public int secondValue;
        public RainbowItem rainbowItem;

        public int ResultValue()
        {
            switch (equationType)
            {
                case EquationType.Addition:
                    return firstValue + secondValue;
                case EquationType.Subtraction:
                    return firstValue - secondValue;
                case EquationType.Division:
                    return firstValue / secondValue;
                case EquationType.Multiplication:
                    return firstValue * secondValue;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public enum EquationType
    {
        Addition,
        Subtraction,
        Division,
        Multiplication
    }

    public enum GroupType
    {
        Juntar,
        Acrescentar,
        Separar,
        Retirar,
    }
}
