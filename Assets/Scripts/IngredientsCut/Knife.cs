using System;
using System.Collections;
using UnityEngine;

namespace IngredientsCut
{
    public class Knife : MonoBehaviour
    {
        private Camera _mainCamera;
        private bool _isDragging;
        private IEnumerator _inputCoroutine;

        private void Start()
        {
            _mainCamera = Camera.main;
        }

        public void EnableInputDetection()
        {
            StopInputDetection();

            _inputCoroutine = DetectInput();
            StartCoroutine(_inputCoroutine);
        }

        public void StopInputDetection()
        {
            if (_inputCoroutine != null)
            {
                StopCoroutine(_inputCoroutine);
                _inputCoroutine = null;
            }
        }

        private IEnumerator DetectInput()
        {
            while (true)
            {
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);
                    Vector3 touchPosition = _mainCamera.ScreenToWorldPoint(touch.position);
                    touchPosition.z = 0;

                    if (touch.phase == TouchPhase.Began && IsTouchingKnife(touchPosition))
                    {
                        _isDragging = true;

                        transform.rotation = Quaternion.Euler(0, 0, 15f);
                    }
                    else if (touch.phase == TouchPhase.Moved && _isDragging)
                    {
                        transform.position = touchPosition;
                    }
                    else if (touch.phase == TouchPhase.Ended)
                    {
                        _isDragging = false;
                        transform.rotation = Quaternion.Euler(0, 0, 0);
                    }
                }

                yield return null;
            }
        }

        private bool IsTouchingKnife(Vector3 touchPosition)
        {
            Collider2D hitCollider = Physics2D.OverlapPoint(touchPosition);
            return hitCollider != null && hitCollider.gameObject == gameObject;
        }
    }
}
