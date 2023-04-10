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
        private DiceController _diceController;

        private int _sumValue;
        #endregion

        #region UNITY_METHODS
        private void Start()
        {
            _button.onClick.AddListener(() => SetupRollButton());
            _diceController.OnRollStarted += RollStarted;
            _diceController.OnRollFinished += RollFinished;
        }

        private void OnDestroy()
        {
            _diceController.OnRollStarted -= RollStarted;
            _diceController.OnRollFinished -= RollFinished;
        }
        #endregion

        #region PRIVATE_METHODS  
        private void SetupRollButton()
        {
            _diceController.FakeRollDice();
        }

        private void RollFinished(int value)
        {
            _sumValue += value;
            _textField.text = value.ToString();
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