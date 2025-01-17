using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace IngredientsCut
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class IngredientToCut : MonoBehaviour
    {
        [SerializeField] private Sprite[] _cutSprites;
        [SerializeField] private GameObject[] _cutPieces;
        [SerializeField] private Transform[] _cutPoints;
        [SerializeField] private float _cutProximity = 0.2f;
        [SerializeField] private float _cutDelay = 1f;
        [SerializeField] private Sprite _defaultSprite;
        [SerializeField] private IngredientType _type;

        private int _currentCutIndex;
        private SpriteRenderer _spriteRenderer;
        private bool[] _isCut;
        private bool _isCutting;

        public event Action Cut;
        public event Action FullyCut;

        public IngredientType Type => _type;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
            _isCut = new bool[_cutPoints.Length];
            _currentCutIndex = 0;
            _isCutting = false;

            _spriteRenderer.sprite = _defaultSprite;

            foreach (var piece in _cutPieces)
            {
                piece.SetActive(false);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            TryCutIngredient(other);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            TryCutIngredient(other);
        }

        private void TryCutIngredient(Collider2D other)
        {
            if (_isCutting) return;

            if (other.TryGetComponent(out Knife knife))
            {
                int cutIndex = GetNextCutPointIndex(other.transform.position);

                if (cutIndex != -1)
                {
                    StartCoroutine(HandleCut(cutIndex));
                }
            }
        }

        private int GetNextCutPointIndex(Vector3 knifePosition)
        {
            for (int i = 0; i < _cutPoints.Length; i++)
            {
                if (!_isCut[i] && IsKnifeNearCutPoint(knifePosition, _cutPoints[i].position))
                {
                    return i;
                }
            }

            return -1;
        }

        private bool IsKnifeNearCutPoint(Vector3 knifePosition, Vector3 cutPoint)
        {
            float distance = Vector2.Distance(knifePosition, cutPoint);
            return distance <= _cutProximity;
        }

        private IEnumerator HandleCut(int cutIndex)
        {
            _isCutting = true;
            _isCut[cutIndex] = true;
            Debug.Log($"Cut point {cutIndex} cut!");

            Cut?.Invoke();

            if (_currentCutIndex < _cutSprites.Length)
            {
                _spriteRenderer.sprite = _cutSprites[_currentCutIndex];
                _cutPieces.FirstOrDefault(piece => !piece.activeSelf)?.SetActive(true);
                _currentCutIndex++;

                yield return new WaitForSeconds(_cutDelay);
            }

            if (_currentCutIndex == _cutSprites.Length)
            {
                Debug.Log("Ingredient fully cut!");
                FullyCut?.Invoke();
                gameObject.SetActive(false);
            }

            _isCutting = false;
        }
    }
}
