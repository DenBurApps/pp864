using System;
using UnityEngine;
using UnityEngine.UI;

namespace IngredientsCut
{
    public class Ingredient : MonoBehaviour
    {
        [SerializeField] private IngredientType _type;
        [SerializeField] private Image _progressImage;
        [SerializeField] private Sprite _defaultProgressSprite;
        [SerializeField] private Sprite[] _progressSprites;
        [SerializeField] private float _zposition;

        private int _progressIndex;
        private RectTransform _rectTransform;

        public delegate void ProgressCompleteHandler(Ingredient ingredient);

        public event ProgressCompleteHandler OnProgressComplete;

        public IngredientType Type => _type;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            SetDefaultProgressSprite();
            //SetZeroPosition();

            if (_rectTransform.position.z < 0)
            {
                _rectTransform.position = new Vector3(_rectTransform.position.x, _rectTransform.position.y, _zposition);
            }
        }

        public void SetDefaultProgressSprite()
        {
            _progressIndex = 0;
            _progressImage.sprite = _defaultProgressSprite;
        }

        public void SetZeroPosition()
        {
            //transform.position = new Vector3(transform.position.x, transform.position.y, _zposition);
            _rectTransform.position = new Vector3(_rectTransform.position.x, _rectTransform.position.y, _zposition);
        }

        public void IncreaseProgress()
        {
            _progressImage.sprite = _progressSprites[_progressIndex];
            _progressIndex++;
            
            if (_progressIndex >= _progressSprites.Length)
            {
                OnProgressComplete?.Invoke(this);
                return;
            }
        }
    }

    public enum IngredientType
    {
        Apple,
        Tomato,
        Cucumber,
        Eggplant
    }
}