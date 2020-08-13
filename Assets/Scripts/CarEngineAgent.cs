using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine.UI;
using UnityEngine.UIElements;

public enum Axel
{
    Front,
    Rear
}

[Serializable]
public struct Wheel
{
    public GameObject model;
    public WheelCollider collider;
    public Axel axel;
}

[System.Serializable]
public struct Sensor
{
    public Transform Transform;
    public float HitThreshold;
}

public class CarEngineAgent : Agent
{

    [SerializeField]
    private float maxAcceleration = 20.0f;
    [SerializeField]
    private float turnSensitivity = 1.0f;
    [SerializeField]
    private float maxSteerAngle = 45.0f;
    [SerializeField]
    private Vector3 centerOfMass;
    [SerializeField]
    private List<Wheel> wheels;

    public bool resetOnCollision;
    public int hitWall;
    [HideInInspector]
    public float score = 0;   

    public Collider[] Colliders;

    private Rigidbody rb;
    private Transform track;
    public Text scoreText;
    int checkpointIndex;
    int hit;
    float time;
    Vector3 position;

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
        GetTrackIncrement();
        rb.centerOfMass = centerOfMass;
        checkpointIndex = -1;
        position = transform.localPosition;
    }

    private void Move(float vertical)
    {
        float acceleration;
        if (rb.velocity.magnitude < 10)
            acceleration = maxAcceleration * 3;
        else if (rb.velocity.magnitude < 15)
            acceleration = maxAcceleration * 2;
        else if (rb.velocity.magnitude < 20)
            acceleration = maxAcceleration;
        else
            acceleration = maxAcceleration / 2;

        foreach (var wheel in wheels)
        {
            if(vertical > 0)
                wheel.collider.motorTorque = vertical * acceleration * 500 * Time.deltaTime;
            else
                wheel.collider.motorTorque = vertical * 5 * maxAcceleration * 500 * Time.deltaTime;            
        }
    }

    private void Turn(float horizontal)
    {
        foreach (var wheel in wheels)
        {
            if (wheel.axel == Axel.Front)
            {
                var steerAngle = horizontal * turnSensitivity * maxSteerAngle;
                wheel.collider.steerAngle = Mathf.Lerp(wheel.collider.steerAngle, steerAngle, 0.5f);
            }
        }
    }
    private void Brake(float brake)
    {        
        foreach (var wheel in wheels)
        {
            if (rb.velocity.magnitude >= 10.0f || rb.velocity.magnitude <= -10.0f)
            {
                wheel.collider.brakeTorque = 30000 * brake;
                if (wheel.collider.brakeTorque > 0 && brake > 0)
                {
                    score -= 0.0001f;
                    AddReward(-0.0001f);                   
                }
            }
            else { wheel.collider.brakeTorque = 0; }
            
        }
    }
    public override void OnActionReceived(float[] vectorAction)
    {
        float horizontal = vectorAction[0];
        float vertical = vectorAction[1];
        /*float space = vectorAction[2];*/

        AnimateWheels();        
        Move(vertical);
        Turn(horizontal);
        /*Brake(space);*/

        var lastPos = transform.position;        

        int reward = GetTrackIncrement();

        var moveVec = transform.position - lastPos;
        float angle = Vector3.Angle(moveVec, track.forward);
        float bonus = (1f - angle / 90f) * Mathf.Clamp01(vertical) * Time.fixedDeltaTime;
        AddReward((bonus + reward)/10);

        score += reward;
        if(scoreText != null)
        {
            scoreText.text = score.ToString();
        }   
        
        if(vertical > 0)
        {
            AddReward(rb.velocity.magnitude * 0.001f);
            score += rb.velocity.magnitude * 0.001f;
        }else
        {
            AddReward(-rb.velocity.magnitude * 0.001f);
            score += -rb.velocity.magnitude * 0.001f;
        }

        if (rb.velocity.magnitude < 0.5 && time == 0)
        {
            AddReward(-0.01f);
            score -= 0.01f;
            time = 1;           
        }else if(rb.velocity.magnitude > 0.5)
        {
            time = 0;
        }

        if (time >= 1)
        {
            time += Time.deltaTime;
            if (GetTrackIncrement() == 1 || GetTrackIncrement() == -1)
                time = 0;
            if (time >= 5)
            {
                transform.localPosition = track.localPosition + new Vector3(0,1f);
                time = 0;                    
                rb.velocity = Vector3.zero;
                transform.localRotation = track.localRotation;
            }               
        }
    }

    private void AnimateWheels()
    {
        foreach (var wheel in wheels)
        {
            Quaternion rot;
            Vector3 pos;
            wheel.collider.GetWorldPose(out pos, out rot);
            wheel.model.transform.position = pos;
            wheel.model.transform.rotation = rot;
        }
    }
    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = Input.GetAxis("Horizontal");
        actionsOut[1] = Input.GetAxis("Vertical");
        /*actionsOut[2] = Input.GetAxis("Jump");*/
    }

    public override void CollectObservations(VectorSensor vectorSensor)
    {
        float angle = Vector3.SignedAngle(track.forward, transform.forward, Vector3.up);
        var next = (checkpointIndex + 1) % Colliders.Length;
        var nextCollider = Colliders[next];
        var direction = (nextCollider.transform.position - rb.transform.position).normalized;

        vectorSensor.AddObservation(angle / 180f);
        vectorSensor.AddObservation(rb.velocity.normalized);
        vectorSensor.AddObservation(direction);
        vectorSensor.AddObservation(transform);
    
        Debug.DrawLine(transform.position, nextCollider.transform.position, Color.magenta);
    }
    private int GetTrackIncrement()
    {
        int reward = 0;
        var carCenter = transform.position + Vector3.up;

        // Find what tile I'm on
        if (Physics.Raycast(carCenter, Vector3.down, out var hit, 2f))
        {
            var newHit = hit.transform;
            // Check if the tile has changed
            if (track != null && newHit != track)
            {
                float angle = Vector3.Angle(track.forward, newHit.position - track.position);
                reward = (angle < 90f) ? 1 : -1;                
            }
            track = newHit;
        }
        return reward;
    }
    public override void OnEpisodeBegin()
    {
        foreach (var wheel in wheels)        
            wheel.collider.motorTorque = 0;

        hit = 0;
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        transform.localPosition = position;
        transform.localRotation = Quaternion.identity;        
        checkpointIndex = -1;
        score = 0;
        rb.isKinematic = false;
        time = 0;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("wall") && resetOnCollision)
        {
            AddReward(-1f);
            hit++;            
            if (hit >= hitWall)
            {
                SetReward(-4f);
                EndEpisode();
            }
        }
    }          

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("meta"))
        {    
            checkpointIndex = 0;
        }

        if (other.gameObject.CompareTag("checkpoint"))
        {
            checkpointIndex++;
        }
    }

    public int CheckPointIndex
    {
        get { return checkpointIndex; }
    }
}
