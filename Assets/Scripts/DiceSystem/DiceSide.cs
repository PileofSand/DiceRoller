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
        public void Setup(int value)
        {
            SideValue = value;
            m_textMesh.text = value.ToString();
        }
        #endregion
    }
}