using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
            _diceRollingSystem.OnRollStarted += OnRollStarted;
            _diceRollingSystem.OnRollFinished += OnRollFinished;
        }

        private void OnDestroy()
        {
            _diceRollingSystem.OnRollStarted -= OnRollStarted;
            _diceRollingSystem.OnRollFinished -= OnRollFinished;
        }

        #endregion

        #region PRIVATE_METHODS  
        
        private void SetupRollButton()
        {
            _diceRollingSystem.FakeRollDice();
        }

        private void OnRollFinished(int value)
        {
            _sumValue += value;
            _textField.text = value.ToString();
            _sumTextField.text = _sumValue.ToString();
            _button.interactable = true;
        }

        private void OnRollStarted()
        {
            _button.interactable = false;
            _textField.text = "?";
        }
        #endregion
    }
}