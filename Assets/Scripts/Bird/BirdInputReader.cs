using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class BirdInputReader : MonoBehaviour
{
    [SerializeField] private BirdController _bird;

    public EventHandler OnBirdJump;
    public EventHandler<float> OnChangeTimeScale;

    public void OnJump(InputAction.CallbackContext context)
    {
        OnBirdJump?.Invoke(this, EventArgs.Empty);
    }

    public void OnTimeScaleChangeX2(InputAction.CallbackContext context)
    {
        OnChangeTimeScale?.Invoke(this, 2f);
    }
    public void OnTimeScaleChangeX5(InputAction.CallbackContext context)
    {
        OnChangeTimeScale?.Invoke(this, 5f);
    }
    public void OnTimeScaleChangeX10(InputAction.CallbackContext context)
    {
        OnChangeTimeScale?.Invoke(this, 10f);
    }
}
