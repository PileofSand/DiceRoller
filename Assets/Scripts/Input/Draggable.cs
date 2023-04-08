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

        #region VARIABLES
        private bool _isDragged;
        #endregion

        #region PROPERTIES
        public bool IsDragged => _isDragged;
        #endregion

        #region UNITY_METHODS
        private void OnMouseUp()
        {
            _isDragged = false;
            OnDragFinished?.Invoke();
        }

        private void OnMouseDown()
        {
            _isDragged = true;
            OnDragStarted?.Invoke();
        }
        #endregion
    }
}