using System;
using UnityEngine;

public class TimeComponent : MonoBehaviour
{
    public static TimeComponent Instance {  get; private set; }

    private void Start()
    {
        Instance = this;
        Pause();

        UI.Instance.OnGameStart += UI_OnGameStart;
        BirdController.Instance.OnDied += BirdComponent_OnDied;

        if(LevelController.Instance.ForceStart)
        {
            Resume();
        }
    }

    private void UI_OnGameStart(object sender, EventArgs e)
    {
        Resume();
    }

    private void BirdComponent_OnDied(object sender, EventArgs e)
    {
        Pause();
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
