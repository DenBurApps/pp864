using UnityEngine;

namespace Gathering
{
    public class FruitSpriteHolder : MonoBehaviour
    {
        [SerializeField] private Sprite[] _goodFruitSprites;
        [SerializeField] private Sprite[] _badFruitSprite;

        public Sprite GetGoodFruitSprite()
        {
            var randomIndex = Random.Range(0, _goodFruitSprites.Length);

            return _goodFruitSprites[randomIndex];
        }
        
        public Sprite GetBadFruitSprite()
        {
            var randomIndex = Random.Range(0, _badFruitSprite.Length);

            return _badFruitSprite[randomIndex];
        }
    }
}
