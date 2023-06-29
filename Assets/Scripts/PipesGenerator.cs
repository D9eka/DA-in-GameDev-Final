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
            SpawnPipe(_lastSpawnedPosition +  _spaceBetweenPipes);
        }
    }

    private void SpawnPipe(float pipePosX)
    {
        var height = FindHeight() - _spaceBetweenTopAndBottomPipes;
        if (height <= 2)
            throw new Exception("Height <= 2!");

        var pipeHandler = Instantiate(_pipeHandler, transform);

        var topPipeHeight = Random.Range(1, height - 1);
        CreatePipe(pipeHandler, topPipeHeight,
            new Vector2(pipePosX, (height + _spaceBetweenTopAndBottomPipes - topPipeHeight) / 2));

        var bottomPipeHeight = height - topPipeHeight;
        CreatePipe(pipeHandler, bottomPipeHeight,
            new Vector2(pipePosX, -(height + _spaceBetweenTopAndBottomPipes - bottomPipeHeight) / 2));

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

    private void CreatePipe(Pipe pipeHandler, float height, Vector2 position)
    {
        var pipe = Instantiate(_pipeBase, pipeHandler.transform);
        pipe.transform.localScale = new Vector3(1, height, 1);
        pipe.transform.position = position;
        pipeHandler.AddPipe(pipe);
    }
}
