using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace DiceRoller
{
    public class Draggable : MonoBehaviour
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