using AdventOfCode.Interfaces;
using AdventOfCode.Solver.Day10;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AdventOfCode.Character
{
    public class PlayerMovement : MonoBehaviour, IMove
    {
        #region Serialized field
        [SerializeField]
        private float _movementSpeed;
        [SerializeField]
        private float _offset = 5f;
        #endregion
        private Vector3 _directionToMove = Vector3.zero;
        private Vector3 _directionToZoom = Vector3.zero;
        private Vector3 _velocity = Vector3.zero;

        // Start is called before the first frame update
        void Start()
        {

        }

        private void OnEnable()
        {
            SolverDay10.OnStart += Place;
        }
        private void OnDisable()
        {
            SolverDay10.OnStart -= Place;
        }

        // Update is called once per frame
        void Update()
        {
            MovePlayer();
            ZoomPlayer();
        }
        private void MovePlayer()
        {
            //transform.position = Vector3.SmoothDamp(transform.position, _directionToMove, ref _velocity, _movementSpeed);   
            transform.Translate(_directionToMove * _movementSpeed * Time.deltaTime);
        }
        private void ZoomPlayer()
        {
            //transform.position = Vector3.SmoothDamp(transform.position, _directionToMove, ref _velocity, _movementSpeed);   
            transform.Translate(_directionToZoom * _movementSpeed * Time.deltaTime);
        }

        public void Place(Vector2 location)
        {
            transform.position = new Vector3(-1*_offset, location.x * -1, location.y);
        }
        public void Move(Vector2 direction)
        {
            _directionToMove = new Vector3 (0f, direction.y , direction.x * -1);
        }

        public void DepthMove(float value)
        {
            _directionToZoom = new Vector3(value, 0, 0);
        }
    }
}