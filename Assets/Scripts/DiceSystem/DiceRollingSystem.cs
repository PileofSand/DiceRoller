using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

namespace DiceRoller
{
    public class DiceRollingSystem : MonoBehaviour
    {
        #region ACTIONS
        public Action<int> OnRollFinished;
        public Action OnRollStarted;
        #endregion

        #region VARIABLES
        [SerializeField]
        private Dice _dice;
        [SerializeField]
        private Camera _mainCamera;
        [SerializeField, Tooltip("Force of fake rolling (Button)")]
        private float _fakeRollForce = 10f;
        [SerializeField, Tooltip("Minimum velocity, for throw to be random")]
        private float _minimumVelocityTreshhold = 1f;
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

        private bool _isRolling;
        private bool _isDragged;
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
            if (!_isDragged)
            {
                return;
            }

            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if (_plane.Raycast(ray, out float initialDistance))
            {
                Vector3 dragPosition = ray.GetPoint(initialDistance);
                Vector3 clampedTargetPos = GetClampedPosition(dragPosition);
                _dice.transform.position = Vector3.SmoothDamp(_dice.transform.position, clampedTargetPos, ref _velocity, _mouseDragSpeed);
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
            _dice.Rigidbody.velocity = Direction * _fakeRollForce;
            StartCoroutine(WaitForDiceToStop());
            OnRollStarted?.Invoke();
        }
        #endregion

        #region PRIVATE_METHODS
        /// <summary>
        /// Corutine used to wait for dice to stop moving before reading upper side value.
        /// </summary>
        private IEnumerator WaitForDiceToStop()
        {
            _isRolling = true;

            while (_dice.Rigidbody.velocity.magnitude > 0f && !_isDragged)
            {
                yield return null;
            }

            _isRolling = false;
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

        /// <summary>
        /// Getting clamped bounds where user can move dice with mouse.
        /// </summary>
        private Vector3 GetClampedPosition(Vector3 dragPosition)
        {
            return new Vector3(
            Mathf.Clamp(dragPosition.x, _xMovementBounds.x, _xMovementBounds.y),
            Mathf.Clamp(dragPosition.y, 0, _pickUpHeight),
            Mathf.Clamp(dragPosition.z, _zMovementBounds.x, _zMovementBounds.y));
        }

        private void DragFinished()
        {
            if (_isRolling)
            {
                return;
            }

            _isDragged = false;
            _dice.Rigidbody.useGravity = true;
            //Check if velocity is higher than minimum treshhold to proper throw.
            if (_velocity.magnitude > _minimumVelocityTreshhold)
            {
                _dice.Rigidbody.velocity = _velocity * _throwAccelerationValue;
                StartCoroutine(WaitForDiceToStop());
            }
            else
            {
                ResetDicePhysics();
            }

            OnRollStarted?.Invoke();
        }

        private void ResetDicePhysics()
        {
            _dice.Rigidbody.velocity = Vector3.zero;
            _dice.Rigidbody.angularVelocity = Vector3.zero;
        }

        private void DragStarted()
        {
            if (_isRolling)
            {
                return;
            }

            _isDragged = true;
            _dice.Rigidbody.useGravity = false;
            ResetDicePhysics();
        }
        #endregion
    }
}