using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LifeHolder : MonoBehaviour
{
    [SerializeField] private Image[] _images;
    [SerializeField] private Sprite _fullLife;
    [SerializeField] private Sprite _emptyLife;

    public void ResetAllLives()
    {
        foreach (var image in _images)
        {
            image.sprite = _fullLife;
        }
    }

    public void EmptyOneLife()
    {
        var image = _images.FirstOrDefault(image => image.sprite == _fullLife);

        if (image != null)
        {
            image.sprite = _emptyLife;
        }
    }
}