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
        void Awake()
        {
            Assert.IsNotNull(_diceSides);
            _rigidbody = GetComponent<Rigidbody>();
            SetupDiceSides(_diceSides);
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
        #endregion

        #region PRIVATE_METHODS
        private void SetupDiceSides(List<DiceSideData> diceSides)
        {
            for (int i = 0; i < diceSides.Count; i++)
            {
                diceSides[i].diceSide.Setup(diceSides[i].sideValue);
            }
        }
        #endregion
    }
}