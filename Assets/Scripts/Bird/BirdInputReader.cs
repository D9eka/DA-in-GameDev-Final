using UnityEngine;
using UnityEngine.InputSystem;

public class BirdInputReader : MonoBehaviour
{
    [SerializeField] private BirdComponent _bird;

    public void OnJump(InputAction.CallbackContext context)
    {
        _bird.Jump();
    }
}
