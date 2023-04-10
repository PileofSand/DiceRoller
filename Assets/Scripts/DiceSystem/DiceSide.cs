using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace DiceRoller
{
    public class DiceSide : MonoBehaviour
    {
        [SerializeField]
        private TextMeshPro m_textMesh;

        public int SideValue { get; set; }

        #region UNITY_METHODS
        private void Awake()
        {
            Assert.IsNotNull(m_textMesh);
        }
        #endregion

        #region PUBLIC_METHODS
        public void Setup(int value, bool addDotSymbol)
        {
            SideValue = value;
            string displayValue;

            if (addDotSymbol)
            {
                displayValue = value + ".";
            }
            else
            {
                displayValue = value.ToString();
            }

            m_textMesh.text = displayValue;
        }
        #endregion
    }
}