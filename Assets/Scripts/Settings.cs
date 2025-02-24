using TMPro;
using UnityEngine;
using DG.Tweening;
#if UNITY_IOS
using UnityEngine.iOS;
#endif

public class Settings : MonoBehaviour
{
    [SerializeField] private GameObject _settingsCanvas;
    [SerializeField] private GameObject _privacyCanvas;
    [SerializeField] private GameObject _termsCanvas;
    [SerializeField] private GameObject _contactCanvas;
    [SerializeField] private GameObject _versionCanvas;
    [SerializeField] private TMP_Text _versionText;
    private string _version = "Application version:\n";
    
    [Header("Animation Settings")]
    [SerializeField] private float _fadeDuration = 0.3f;
    
    private CanvasGroup _settingsCanvasGroup;

    private void Awake()
    {
        _settingsCanvasGroup = _settingsCanvas.GetComponent<CanvasGroup>();
        if (_settingsCanvasGroup == null)
        {
            _settingsCanvasGroup = _settingsCanvas.AddComponent<CanvasGroup>();
        }
        
        _settingsCanvas.SetActive(false);
        _privacyCanvas.SetActive(false);
        _termsCanvas.SetActive(false);
        _contactCanvas.SetActive(false);
        _versionCanvas.SetActive(false);
        
        SetVersion();
    }

    private void SetVersion()
    {
        _versionText.text = _version + Application.version;
    }

    public void ShowSettings()
    {
        DOTween.Kill(_settingsCanvasGroup);
        
        _settingsCanvasGroup.alpha = 0f;
        
        _settingsCanvas.SetActive(true);
        
        _settingsCanvasGroup.DOFade(1f, _fadeDuration);
    }
    
    public void HideSettings()
    {
        _settingsCanvasGroup.DOFade(0f, _fadeDuration)
            .OnComplete(() => {
                _settingsCanvas.SetActive(false);
            });
    }

    public void RateUs()
    {
#if UNITY_IOS
        Device.RequestStoreReview();
#endif
    }
    
    private void OnDestroy()
    {
        if (_settingsCanvasGroup != null)
        {
            DOTween.Kill(_settingsCanvasGroup);
        }
    }
}