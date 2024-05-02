using System.Collections;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private float _fallHeight;
    [SerializeField] private float _maxPositionOffset;

    private BaseCube _prefabObject;
    private ObjectPool<BaseCube> _objectsPool;

    private void Awake()
    {
        _prefabObject = Resources.Load<BaseCube>("Prefabs/Cube");
        _objectsPool = new ObjectPool<BaseCube>(_prefabObject, Create, Enable, Disable);
    }

    private void OnDisable()
    {
        _objectsPool.ReturnAll();
    }

    private void Start()
    {
        StartCoroutine(SpawnObjects());
    }

    private BaseCube Create(BaseCube prefab)
    {
        var item = Instantiate<BaseCube>(prefab);
        item.transform.SetParent(transform);

        return item;
    }

    private void Enable(BaseCube cube)
    {
        cube.gameObject.SetActive(true);
        cube.TimeEnded += OnLifetimeEnded;
    }

    private void Disable(BaseCube cube)
    {
        cube.TimeEnded -= OnLifetimeEnded;
        cube.gameObject.SetActive(false);
    }

    private void OnLifetimeEnded(BaseCube cube)
    {
        _objectsPool.Return(cube);
    }

    private IEnumerator SpawnObjects()
    {
        const float Second = 0.3f;
        const bool IsWork = true;
        var spawnDelay = new WaitForSeconds(Second);

        while (IsWork)
        {
            float posX = Random.Range(-_maxPositionOffset, _maxPositionOffset);
            float posZ = Random.Range(-_maxPositionOffset, _maxPositionOffset);
            Vector3 position = new Vector3(posX, _fallHeight, posZ);

            var cube = _objectsPool.Get();
            cube.StartInitialization(position);

            yield return spawnDelay;
        }
    }
}
