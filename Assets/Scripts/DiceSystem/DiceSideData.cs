using System;

namespace DiceRoller
{
    [Serializable]
    public struct DiceSideData
    {
        public int sideValue;
        public DiceSide diceSide;
        public bool AddDotSymbol;
    }
}