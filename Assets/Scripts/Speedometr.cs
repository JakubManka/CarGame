using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Speedometr : MonoBehaviour
{
    public Rigidbody rb;

    private const float maxSpeedAngle = -135;
    private const float zeroSpeedAngle = 137;

    private Transform needleTransform;

    private float speedMax;
    private float speed;

    private void Awake()
    {
        needleTransform = transform.Find("Needle");

        speed = 0f;
        speedMax = 280f;
    }

    private void Update()
    {
        speed = rb.velocity.magnitude * 4;    
        needleTransform.eulerAngles = new Vector3(0, 0, GetSpeedRotation());
    }


    private float GetSpeedRotation()
    {
        float totalAngleSize = zeroSpeedAngle - maxSpeedAngle;

        float speedNormalized = speed / speedMax;

        return zeroSpeedAngle - speedNormalized * totalAngleSize;
    }
}
