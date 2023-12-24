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

    public static PipesGenerator Instance { get; private set; }

    protected override void Start()
    {
        base.Start();

        Instance = this;

        _prefabWidth = _prefab.transform.localScale.x;
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
        float pipePosX = _cameraRightEdge + _prefabWidth;

        float height = _cameraHeight - _spaceBetweenPipes;

        GameObject pipeHandler = Instantiate(_pipeHandler, new Vector2(pipePosX, 0), Quaternion.identity, transform);

        float topPipeHeight = Random.Range(_minPipeHeight, height - _minPipeHeight);
        CreatePipe(pipeHandler,
            new Vector2(pipePosX, (_cameraHeight - topPipeHeight) / 2),
            topPipeHeight);

        GameObject scoreTrigger = Instantiate(_scoreTrigger, new Vector2(pipePosX, (_cameraHeight - _spaceBetweenPipes) / 2 - topPipeHeight), Quaternion.identity, pipeHandler.transform);
        scoreTrigger.transform.localScale = new Vector2(scoreTrigger.transform.localScale.x, _spaceBetweenPipes);

         float bottomPipeHeight = height - topPipeHeight;
        CreatePipe(pipeHandler,
            new Vector2(pipePosX, -(_cameraHeight - bottomPipeHeight) / 2),
            bottomPipeHeight, true);

        _spawnedPrefabs.Add(pipeHandler.transform);
    }

    private void CreatePipe(GameObject pipeHandler, Vector2 position, float height, bool flipY = false)
    {
        GameObject pipe = Instantiate(_prefab, position, Quaternion.identity, pipeHandler.transform);
        pipe.GetComponent<SpriteRenderer>().size = new Vector2(1, height);
        pipe.GetComponent<SpriteRenderer>().flipY = flipY;
        pipe.GetComponent<BoxCollider2D>().size = new Vector2(1, height);
    }

    public PipeConfig GetNextPipe()
    {
        if (_spawnedPrefabs.Count == 0)
            return null;

        for (int i = 0; i < _spawnedPrefabs.Count; i++)
        {
            GameObject pipe = _spawnedPrefabs[i].gameObject;
            if (pipe.transform.position.x > BirdController.Instance.transform.position.x)
            {
                Transform pipeGap = pipe.GetComponentsInChildren<BoxCollider2D>()[1].transform;
                return new PipeConfig(pipeGap.position.y + pipeGap.localScale.y / 2, pipeGap.position.y - pipeGap.localScale.y / 2);
            }
        }
        return null;
    }
}

public class PipeConfig
{
    public float pipeGapTop { get; private set; }
    public float pipeGapBottom { get; private set; }

    public PipeConfig(float pipeGapTop, float pipeGapBottom)
    {
        this.pipeGapTop = pipeGapTop;
        this.pipeGapBottom = pipeGapBottom;
    }
}