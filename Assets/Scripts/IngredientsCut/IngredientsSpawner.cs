using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace IngredientsCut
{
    public class IngredientsSpawner : ObjectPool<Ingredient>
    {
        [SerializeField] private List<Ingredient> _prefabs;
        [SerializeField] private int _capacity;
        [SerializeField] private int _spawnCount;

        private List<Ingredient> _spawnedObjects = new();
        private IEnumerator _spawnCoroutine;
        public event Action IngredientComplete;
        public event Action Spawned;

        private void Awake()
        {
            for (int i = 0; i < _capacity; i++)
            {
                var randomIndex = Random.Range(0, _prefabs.Count);
                Initalize(_prefabs[randomIndex]);
            }
        }

        public void StartSpawn()
        {
            StopSpawn();

            _spawnCoroutine = SpawningCoroutine();
            StartCoroutine(_spawnCoroutine);
        }

        public void StopSpawn()
        {
            if (_spawnCoroutine == null)
                return;

            StopCoroutine(_spawnCoroutine);
            _spawnCoroutine = null;
        }

        private IEnumerator SpawningCoroutine()
        {
            while (true)
            {
                SpawnIngredient();
                yield return null;
            }
        }

        public void ActivateIngredients(int count)
        {
            for (int i = 0; i < count; i++)
            {
                SpawnIngredient();
            }

            Spawned?.Invoke();
        }

        private void SpawnIngredient()
        {
            if (_spawnedObjects.Count >= _capacity)
                return;

            var randomIndex = Random.Range(0, _prefabs.Count);
            Ingredient prefabToSpawn = _prefabs[randomIndex];

            if (TryGetObject(out Ingredient ingredient, prefabToSpawn))
            {
                ingredient.OnProgressComplete += OnIngredientComplete;
                _spawnedObjects.Add(ingredient);
            }
        }

        public void ReturnToPool(Ingredient ingredient)
        {
            if (ingredient == null)
                return;

            ingredient.OnProgressComplete -= OnIngredientComplete;
            PutObject(ingredient);

            if (_spawnedObjects.Contains(ingredient))
                _spawnedObjects.Remove(ingredient);
        }

        public Ingredient GetFirstIngredient()
        {
            return _spawnedObjects.FirstOrDefault(o => o.isActiveAndEnabled);
        }

        public void ReturnAllObjectsToPool()
        {
            if (_spawnedObjects.Count <= 0)
                return;

            List<Ingredient> objectsToReturn = new List<Ingredient>(_spawnedObjects);
            foreach (var obj in objectsToReturn)
            {
                ReturnToPool(obj);
            }
        }

        private void OnIngredientComplete(Ingredient ingredient)
        {
            ReturnToPool(ingredient);
            IngredientComplete?.Invoke();
        }
    }
}