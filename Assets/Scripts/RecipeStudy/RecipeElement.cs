using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RecipeStudy
{
    public class RecipeElement : MonoBehaviour
    {
        [SerializeField] private float _disablingPeriod;
        [SerializeField] private ElementType _elementType;
        [SerializeField] private Button _button;

        private IEnumerator _disablingCoroutine;
        
        public event Action<RecipeElement> ElementClicked;

        public ElementType Type => _elementType;

        private void OnEnable()
        {
            _button.onClick.AddListener(OnElementClicked);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnElementClicked);
        }

        public void StartDisabling()
        {
            _button.enabled = false;
            StopDisabling();
            
            if (_disablingCoroutine != null) return; 

            _disablingCoroutine = DisablingCoroutine();
            StartCoroutine(_disablingCoroutine);
        }

        public void StopDisabling()
        {
            if(_disablingCoroutine == null) return;
            
            StopCoroutine(_disablingCoroutine);
            _disablingCoroutine = null;
        }

        public void Enable()
        {
            gameObject.SetActive(true);
            _button.enabled = true;
            Debug.Log(_button.enabled);
        }

        private IEnumerator DisablingCoroutine()
        {
            gameObject.SetActive(true);

            yield return new WaitForSeconds(_disablingPeriod);
            
            gameObject.SetActive(false);
        }
        
        private void OnElementClicked()
        {
            ElementClicked?.Invoke(this);
        }
    }

    public enum ElementType
    {
        Potato,
        Avocado,
        Mushroom,
        Tomato,
        Apple,
        Eggplant,
        Orange,
        Lemon,
        Kiwi
    }
}
