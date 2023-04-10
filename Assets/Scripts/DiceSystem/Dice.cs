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
    public class Dice : Draggable
    {
        #region VARIABLES
        [SerializeField]
        private List<DiceSideData> _diceSides = new List<DiceSideData>();

        private Rigidbody _rigidbody;
        #endregion

        #region PROPERTIES
        public Rigidbody Rigidbody => _rigidbody;
        #endregion

        #region UNITY_METHODS
        // Start is called before the first frame update
        private void Awake()
        {
            Assert.IsNotNull(_diceSides);
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            SetupDiceSides(_diceSides);
        }
        #endregion

        #region PUBLIC_METHODS
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
                    diceSideData.diceSide.Setup(diceSideData.sideValue, true );
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