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
        private Dice _dice;

        private int _sumValue;
        #endregion

        #region UNITY_METHODS

        private void Start()
        {
            _button.onClick.AddListener(() => _dice.FakeRollDice(10f));
            _dice.OnDragFinished += OnRollStart;
            _dice.OnRollFinished += SetRolledValue;
        }

        private void OnDestroy()
        {
            _dice.OnDragFinished -= OnRollStart;
            _dice.OnRollFinished -= SetRolledValue;
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