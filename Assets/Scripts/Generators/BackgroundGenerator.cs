using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BackgroundGenerator : Generator
{
    public static BackgroundGenerator Instance { get; private set; }

    protected override void Start()
    {
        base.Start();

        Instance = this;

        var scale = _levelHeight / _prefab.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        _prefab.transform.localScale = new Vector2(scale, scale);
        _prefabWidth = _levelHeight;
        SpawnPrefab();
    }

    protected override void Update()
    {
        SpawnPrefab();
    }

    protected override void SpawnPrefab()
    {
        if(_spawnedPrefabs.Count == 0)
        {
            GameObject background = Instantiate(_prefab, new Vector2(_levelLeftEdge, 0f), Quaternion.identity, transform);
            _spawnedPrefabs.Add(background.transform);
        }

        while (_spawnedPrefabs.Last().position.x < _levelRightEdge)
        {
            GameObject background = Instantiate(_prefab, new Vector2(_spawnedPrefabs.Last().position.x + _prefabWidth, 0f), Quaternion.identity, transform);
            _spawnedPrefabs.Add(background.transform);
        }
    }
}
