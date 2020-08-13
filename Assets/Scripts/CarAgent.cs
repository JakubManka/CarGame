﻿using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class CarAgent : Agent
{

    public float speed;
    public float maxSpeed;
    public float torque;      


    [HideInInspector]
    public float speedText;
    [HideInInspector]
    public int score = 0;
    public bool resetOnCollision = true;
    private Transform track;
    private Rigidbody rb;

    

    public override void Initialize()
    {
        GetTrackIncrement();
        rb = GetComponent<Rigidbody>();
    }

    private void MoveCar(float horizontal, float vertical, float dt)
    {
        float distance = speed * vertical;
        /*transform.Translate(distance * dt * Vector3.forward);*/
        rb.AddForce(distance *  transform.forward);
        
        if(speed < maxSpeed && speed != 0)
        {
            speed += 0.2f;
        }


        float rotation = horizontal * torque * 90f;
        transform.Rotate(0f, rotation * dt, 0f);

        var vel = rb.velocity;
        speedText = vel.magnitude;
        print(distance * dt * Vector3.forward);
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        float horizontal = vectorAction[0];
        float vertical = vectorAction[1];

        var lastPos = transform.position;
        MoveCar(horizontal, vertical, Time.fixedDeltaTime);

        int reward = GetTrackIncrement();

        var moveVec = transform.position - lastPos;
        float angle = Vector3.Angle(moveVec, track.forward);
        float bonus = (1f - angle / 90f) * Mathf.Clamp01(vertical) * Time.fixedDeltaTime;
        AddReward(bonus + reward);

        score += reward;
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = Input.GetAxis("Horizontal");
        actionsOut[1] = Input.GetAxis("Vertical");     
    }
   

    public override void CollectObservations(VectorSensor vectorSensor)
    {
        float angle = Vector3.SignedAngle(track.forward, transform.forward, Vector3.up);

        vectorSensor.AddObservation(angle / 180f);
        vectorSensor.AddObservation(ObserveRay(1.5f, .5f, 25f));
        vectorSensor.AddObservation(ObserveRay(1.5f, 0f, 0f));
        vectorSensor.AddObservation(ObserveRay(1.5f, -.5f, -25f));
        vectorSensor.AddObservation(ObserveRay(-1.5f, 0, 180f));
    }

    private float ObserveRay(float z, float x, float angle)
    {
        var tf = transform;

        // Get the start position of the ray
        var raySource = tf.position + Vector3.up / 2f;
        const float RAY_DIST = 5f;
        var position = raySource + tf.forward * z + tf.right * x;

        // Get the angle of the ray
        var eulerAngle = Quaternion.Euler(0, angle, 0f);
        var dir = eulerAngle * tf.forward;

        // See if there is a hit in the given direction
        Physics.Raycast(position, dir, out var hit, RAY_DIST);
        return hit.distance >= 0 ? hit.distance / RAY_DIST : -1f;
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
        if (resetOnCollision)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("wall"))
        {
            SetReward(-1f);
            EndEpisode();
        }
    }
}