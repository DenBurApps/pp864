using UnityEngine;

namespace Gathering
{
    public class PoolReturner : MonoBehaviour
    {
        [SerializeField] private FruitSpawner _spawner;
    
        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.TryGetComponent(out Fruit @object))
            {
                _spawner.ReturnToPool(@object);
            }
        }
    }
}
