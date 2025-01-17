using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gathering
{
    public class FruitSpawner : ObjectPool<Fruit>
    {
        [SerializeField] private float _spawnInterval;
        [SerializeField] private Fruit[] _prefabs;
        [SerializeField] private SpawnArea _spawnArea;
        [SerializeField] private int _poolCapacity;
        [SerializeField] private float _objMovingSpeed;
        [SerializeField] private FruitSpriteHolder _fruitSpriteHolder;

        private List<Fruit> _spawnedObjects = new List<Fruit>();
        private IEnumerator _spawnCoroutine;

        private void Awake()
        {
            for (int i = 0; i <= _poolCapacity; i++)
            {
                ShuffleArray();

                foreach (var prefab in _prefabs)
                {
                    Initalize(prefab);
                }
            }
        }

        public void StartSpawn()
        {
            if (_spawnCoroutine != null) return;
            _spawnCoroutine = StartSpawning();
            StartCoroutine(_spawnCoroutine);
        }

        public void StopSpawn()
        {
            if (_spawnCoroutine == null) return;

            StopCoroutine(_spawnCoroutine);
            _spawnCoroutine = null;
        }

        private IEnumerator StartSpawning()
        {
            WaitForSeconds interval = new WaitForSeconds(_spawnInterval);

            while (true)
            {
                Spawn();
                yield return interval;
            }
        }

        private void Spawn()
        {
            if (ActiveObjects.Count >= _poolCapacity)
                return;

            int randomIndex = Random.Range(0, _prefabs.Length);
            Fruit prefabToSpawn = _prefabs[randomIndex];

            if (TryGetObject(out Fruit fruit, prefabToSpawn))
            {
                fruit.transform.position = _spawnArea.GetRandomXPositionToSpawn();
                _spawnedObjects.Add(fruit);
                fruit.EnableMovement();
                fruit.SetSpeed(_objMovingSpeed);

                fruit.SetSprite(fruit is BadFruit
                    ? _fruitSpriteHolder.GetBadFruitSprite()
                    : _fruitSpriteHolder.GetGoodFruitSprite());
            }
        }

        public void ReturnToPool(Fruit fruit)
        {
            if (fruit == null)
                return;

            fruit.DisableMovement();
            PutObject(fruit);

            if (_spawnedObjects.Contains(fruit))
                _spawnedObjects.Remove(fruit);
        }

        public void IncreaseSpeed()
        {
            _objMovingSpeed += 0.5f;
        }

        public void ReturnAllObjectsToPool()
        {
            if (_spawnedObjects.Count <= 0)
                return;

            List<Fruit> objectsToReturn = new List<Fruit>(_spawnedObjects);
            foreach (var @object in objectsToReturn)
            {
                @object.DisableMovement();
                ReturnToPool(@object);
            }
        }

        private void ShuffleArray()
        {
            for (int i = 0; i < _prefabs.Length - 1; i++)
            {
                Fruit temp = _prefabs[i];
                int randomIndex = Random.Range(0, _prefabs.Length);
                _prefabs[i] = _prefabs[randomIndex];
                _prefabs[randomIndex] = temp;
            }
        }
    }
}