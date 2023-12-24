using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System;
using UnityEngine.InputSystem;

public class BirdAgent : Agent
{
    private BirdController bird;

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

    private void Bird_OnDied(object sender, System.EventArgs e)
    {
        EndEpisode();

        new ReloadLevelComponent().Reload();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        Vector2 cameraBottomLeftAngle = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        Vector2 cameraTopRightAngle = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, Camera.main.nearClipPlane));
        float cameraHeight = cameraTopRightAngle.y - cameraBottomLeftAngle.y;
        float birdNormalizedY = (bird.transform.position.y + (cameraHeight / 2f)) / cameraHeight;
        sensor.AddObservation(birdNormalizedY);

        PipeConfig nextPipe = PipesGenerator.Instance.GetNextPipe();
        if (nextPipe != null)
        {
            sensor.AddObservation(nextPipe.pipeGapTop);
            sensor.AddObservation(nextPipe.pipeGapBottom);
        }
        else
        {
            sensor.AddObservation(1f);
            sensor.AddObservation(1f);
        }

        sensor.AddObservation(bird.GetComponent<Rigidbody2D>().velocity.y / cameraHeight);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        if (actions.DiscreteActions[0] == 1)
        {
            OnJump?.Invoke(this, EventArgs.Empty);
        }

        Debug.Log(GetCumulativeReward());
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = Keyboard.current.spaceKey.IsPressed() ? 1 : 0;
    }

}
