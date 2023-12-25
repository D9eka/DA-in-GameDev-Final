using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Generator : MonoBehaviour
{
    [SerializeField] protected GameObject _prefab;
    [SerializeField] protected float _speed;

    protected float _prefabWidth;

    protected float _levelLeftEdge;
    protected float _levelRightEdge;
    protected float _levelWidth;
    protected float _levelHeight;

    protected bool _active;

    protected List<Transform> _spawnedPrefabs = new List<Transform>();

    protected virtual void Start()
    {
        GetLevelParams();
        Initialize();
        
        UI.Instance.OnGameStart += UI_OnGameStart;
        LevelController.Instance.OnChangeDifficulty += Level_OnChangeDifficulty;
        BirdController.Instance.OnDied += BirdComponent_OnDied;

        if(LevelController.Instance.ForceStart)
        {
            _active = true;
        }
    }

    private void GetLevelParams()
    {
        LevelController level = LevelController.Instance;
        _levelLeftEdge = level.LeftEdge;
        _levelRightEdge = level.RightEdge;
        _levelWidth = level.Width;
        _levelHeight = level.Height;
    }

    protected virtual void Initialize()
    {
        LevelDifficulty values = LevelController.Instance.GetInitialValues();

        _speed = values.Speed;
    }

    private void UI_OnGameStart(object sender, EventArgs e)
    {
        _active = true;
    }

    protected virtual void Level_OnChangeDifficulty(object sender, LevelDifficulty e)
    {
        _speed = e.Speed;
        Debug.Log("Changed");
    }

    private void BirdComponent_OnDied(object sender, EventArgs e)
    {
        _active = false;
    }

    protected virtual void Update()
    {
        if (!_active)
            return;
    }

    protected virtual void FixedUpdate()
    {
        MovePrefabs();
    }

    protected abstract void SpawnPrefab();

    protected void MovePrefabs()
    {
        for (int i = 0; i < _spawnedPrefabs.Count; i++)
        {
            Transform prefab = _spawnedPrefabs[i];

            Vector3 tempVect = Vector2.left * _speed * Time.fixedDeltaTime;
            prefab.GetComponent<Rigidbody2D>().MovePosition(prefab.position + tempVect);

            if (prefab.position.x < _levelLeftEdge - _prefabWidth)
            {
                Destroy(prefab.gameObject);
                _spawnedPrefabs.Remove(prefab);
            }
        }
    }
}