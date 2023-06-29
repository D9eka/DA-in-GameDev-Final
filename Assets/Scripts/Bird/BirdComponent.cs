using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BirdComponent : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;

    private Rigidbody2D _rigidbody;
    private float _gravityScale;

    private void Awake()
    {
        GetComponent<PlayerInput>().DeactivateInput();
        _rigidbody = GetComponent<Rigidbody2D>();
        _gravityScale = _rigidbody.gravityScale;
        _rigidbody.gravityScale = 0;
    }

    private void FixedUpdate()
    {
        if(_rigidbody.gravityScale != 0)
            _rigidbody.velocity = new Vector2(Time.fixedDeltaTime * _speed, _rigidbody.velocity.y);
    }

    public void Jump()
    {
        _rigidbody.gravityScale = _gravityScale;
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0);
        _rigidbody.totalForce = new Vector2(0, _jumpForce);
    }
}
