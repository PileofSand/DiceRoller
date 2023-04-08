using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

namespace DiceRoller
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    public class Dice : MonoBehaviour
    {
        #region ACTIONS
        public Action OnDragStarted;
        public Action OnDragFinished;
        public Action<int> OnRollFinished;
        #endregion

        #region VARIABLES
        [SerializeField]
        private List<DiceSideData> _diceSides = new List<DiceSideData>();

        private Rigidbody _rigidbody;
        private bool _isDragged;
        #endregion

        #region PROPERTIES
        public Rigidbody Rigidbody => _rigidbody;
        public bool IsDragged => _isDragged;
        #endregion

        #region UNITY_METHODS

        // Start is called before the first frame update
        void Awake()
        {
            Assert.IsNotNull(_diceSides);
            _rigidbody = GetComponent<Rigidbody>();
            SetupDiceSides(_diceSides);
        }

        private void OnMouseUp()
        {
            _isDragged = false;
            RollDice();
        }

        private void OnMouseDown()
        {
            _isDragged = true;
            OnDragStarted?.Invoke();
        }
        #endregion

        #region PRIVATE_METHODS

        private void RollDice()
        {
            OnDragFinished?.Invoke();
            StartCoroutine(WaitForDiceToStop());
        }

        private void SetupDiceSides(List<DiceSideData> diceSides)
        {
            for (int i = 0; i < diceSides.Count; i++)
            {
                diceSides[i].diceSide.Setup(diceSides[i].sideValue);
            }
        }

        private IEnumerator WaitForDiceToStop()
        {
            while (Rigidbody.velocity.magnitude > 0f && !IsDragged)
            {
                yield return null;
            }

            // Cast a ray downwards from the center of the dice to detect which side is facing up
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.up, out hit))
            {
                // Determine the value of the roll based on the name of the GameObject that was hit
                var diceSide = hit.collider.gameObject.GetComponent<DiceSide>();
                if (diceSide)
                {
                    Debug.Log("Dice rolled: " + diceSide.SideValue);
                    OnRollFinished?.Invoke(diceSide.SideValue);
                }
                else
                {
                    Debug.Log("No Side Detected");
                }
            }
        }
        #endregion

        #region PUBLIC_METHODS
        //Context menu method used to gather all dice side datas into list.
        [ContextMenu("Gather Dice Sides")]
        public void GatherDiceSides()
        {
            DiceSide[] diceSides = GetComponentsInChildren<DiceSide>();
            _diceSides.Clear();
            for (int i = 0; i < diceSides.Length; i++)
            {
                DiceSideData diceSideData = new DiceSideData();
                diceSideData.sideValue = i + 1;
                diceSideData.diceSide = diceSides[i];
                _diceSides.Add(diceSideData);
            }
        }

        public void FakeRollDice(float forceMagnitude)
        {
            Vector3 Direction = new Vector3(Random.Range(-1f, 1f), Random.Range(0f, 1f), Random.Range(-1f, 1f)).normalized;
            // Apply the force to the die's Rigidbody component
            _rigidbody.AddForce(Direction * forceMagnitude, ForceMode.Impulse);
            _rigidbody.AddTorque(Direction * forceMagnitude, ForceMode.Impulse);
            RollDice();
        }
        #endregion
    }
}