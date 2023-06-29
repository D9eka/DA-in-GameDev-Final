using UnityEngine;

public class TimeComponent : MonoBehaviour
{
    private void Start()
    {
        Resume();
    }

    [ContextMenu("Resume")]
    public void Resume()
    { 
        Time.timeScale = 1;
    }

    [ContextMenu("Pause")]
    public void Pause()
    {
        Time.timeScale = 0;
    }
}
