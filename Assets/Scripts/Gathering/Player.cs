using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gathering
{
    [RequireComponent(typeof(Collider2D))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _maxX;
        [SerializeField] private float _minX;

        private Vector2 _defaultPosition;
        private Vector2 _previousTouchPosition;
        private Transform _transform;
        private int _currentDirection = 0;

        public event Action<Fruit> BadFruitCatched;
        public event Action<Fruit> GoodFruitCatched;

        private void Awake()
        {
            _transform = transform;
            _defaultPosition = _transform.position;
        }

        private void Start()
        {
            _transform.position = _defaultPosition;
        }

        public void ReturnToDefaultPosition()
        {
            _transform.position = _defaultPosition;
        }

        private void Update()
        {
            if (_currentDirection != 0)
            {
                Move(_currentDirection);
            }
        }

        private void Move(int direction)
        {
            Vector2 newPosition = _transform.position;
            newPosition.x += direction * _speed * Time.deltaTime;

            newPosition.x = Mathf.Clamp(newPosition.x, _minX, _maxX);

            _transform.position = newPosition;
        }

        public void SetDirection(int direction)
        {
            _currentDirection = direction;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out IIntractable interactable))
            {
                if (interactable is BadFruit badFruit)
                {
                    BadFruitCatched?.Invoke(badFruit);
                }
                else if (interactable is GoodFruit goodFruit)
                {
                    GoodFruitCatched?.Invoke(goodFruit);
                }
            }
        }
    }
}