using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace DiceRoller
{
    public class DiceRollingSystem : MonoBehaviour
    {
        #region VARIABLES
        [SerializeField]
        private Dice _dice;
        [SerializeField]
        private Camera _mainCamera;
        [SerializeField, Tooltip("Speed multiplier for the dragging, used on object.")]
        private float _mouseDragSpeed = 0.5f;
        [SerializeField, Tooltip("Value used to boost throw speed, Set to [1] to use realistic physics")]
        private float _throwAccelerationValue = 1f;
        [SerializeField, Tooltip("Height of dice when picking it up")]
        private float _pickUpHeight = 0.5f;
        [SerializeField, Tooltip("Speed of picking up dice")]
        private float _pickUpSpeed = 2f;

        private Vector3 _velocity;
        private Plane _plane;
        private Vector3 _throwVelocity;
        #endregion

        #region UNITY_METHODS
        private void Awake()
        {
            Assert.IsNotNull(_dice);
            Assert.IsNotNull(_mainCamera);
            // define a plane that is parallel to the ground and passes through the dice object
            _plane = new Plane(Vector3.up, _dice.transform.position);
        }

        private void Start()
        {
            _dice.OnDragStarted += DragStarted;
            _dice.OnDragFinished += DragFinished;
        }

        private void OnDestroy()
        {
            _dice.OnDragStarted -= DragStarted;
            _dice.OnDragFinished -= DragFinished;
        }

        private void FixedUpdate()
        {
            if (!_dice.IsDragged)
            {
                return;
            }

            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if (_plane.Raycast(ray, out float initialDistance))
            {
                Vector3 dragPosition = ray.GetPoint(initialDistance);
                _throwVelocity = (dragPosition - _dice.transform.position);
                _dice.transform.position = Vector3.SmoothDamp(_dice.transform.position, dragPosition, ref _velocity, _mouseDragSpeed);
            }
        }
        #endregion

        #region PRIVATE_METHODS

        private void DragFinished()
        {
            _dice.Rigidbody.useGravity = true;
            _dice.Rigidbody.velocity = _throwVelocity * _throwAccelerationValue;
        }

        private void DragStarted()
        {
            _dice.Rigidbody.useGravity = false;
            _dice.Rigidbody.velocity = Vector3.zero;
            _dice.transform.Translate(Vector3.up * _pickUpHeight * _pickUpSpeed);
        }
        #endregion
    }
}