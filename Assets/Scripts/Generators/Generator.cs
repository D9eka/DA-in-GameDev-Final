using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Generator : MonoBehaviour
{
    [SerializeField] protected GameObject _prefab;
    [SerializeField] protected float _speed;

    protected float _prefabWidth;

    protected float _cameraLeftEdge;
    protected float _cameraRightEdge;
    protected float _cameraWidth;
    protected float _cameraHeight;

    protected bool _active;

    protected List<Transform> _spawnedPrefabs = new List<Transform>();

    protected virtual void Start()
    {
        Vector2 cameraBottomLeftAngle = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        Vector2 cameraTopRightAngle = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, Camera.main.nearClipPlane));
        _cameraLeftEdge = cameraBottomLeftAngle.x;
        _cameraRightEdge = cameraTopRightAngle.x;
        _cameraWidth = _cameraRightEdge - _cameraLeftEdge;
        _cameraHeight = cameraTopRightAngle.y - cameraBottomLeftAngle.y;

        UI.Instance.OnGameStart += UI_OnGameStart;
        LevelController.Instance.OnChangeDifficulty += Level_OnChangeDifficulty;
        BirdController.Instance.OnDied += BirdComponent_OnDied;
    }

    private void UI_OnGameStart(object sender, EventArgs e)
    {
        _active = true;
    }

    protected virtual void Level_OnChangeDifficulty(object sender, LevelDifficulty e)
    {
        _speed = e.Speed;
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

            if (prefab.position.x < _cameraLeftEdge - _prefabWidth)
            {
                Destroy(prefab.gameObject);
                _spawnedPrefabs.Remove(prefab);
            }
        }
    }
}