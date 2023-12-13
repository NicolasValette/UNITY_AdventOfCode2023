using AdventOfCode.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AdventOfCode.Character
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField]
        private GameObject _objectToMove;

        private IMove _moveProvider;



        private void Awake()
        {
            _moveProvider = _objectToMove.GetComponentInChildren<IMove>();
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnMove(InputValue value)
        {
            Vector3 inputValue = value.Get<Vector2>();
            Debug.Log("OnMove" + inputValue);
            _moveProvider.Move(inputValue);
        }
        public void OnFire(InputValue value)
        {
           
            Debug.Log("OnFire");
           
        }
        public void OnZoom(InputValue value)
        {
            float val = value.Get<float>();
            Debug.Log("OnZoom " + val);
            if (val > 0)
            {
                val = 1f;
            }
            else if (val < 0)
            {
                val = -1f;
            }
            _moveProvider.DepthMove(val);
        }
      
    }
}