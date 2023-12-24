using System.Collections;
using System.Linq;
using UnityEngine;


public class MidgroundGenerator : Generator
{
    [SerializeField] private GameObject _midgroundHandler;
    [SerializeField] private GameObject _topColliderPrefab;

    public static MidgroundGenerator Instance { get; private set; }

    private float _prefabHeight;

    protected override void Start()
    {
        base.Start();

        Instance = this;
        
        _prefab.GetComponent<SpriteRenderer>().size = new Vector2(_cameraWidth, _prefab.GetComponent<SpriteRenderer>().size.y);
        _prefab.GetComponent<BoxCollider2D>().size = new Vector2(_cameraWidth, _prefab.GetComponent<BoxCollider2D>().size.y);
        _topColliderPrefab.transform.localScale = new Vector2(_cameraWidth, _topColliderPrefab.transform.localScale.y);
        _prefabWidth = _cameraWidth;
        _prefabHeight = _prefab.GetComponent<SpriteRenderer>().size.y;

        SpawnPrefab();
    }

    protected override void Update()
    {
        SpawnPrefab();
    }

    protected override void SpawnPrefab()
    {

        if (_spawnedPrefabs.Count == 0)
        {
            Spawn(_cameraLeftEdge + _prefabWidth / 2);
        }

        while (_spawnedPrefabs.Last().position.x < _cameraRightEdge)
        {
            Spawn(_spawnedPrefabs.Last().position.x + _prefabWidth);
        }
    }

    private void Spawn(float positionX)
    {
        Transform midgroundHandler = Instantiate(_midgroundHandler, new Vector2(positionX, 0),
                                                  Quaternion.identity, transform).transform;

        Instantiate(_prefab,
                    new Vector2(positionX, (-_cameraHeight + _prefabHeight) / 2), Quaternion.identity, midgroundHandler);
        Instantiate(_topColliderPrefab,
                    new Vector2(positionX, (_cameraHeight + _prefabHeight) / 2), Quaternion.identity, midgroundHandler);
        _spawnedPrefabs.Add(midgroundHandler.transform);
    }
}