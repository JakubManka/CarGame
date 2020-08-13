using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Car : MonoBehaviour
{
    public float speed = 10f;
    public float torque = 1f;

    [HideInInspector]
    public float score = 0;

    private Transform track;

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float dt = Time.deltaTime;
        MoveCar(horizontal, vertical, dt);
        score += GetTrackIncrement();
        
    }

    private void MoveCar(float horizontal, float vertical, float dt)
    {
        // Translated in the direction the car is facing
        float moveDist = speed * vertical;
        transform.Translate(dt * moveDist * Vector3.forward);

        // Rotate alongside it up axis 
        float rotation = horizontal * torque * 90f;
        transform.Rotate(0f, rotation * dt, 0f);
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
}