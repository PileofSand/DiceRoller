﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

namespace DiceRoller
{
    public class DiceRollingSystem : MonoBehaviour
    {
        #region ACTIONS
        public Action<string> OnRollFinished;
        public Action OnRollStarted;
        #endregion

        #region VARIABLES
        [SerializeField]
        private Dice _dice;
        [SerializeField]
        private Camera _mainCamera;
        [SerializeField, Tooltip("Force of fake rolling (Button)")]
        private float _fakeRollForce = 10f;
        [SerializeField, Tooltip("Speed multiplier for the dragging, used on object.")]
        private float _mouseDragSpeed = 0.5f;
        [SerializeField, Tooltip("Value used to boost throw speed, Set to [1] to use realistic physics")]
        private float _throwAccelerationValue = 1f;
        [SerializeField, Tooltip("Height of dice when picking it up")]
        private float _pickUpHeight = 0.5f;
        [SerializeField, Tooltip("Speed of picking up dice")]
        private float _pickUpSpeed = 2f;
        [SerializeField, Tooltip("Bounds for Dice movement on X axis")]
        private Vector2 _xMovementBounds;
        [SerializeField, Tooltip("Bounds for Dice movement on Z axis")]
        private Vector2 _zMovementBounds;


        private bool _isFakeRolling;
        private Vector3 _velocity;
        private Plane _plane;
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
                Vector3 clampedTargetPos = new Vector3(
        Mathf.Clamp(dragPosition.x, _xMovementBounds.x, _xMovementBounds.y),
        _dice.transform.position.y,
        Mathf.Clamp(dragPosition.z, _zMovementBounds.x, _zMovementBounds.y));
        
                _dice.transform.position = Vector3.SmoothDamp(_dice.transform.position, clampedTargetPos, ref _velocity, _mouseDragSpeed);
            }
        }
        #endregion

        #region PUBLIC_METHODS
        public bool FakeRollDice()
        {
            if (_isFakeRolling)
            {
                return _isFakeRolling;
            }

            Vector3 Direction = new Vector3(Random.Range(-1f, 1f), Random.Range(0f, 1f), Random.Range(-1f, 1f)).normalized;
            // Apply the force to the die's Rigidbody component
            _dice.Rigidbody.velocity = Direction * _fakeRollForce;
            StartCoroutine(WaitForDiceToStop());
            OnRollStarted?.Invoke();
            return _isFakeRolling;
        }
        #endregion

        #region PRIVATE_METHODS
        private IEnumerator WaitForDiceToStop()
        {
            _isFakeRolling = true;
            while (_dice.Rigidbody.velocity.magnitude > 0f && !_dice.IsDragged)
            {
                yield return null;
            }
            _isFakeRolling = false;
            // Cast a ray downwards from the center of the dice to detect which side is facing up
            RaycastHit hit;
            if (Physics.Raycast(_dice.transform.position, Vector3.up, out hit))
            {
                // Determine the value of the roll based on the name of the GameObject that was hit
                var diceSide = hit.collider.gameObject.GetComponent<DiceSide>();
                if (diceSide)
                {
                    OnRollFinished?.Invoke(diceSide.SideValue);
                }
                else
                {
                    Debug.Log("No Side Detected");
                }
            }
        }

        private void DragFinished()
        {
            _dice.Rigidbody.useGravity = true;
            _dice.Rigidbody.velocity = _velocity * _throwAccelerationValue;
            StartCoroutine(WaitForDiceToStop());
            OnRollStarted?.Invoke();
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