using UnityEngine;
using Random = UnityEngine.Random;

public class PipesGenerator : Generator
{
    [SerializeField] private GameObject _pipeHandler;
    [SerializeField] private GameObject _scoreTrigger;
    [Space]
    [SerializeField] private float _minPipeHeight;
    [SerializeField] private float _spaceBetweenPipes;
    [SerializeField] private float _pipeSpawnDelay;

    private float _pipeSpawnCounter;

    public float SpawnPosition { get; private set; }

    public static PipesGenerator Instance { get; private set; }

    protected override void Start()
    {
        base.Start();

        Instance = this;

        _prefabWidth = _prefab.transform.localScale.x;
        SpawnPosition = _levelRightEdge + _prefabWidth;
    }

    protected override void Initialize()
    {
        LevelDifficulty values = LevelController.Instance.GetInitialValues();

        _speed = values.Speed;
        _pipeSpawnDelay = values.PipeSpawnDelay;
        _spaceBetweenPipes = values.SpaceBetweenPipes;
    }

    protected override void Level_OnChangeDifficulty(object sender, LevelDifficulty e)
    {
        base.Level_OnChangeDifficulty(sender, e);

        _spaceBetweenPipes = e.SpaceBetweenPipes;
        _pipeSpawnDelay = e.PipeSpawnDelay;
    }

    protected override void Update()
    {
        base.Update();

        _pipeSpawnCounter -= Time.deltaTime;

        if(_pipeSpawnCounter < 0)
        {
            SpawnPrefab();
            _pipeSpawnCounter = _pipeSpawnDelay;
        }
    }

    protected override void FixedUpdate()
    {
        if (!_active)
            return;

        base.FixedUpdate();
    }

    protected override void SpawnPrefab()
    {
        float height = _levelHeight - _spaceBetweenPipes;

        GameObject pipeHandler = Instantiate(_pipeHandler, new Vector2(SpawnPosition, 0), Quaternion.identity, transform);

        float topPipeHeight = Random.Range(_minPipeHeight, height - _minPipeHeight);
        CreatePipe(pipeHandler,
            new Vector2(SpawnPosition, (_levelHeight - topPipeHeight) / 2),
            topPipeHeight);

        GameObject scoreTrigger = Instantiate(_scoreTrigger, new Vector2(SpawnPosition, (_levelHeight - _spaceBetweenPipes) / 2 - topPipeHeight), 
                                              Quaternion.identity, pipeHandler.transform);
        scoreTrigger.transform.localScale = new Vector2(scoreTrigger.transform.localScale.x, _spaceBetweenPipes);

         float bottomPipeHeight = height - topPipeHeight;
        CreatePipe(pipeHandler,
            new Vector2(SpawnPosition, -(_levelHeight - bottomPipeHeight) / 2),
            bottomPipeHeight, true);

        _spawnedPrefabs.Add(pipeHandler.transform);
    }

    private void CreatePipe(GameObject pipeHandler, Vector2 position, float height, bool flipY = false)
    {
        GameObject pipe = Instantiate(_prefab, position, Quaternion.identity, pipeHandler.transform);
        SpriteRenderer renderer = pipe.GetComponent<SpriteRenderer>();
        renderer.size = new Vector2(renderer.size.x, height);
        renderer.flipY = flipY;
        BoxCollider2D collider = pipe.GetComponent<BoxCollider2D>();
        collider.size = new Vector2(collider.size.x, height);
    }

    public float? GetNextPipePosition()
    {
        if (_spawnedPrefabs.Count == 0)
            return null;

        for (int i = 0; i < _spawnedPrefabs.Count; i++)
        {
            GameObject pipe = _spawnedPrefabs[i].gameObject;
            if (pipe.transform.position.x > BirdController.Instance.transform.position.x)
            {
                return pipe.transform.position.x;
            }
        }
        return null;
    }
}