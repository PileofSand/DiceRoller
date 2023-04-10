using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiceRoller
{
    public class DiceInput : MonoBehaviour
    {
        #region ACTIONS
        public Action OnDragStarted;
        public Action OnDragFinished;
        #endregion

        #region UNITY_METHODS
        private void OnMouseUp()
        {
            OnDragFinished?.Invoke();
        }

        private void OnMouseDown()
        {
            OnDragStarted?.Invoke();
        }
        #endregion
    }
}