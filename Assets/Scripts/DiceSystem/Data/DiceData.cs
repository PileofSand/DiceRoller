using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiceRoller
{
    public class DiceData : MonoBehaviour
    {
        #region VARIABLES
        [SerializeField, Tooltip("Sides for dice, setup in inspector (Context Menu)")]
        private List<DiceSideData> _diceSides = new List<DiceSideData>();
        [SerializeField, Tooltip("Force of fake rolling (Button)")]
        private float _fakeRollForce = 10f;
        [SerializeField, Tooltip("Minimum velocity, for throw to be random")]
        private float _minimumVelocityTreshhold = 1f;
        [SerializeField, Tooltip("Speed multiplier for the dragging, used on object.")]
        private float _mouseDragSpeed = 0.5f;
        [SerializeField, Tooltip("Height of dice when picking it up")]
        private float _pickUpHeight = 0.5f;
        [SerializeField, Tooltip("Bounds for Dice movement on X axis")]
        private Vector2 _xMovementBounds;
        [SerializeField, Tooltip("Bounds for Dice movement on Z axis")]
        private Vector2 _zMovementBounds;

        private Plane _plane;
        private Rigidbody _rigidbody;

        public float MinimumVelocityTreshhold => _minimumVelocityTreshhold;
        public float MouseDragSpeed => _mouseDragSpeed;
        public float FakeRollForce => _fakeRollForce;
        public Plane Plane => _plane;
        public Rigidbody Rigidbody => _rigidbody;
        #endregion

        #region UNITY_METHODS

        private void Start()
        {
            SetupDiceSides(_diceSides);
            _rigidbody = GetComponent<Rigidbody>();
            _plane = new Plane(Vector3.up, transform.position);
        }
        #endregion

        #region PUBLIC_METHODS

        public void ResetDicePhysics()
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }

        /// <summary>
        /// Getting clamped bounds where user can move dice with mouse.
        /// </summary>
        public Vector3 GetClampedPosition(Vector3 dragPosition)
        {
            return new Vector3(
            Mathf.Clamp(dragPosition.x, _xMovementBounds.x, _xMovementBounds.y),
            Mathf.Clamp(dragPosition.y, 0, _pickUpHeight),
            Mathf.Clamp(dragPosition.z, _zMovementBounds.x, _zMovementBounds.y));
        }

        /// <summary>
        /// Method used in inpector context menu to gather all dice sides into a list.
        /// </summary>
        [ContextMenu("Gather Dice Sides")]
        public void GatherDiceSides()
        {
            DiceSide[] diceSides = GetComponentsInChildren<DiceSide>();
            _diceSides.Clear();
            for (int i = 0; i < diceSides.Length; i++)
            {
                DiceSideData diceSideData = new DiceSideData();
                diceSideData.sideValue = (i + 1);
                diceSideData.diceSide = diceSides[i];
                _diceSides.Add(diceSideData);
            }
        }
        #endregion

        #region PRIVATE_METHODS
        private void SetupDiceSides(List<DiceSideData> diceSides)
        {
            for (int i = 0; i < diceSides.Count; i++)
            {
                DiceSideData diceSideData = diceSides[i];
                if (diceSideData.AddDotSymbol)
                {
                    diceSideData.diceSide.Setup(diceSideData.sideValue, true);
                }
                else
                {
                    diceSideData.diceSide.Setup(diceSideData.sideValue, false);
                }
            }
        }
        #endregion
    }
}