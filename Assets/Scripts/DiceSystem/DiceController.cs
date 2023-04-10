using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

namespace DiceRoller
{
    public class DiceController : MonoBehaviour
    {
        #region ACTIONS
        public Action<int> OnRollFinished;
        public Action OnRollStarted;
        #endregion

        #region VARIABLES
        [SerializeField]
        private Camera _mainCamera;
        [SerializeField]
        private DiceData _diceData;
        [SerializeField]
        private DiceInput _diceInput;

        private bool _isRolling;
        private bool _isDragged;
        private Vector3 _velocity;
        private Vector3 _pickUpPosition;
        private Quaternion _pickUpRotation;
        #endregion

        #region UNITY_METHODS
        private void Awake()
        {
            Assert.IsNotNull(_diceData);
            Assert.IsNotNull(_diceInput);
            Assert.IsNotNull(_mainCamera);
        }

        private void Start()
        {
            _diceInput.OnDragStarted += DragStarted;
            _diceInput.OnDragFinished += DragFinished;
        }

        private void OnDestroy()
        {
            _diceInput.OnDragStarted -= DragStarted;
            _diceInput.OnDragFinished -= DragFinished;
        }

        private void FixedUpdate()
        {
            if (!_isDragged)
            {
                return;
            }

            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if (_diceData.Plane.Raycast(ray, out float initialDistance))
            {
                Vector3 dragPosition = ray.GetPoint(initialDistance);
                Vector3 clampedTargetPos = _diceData.GetClampedPosition(dragPosition);
                transform.position = Vector3.SmoothDamp(transform.position, clampedTargetPos, ref _velocity, _diceData.MouseDragSpeed);
            }
        }

        #endregion

        #region PUBLIC_METHODS
        /// <summary>
        /// Method used to roll a dice by UI Button.
        /// </summary>
        public void FakeRollDice()
        {
            if (_isRolling)
            {
                return;
            }

            Vector3 Direction = new Vector3(Random.Range(-1f, 1f), Random.Range(0f, 0.5f), Random.Range(-1f, 1f)).normalized;
            _diceData.Rigidbody.velocity = Direction * _diceData.FakeRollForce;
            StartCoroutine(WaitForDiceToStop());
            OnRollStarted?.Invoke();
        }
        #endregion

        #region PRIVATE_METHODS
        private void DragFinished()
        {
            if (_isRolling)
            {
                return;
            }

            _isDragged = false;
            _diceData.Rigidbody.useGravity = true;
            //Check if velocity is higher than minimum treshhold to proper throw.
            if (_velocity.magnitude > _diceData.MinimumVelocityTreshhold)
            {
                _diceData.Rigidbody.velocity = _velocity;
                StartCoroutine(WaitForDiceToStop());
                OnRollStarted?.Invoke();
            }
            else
            {
                StartCoroutine(ReturnDice(_pickUpPosition, _pickUpRotation, _diceData.ReturnDiceTime));
                _diceData.ResetDicePhysics();
            }
        }

        private void DragStarted()
        {
            if (_isRolling)
            {
                return;
            }
            _pickUpPosition = transform.position;
            _pickUpRotation = transform.rotation;

            _isDragged = true;
            _diceData.Rigidbody.useGravity = false;
            _diceData.ResetDicePhysics();
        }

        /// <summary>
        /// Corutine used to wait for dice to stop moving before reading upper side value.
        /// </summary>
        private IEnumerator WaitForDiceToStop()
        {
            _isRolling = true;

            while (_diceData.Rigidbody.velocity.magnitude > 0f && !_isDragged)
            {
                yield return null;
            }

            _isRolling = false;
            // Cast a ray downwards from the center of the dice to detect which side is facing up
            RaycastHit hit;
            if (Physics.Raycast(_diceData.transform.position, Vector3.up, out hit))
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

        /// <summary>
        /// Corutine used to move dice back to pickUp spot if velocity wasn't high enough to roll it properly.
        /// </summary>
        IEnumerator ReturnDice(Vector3 targetPosition, Quaternion targetRotation, float duration)
        {
            Vector3 startPosition = transform.position;
            Quaternion startRotation = transform.rotation;

            float time = 0f;
            while (time < duration)
            {
                time += Time.deltaTime;

                float t = Mathf.Clamp01(time / duration);
                transform.position = Vector3.Lerp(startPosition, targetPosition, t);
                transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);

                yield return null;
            }
            _isRolling = false;
            
            transform.position = targetPosition;
            transform.rotation = targetRotation;
        }
        #endregion
    }
}