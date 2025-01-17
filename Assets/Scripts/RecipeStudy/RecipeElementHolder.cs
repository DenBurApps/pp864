using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RecipeStudy
{
    public class RecipeElementHolder : MonoBehaviour
    {
        [SerializeField] private RecipeElement[] _recipeElements;

        private int _sequenceCount;
        private int _sequenceInterval = 2;
        private List<ElementType> _shownElements = new();
        private IEnumerator _disablingCoroutine;
        private bool _isPaused;

        public event Action AllElementsShown;
        public event Action ElementCorrectlyChosen;
        public event Action ElementIncorrectlyChosen;
        public event Action AllElementsCorrectlyChosen;

        public IReadOnlyCollection<ElementType> ShownElements => _shownElements;

        private void OnEnable()
        {
            foreach (var element in _recipeElements)
            {
                element.ElementClicked += OnElementClicked;
            }
        }

        private void OnDisable()
        {
            foreach (var element in _recipeElements)
            {
                element.ElementClicked -= OnElementClicked;
            }
        }

        public void DisableAllElements()
        {
            foreach (var element in _recipeElements)
            {
                element.gameObject.SetActive(false);
            }
        }

        public void EnableAllElements()
        {
            foreach (var element in _recipeElements)
            {
                element.gameObject.SetActive(true);
                element.Enable();
            }
        }

        public void StartSequence()
        {
            StopSequence();

            _shownElements.Clear();
            _disablingCoroutine = SequenceCoroutine();
            StartCoroutine(_disablingCoroutine);
        }

        public void StopSequence()
        {
            if (_disablingCoroutine == null) return;

            StopCoroutine(_disablingCoroutine);
            _disablingCoroutine = null;
        }

        public void SetSequenceCount(int count)
        {
            if (count <= 0 || _recipeElements == null || count > _recipeElements.Length)
                return;

            _sequenceCount = count;
        }

        public void PauseSequence()
        {
            _isPaused = true;
        }

        public void ResumeSequence()
        {
            _isPaused = false;
        }

        private IEnumerator SequenceCoroutine()
        {
            var interval = new WaitForSeconds(_sequenceInterval);

            for (int i = 0; i < _sequenceCount; i++)
            {
                while (_isPaused)
                {
                    yield return null;
                }

                yield return interval;

                var randomIndex = Random.Range(0, _recipeElements.Length);
                _recipeElements[randomIndex].gameObject.SetActive(true);
                _recipeElements[randomIndex].StartDisabling();
                _shownElements.Add(_recipeElements[randomIndex].Type);

                yield return interval;
            }

            AllElementsShown?.Invoke();
        }

        private void OnElementClicked(RecipeElement clickedElement)
        {
            if (_shownElements.Count == 0) return;

            if (_shownElements[0] == clickedElement.Type)
            {
                _shownElements.RemoveAt(0);
                ElementCorrectlyChosen?.Invoke();

                if (_shownElements.Count == 0)
                {
                    AllElementsCorrectlyChosen?.Invoke();
                }
            }
            else
            {
                ElementIncorrectlyChosen?.Invoke();
                Debug.Log("incorrect");
            }
        }
    }
}