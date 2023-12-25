using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class BirdInputReader : MonoBehaviour
{
    [SerializeField] private BirdController _bird;

    public EventHandler OnBirdJump;

    public void OnJump(InputAction.CallbackContext context)
    {
        OnBirdJump?.Invoke(this, EventArgs.Empty);
    }
}
