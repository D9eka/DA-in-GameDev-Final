using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PipesGenerator : MonoBehaviour
{
    [SerializeField] private Pipe _pipeHandler;
    [SerializeField] private GameObject _pipeBase;
    [SerializeField] private float _spaceBetweenTopAndBottomPipes;
    [Space]
    [SerializeField] private float _startPosition;
    [SerializeField] private float _spaceBetweenPipes;

    private Pipe _lastSpawnedPipe;
    private float _lastSpawnedPosition;

    private void Start()
    {
        SpawnPipe(_startPosition);
    }
    private void Update()
    {
        while (_lastSpawnedPipe.IsVisible)
        { 
            SpawnPipe(_lastSpawnedPosition +  _spaceBetweenPipes +
                _lastSpawnedPipe.GetComponentInChildren<SpriteRenderer>().size.x);
        }
    }

    private void SpawnPipe(float pipePosX)
    {
        var height = FindHeight() - _spaceBetweenTopAndBottomPipes;
        if (height <= 2)
            throw new Exception("Height <= 2!");

        var pipeHandler = Instantiate(_pipeHandler, transform);

        var topPipeHeight = Random.Range(1, height - 1);
        CreatePipe(pipeHandler, 
            new Vector2(pipePosX, (height + _spaceBetweenTopAndBottomPipes - topPipeHeight) / 2),
            topPipeHeight);

        var bottomPipeHeight = height - topPipeHeight;
        CreatePipe(pipeHandler,
            new Vector2(pipePosX, -(height + _spaceBetweenTopAndBottomPipes - bottomPipeHeight) / 2),
            bottomPipeHeight, true);

        _lastSpawnedPipe = pipeHandler;
        _lastSpawnedPosition = pipePosX;
    }
    private float FindHeight()
    {
        var camBottomLeftCorner = Camera.main.ViewportToWorldPoint(
            new Vector3(0, 0, Camera.main.nearClipPlane));
        var camTopRightCorner = Camera.main.ViewportToWorldPoint(
            new Vector3(1f, 1f, Camera.main.nearClipPlane));
        return camTopRightCorner.y - camBottomLeftCorner.y;
    }

    private void CreatePipe(Pipe pipeHandler, Vector2 position, float height, bool flipY = false)
    {
        var pipe = Instantiate(_pipeBase, position, Quaternion.identity, pipeHandler.transform);
        pipe.GetComponent<SpriteRenderer>().size = new Vector2(1, height);
        pipe.GetComponent<SpriteRenderer>().flipY = flipY;
        pipe.GetComponent<BoxCollider2D>().size = new Vector2(1, height);
        pipeHandler.AddPipe(pipe);
    }
}
