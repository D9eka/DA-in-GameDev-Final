using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class BirdController : MonoBehaviour
{
    [SerializeField] private float _jumpForce;

    private Rigidbody2D _rigidbody;

    private int _score;

    public EventHandler<int> OnIncreaseScore;
    public EventHandler OnDied;

    public static BirdController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        BirdInputReader input = GetComponent<BirdInputReader>();
        input.OnBirdJump += BirdInputReader_OnJump;
        UI.Instance.OnGameStart += UI_OnGameStart;

        if (TryGetComponent(out BirdAgent agent)) 
        {
            agent.OnJump += BirdInputReader_OnJump;
        }

        if (LevelController.Instance.ForceStart)
        {
            GetComponent<PlayerInput>().ActivateInput();
        }
    }

    private void BirdInputReader_OnJump(object sender, EventArgs e)
    {
        _rigidbody.velocity = Vector2.up * _jumpForce;
    }

    private void UI_OnGameStart(object sender, EventArgs e)
    {
        GetComponent<PlayerInput>().ActivateInput();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GetComponent<PlayerInput>().DeactivateInput();
        _rigidbody.velocity = Vector2.zero;

        OnDied?.Invoke(this, EventArgs.Empty);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _score++;
        OnIncreaseScore?.Invoke(this, _score);
    }

    public float GetNormalizedVelocity()
    {
        return _rigidbody.velocity.y / _jumpForce;
    }
}
