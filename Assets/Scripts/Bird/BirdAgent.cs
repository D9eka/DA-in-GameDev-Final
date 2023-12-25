using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System;
using UnityEngine.InputSystem;

public class BirdAgent : Agent
{
    private BirdController bird;

    private bool isJumpInputDown;

    public EventHandler OnEpisodeBeginEvent;
    public EventHandler OnJump;

    private void Awake()
    {
        bird = GetComponent<BirdController>();
    }

    private void Start()
    {
        bird.OnIncreaseScore += Bird_OnIncreaseScore;
        bird.OnDied += Bird_OnDied;
    }

    private void Bird_OnIncreaseScore(object sender, int e)
    {
        AddReward(1f);
    }

    private void Bird_OnDied(object sender, EventArgs e)
    {
        EndEpisode();

        ReloadLevelComponent.Reload();
    }

    /*
    private void Update()
    {
        isJumpInputDown = Keyboard.current.spaceKey.wasPressedThisFrame || 
                          Mouse.current.leftButton.wasPressedThisFrame;
    }
    */

    public override void OnEpisodeBegin()
    {
        OnEpisodeBeginEvent?.Invoke(this, EventArgs.Empty);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        LevelController level = LevelController.Instance;
        float birdNormalizedY = (bird.transform.position.y + (level.Height / 2f)) / level.Height;
        sensor.AddObservation(birdNormalizedY);

        PipesGenerator pipesGenerator = PipesGenerator.Instance;
        float pipeSpawnPosition = pipesGenerator.SpawnPosition;
        float? nextPipePosition = pipesGenerator.GetNextPipePosition();
        if (nextPipePosition != null)
        {
            sensor.AddObservation(nextPipePosition.Value / pipeSpawnPosition);
        }
        else
        {
            sensor.AddObservation(1f);
        }

        sensor.AddObservation(bird.GetNormalizedVelocity());
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        if (actions.DiscreteActions[0] == 1)
        {
            OnJump?.Invoke(this, EventArgs.Empty);
        }

        Debug.Log(GetCumulativeReward());
    }

    /*
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = isJumpInputDown ? 1 : 0;

        isJumpInputDown = false;
    }
    */
}
