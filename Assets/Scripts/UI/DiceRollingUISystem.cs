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
            _button.onClick.AddListener(() => _diceRollingSystem.FakeRollDice(10f));
            _diceRollingSystem.OnRollStarted += OnRollStart;
            _diceRollingSystem.OnRollFinished += SetRolledValue;
        }

        private void OnDestroy()
        {
            _diceRollingSystem.OnRollStarted -= OnRollStart;
            _diceRollingSystem.OnRollFinished -= SetRolledValue;
        }

        #endregion

        #region PRIVATE_METHODS        
        private void SetRolledValue(int value)
        {
            _sumValue += value;
            _textField.text = value.ToString();
            _sumTextField.text = _sumValue.ToString();
        }

        private void OnRollStart()
        {
            _textField.text = "?";
        }
        #endregion
    }
}