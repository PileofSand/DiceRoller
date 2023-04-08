using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Globalization;

namespace DiceRoller.UI
{
    public class DiceRollingUISystem : MonoBehaviour
    {
        #region VARIABLES
        [SerializeField]
        private TextMeshProUGUI _textField;
        [SerializeField]
        private TextMeshProUGUI _sumTextField;
        [SerializeField]
        private Button _button;
        [SerializeField]
        private DiceRollingSystem _diceRollingSystem;

        private int _sumValue;
        #endregion

        #region UNITY_METHODS

        private void Start()
        {
            _button.onClick.AddListener(() => SetupRollButton());
            _diceRollingSystem.OnRollStarted += RollStarted;
            _diceRollingSystem.OnRollFinished += RollFinished;
        }

        private void OnDestroy()
        {
            _diceRollingSystem.OnRollStarted -= RollStarted;
            _diceRollingSystem.OnRollFinished -= RollFinished;
        }

        #endregion

        #region PRIVATE_METHODS  
        
        private void SetupRollButton()
        {
            _diceRollingSystem.FakeRollDice();
        }

        private void RollFinished(string value)
        {
            if (value.Contains("."))
            {
                value = value.Replace(".", "");
            }

            int addValue = int.Parse(value);
            _sumValue += addValue;
            _textField.text = addValue.ToString();
            _sumTextField.text = _sumValue.ToString();
            _button.interactable = true;
        }

        private void RollStarted()
        {
            _button.interactable = false;
            _textField.text = "?";
        }
        #endregion
    }
}